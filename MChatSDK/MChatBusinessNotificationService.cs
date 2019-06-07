using System;
using System.Linq;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MChatSDK
{
    class MChatBusinessNotificationServiceBuilder
    {
        public String apiKey;
        public int port;
        public String domain;
        public int timeout;

        public MChatBusinessNotificationService Build()
        {
            MChatBusinessNotificationService service = new MChatBusinessNotificationService(this);
            return service;
        }
    }

    public enum BNSProtocolState
    {
        Authentication,
        Registration,
        PaymentNotification
    }

    public enum BNSProtocolConnectionState
    {
        Connected,
        Disconnected,
        Timeout,
        Authenticated,
        Registered
    }

    class MChatBusinessNotificationService
    {
        private MChatBusinessNotificationServiceBuilder configBuilder;

        private TcpClient client;
        private SslStream nwStream;

        private String protocolVersion;
        private String generatedQRCode;

        public ProtocolResponse protocolResponse;
        public ConnectionState connectionState;

        public delegate void ProtocolResponse(BNSProtocolState state, MChatResponse response);
        public delegate void ConnectionState(MChatBusinessNotificationService service, BNSProtocolConnectionState state);

        public MChatBusinessNotificationService(MChatBusinessNotificationServiceBuilder configBuilder)
        {
            this.configBuilder = configBuilder;
        }

        public void connect(String generatedQRCode)
        {
            this.generatedQRCode = generatedQRCode;
            try
            {
                client = new TcpClient(configBuilder.domain, configBuilder.port);
                nwStream = new SslStream(client.GetStream());
                nwStream.AuthenticateAsClient(configBuilder.domain);
                String packetData = "";
                while (true)
                {
                    if (!nwStream.CanRead)
                    {
                        break;
                    }
                    byte[] bytesToRead = new byte[512];
                    int bytesRead = nwStream.Read(bytesToRead, 0, 512);
                    String read = Encoding.ASCII.GetString(bytesToRead, 0, bytesRead);
                    if (!read.Contains("\r\n"))
                    {
                        packetData += read;
                    }
                    else
                    {
                        string[] tokens = read.Split(new[] { "\r\n" }, StringSplitOptions.None);
                        if (tokens.Length == 2)
                        {
                            packetData += tokens[0];
                            packet(packetData);
                            packetData = tokens[1];
                        }
                    }
                }
            }
            catch (Exception e)
            {
                disconnected();
                //Console.WriteLine(e.ToString());
            }
        }

        private void packet(String data)
        {
            XElement xmlData = XElement.Parse(data);
            switch (xmlData.Name.LocalName)
            {
                case "biznot":
                    protocolVersion = xmlData.Attribute("version").Value;
                    connected();
                    break;
                case "auth":
                    {
                        var response = xmlData.Elements();
                        foreach (XElement element in response)
                        {
                            if (element.Name.LocalName == "response")
                            {
                                MChatResponse mChatResponse = new MChatResponse
                                {
                                    code = int.Parse(element.Attribute("code").Value),
                                    message = element.Value
                                };
                                protocolResponse?.Invoke(BNSProtocolState.Authentication, mChatResponse);
                                if (mChatResponse.code == 200)
                                {
                                    register();
                                }
                            }
                        }
                    }
                    break;
                case "register":
                    {
                        var response = xmlData.Elements();
                        foreach (XElement element in response)
                        {
                            if (element.Name.LocalName == "response")
                            {
                                MChatResponse mChatResponse = new MChatResponse
                                {
                                    code = int.Parse(element.Attribute("code").Value),
                                    message = element.Value
                                };
                                protocolResponse?.Invoke(BNSProtocolState.Registration, mChatResponse);
                            }
                        }
                    }
                    break;
                case "payment":
                    {
                        var response = xmlData.Elements();
                        foreach (XElement element in response)
                        {
                            if (element.Name.LocalName == "response")
                            {
                                MChatResponse mChatResponse = new MChatResponse
                                {
                                    code = int.Parse(element.Attribute("code").Value),
                                    message = element.Value
                                };
                                protocolResponse?.Invoke(BNSProtocolState.PaymentNotification, mChatResponse);
                                Disconnect();
                            }
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        private void authenticate()
        {
            XNamespace ns = "client:auth";
            XElement xmlData = new XElement(ns + "auth", new XAttribute("api_key", configBuilder.apiKey));
            byte[] sendData = Encoding.ASCII.GetBytes(xmlData.ToString() + "\r\n");
            nwStream.Write(sendData, 0, sendData.Length);
            nwStream.Flush();
        }

        private void register()
        {
            XNamespace ns = "client:reg";
            XElement xmlData = new XElement(ns + "register", new XAttribute("code", this.generatedQRCode));
            byte[] sendData = Encoding.ASCII.GetBytes(xmlData.ToString() + "\r\n");
            nwStream.Write(sendData, 0, sendData.Length);
            nwStream.Flush();
        }

        private void connected()
        {
            authenticate();
            connectionState?.Invoke(this, BNSProtocolConnectionState.Connected);
            Task.Run(async () => {
                await Task.Delay(this.configBuilder.timeout);
                if (connectionState != null)
                {
                    connectionState?.Invoke(this, BNSProtocolConnectionState.Timeout);
                    Disconnect();
                }
            });
        }

        private void disconnected()
        {
            connectionState?.Invoke(this, BNSProtocolConnectionState.Disconnected);
            connectionState = null;
        }

        public void Disconnect()
        {
            nwStream.Close();
            nwStream = null;
            client.Close();
            client = null;
            disconnected();
        }
    }
}
