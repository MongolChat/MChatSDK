using System;
using System.Collections;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MChatSDK
{
    public class MChatGenerateQRCodeRequestBody
    {
        public double totalPrice;
        public ArrayList products = new ArrayList();
        public String title = "";
        public String subTitle = "";
        public String billId = "";
        public String noat = "";
        public String nhat = "";
        public String ttd = "";
    }

    class MChatGenerateQRCodeBodyPrivate
    {
        [JsonProperty("total_price")]
        public double totalPrice;
        [JsonProperty("products")]
        public ArrayList products = new ArrayList();

        [JsonProperty("title")]
        public String title = "";
        [JsonProperty("sub_title")]
        public String subTitle = "";
        [JsonProperty("noat")]
        public String noat = "";
        [JsonProperty("nhat")]
        public String nhat = "";
        [JsonProperty("ttd")]
        public String ttd = "";
        [JsonProperty("bill_id")]
        public String billID = "";

        public MChatGenerateQRCodeBodyPrivate(MChatGenerateQRCodeRequestBody body)
        {
            this.totalPrice = body.totalPrice;
            this.products = body.products;
            this.title = body.title;
            this.subTitle = body.subTitle;
            this.noat = body.noat;
            this.nhat = body.nhat;
            this.ttd = body.ttd;
        }

    }

    public class MChatProduct
    {
        [JsonProperty("product_name")]
        public String name;
        [JsonProperty("price")]
        public double unitPrice;
        [JsonProperty("quantity")]
        public int quantity;
        [JsonProperty("tag")]
        public String tag = "";
    }

    class MChatCheckQRCodePaymentRequestBody
    {
        [JsonProperty("qr")]
        public String qrCode = "";
    }

    public class MChatResponse
    {
        [JsonProperty("code")]
        public int code;
        [JsonProperty("message")]
        public String message;

        public override string ToString()
        {
            return "code: " + code + "\nmessage: " + message;
        }
    }

    public class MChatResponseGenerateQRCode : MChatResponse
    {
        [JsonProperty("qr")]
        public String generatedQRCode;
    }

    public class MChatResponseCheckState : MChatResponse
    {
        [JsonProperty("status")]
        public String status;
    }

    public class MChatScanPaymentBuilder
    {
        public String domain;
        public String apiKey;
        public String workerKey;
        public int timeout = 120000;

        public MChatScanPayment Build()
        {
            if (domain.Length == 0 || apiKey.Length == 0)
            {
                throw new System.ArgumentException("Parameter cannot be null , url, apiKey");
            }
            MChatScanPayment generator = new MChatScanPayment(this);
            return generator;
        }
    }

    public enum BNSState
    {
        PaymentSuccessful,
        Ready,
        Connected,
        Disconnected,
        ErrorOccured
    }

    public class MChatScanPayment
    {

        private static readonly HttpClient httpClient = new HttpClient();
        private MChatScanPaymentBuilder configBuilder;
        public event StateChanged stateChanged;

        private MChatBusinessNotificationService businessNotificationService;
        private MChatResponseGenerateQRCode mChatResponseGenerateQRCode;

        public delegate void StateChanged(MChatScanPayment scanPayment, BNSState state, String generatedQRCode, MChatResponse response);

        public MChatScanPayment(MChatScanPaymentBuilder configBuilder)
        {
            this.configBuilder = configBuilder;
            httpClient.DefaultRequestHeaders.Add("Authorization", "WorkerKey " + this.configBuilder.workerKey);
            httpClient.DefaultRequestHeaders.Add("Api-Key", this.configBuilder.apiKey);
            MChatBusinessNotificationServiceBuilder builder = new MChatBusinessNotificationServiceBuilder();
            builder.domain = "biznot.mongolchat.com";
            builder.port = 8790;
            builder.apiKey = this.configBuilder.apiKey;
            builder.timeout = this.configBuilder.timeout;
            this.businessNotificationService = builder.Build();
        }

        public async Task<MChatResponseGenerateQRCode> GenerateNewCodeAsync(MChatGenerateQRCodeRequestBody generateQRCodeBody, StateChanged bnsStateChanged)
        {
            this.stateChanged = bnsStateChanged;
            MChatGenerateQRCodeBodyPrivate privateBody = new MChatGenerateQRCodeBodyPrivate(generateQRCodeBody)
            {
            };
            String body = JsonConvert.SerializeObject(privateBody);
            var response = await httpClient.PostAsync("https://" + configBuilder.domain + "/v1/api/worker/onlineqr/generate", new StringContent(body, Encoding.UTF8, "application/json"));
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Response: " + responseBody);
                mChatResponseGenerateQRCode = JsonConvert.DeserializeObject<MChatResponseGenerateQRCode>(responseBody);
                ConnectToBusinessNotificationService();
                return mChatResponseGenerateQRCode;
            }
            else
            {
                mChatResponseGenerateQRCode = new MChatResponseGenerateQRCode();
                mChatResponseGenerateQRCode.code = (int)response.StatusCode;
                mChatResponseGenerateQRCode.message = response.ReasonPhrase;
                return mChatResponseGenerateQRCode;
            }
        }

        public void ConnectToBusinessNotificationService()
        {

            this.businessNotificationService.connectionState = (MChatBusinessNotificationService service, BNSProtocolConnectionState state) => {
                if (state == BNSProtocolConnectionState.Connected)
                {
                    this.stateChanged?.Invoke(this, BNSState.Connected, mChatResponseGenerateQRCode.generatedQRCode, null);

                }
                else if (state == BNSProtocolConnectionState.Disconnected)
                {
                    this.stateChanged?.Invoke(this, BNSState.Disconnected, mChatResponseGenerateQRCode.generatedQRCode, null);
                }
                else if (state == BNSProtocolConnectionState.Timeout)
                {
                    MChatResponse timeoutRes = new MChatResponse();
                    timeoutRes.code = 408;
                    timeoutRes.message = "Time Out";
                    this.stateChanged?.Invoke(this, BNSState.ErrorOccured, mChatResponseGenerateQRCode.generatedQRCode, timeoutRes);
                }
            };
            this.businessNotificationService.protocolResponse = (BNSProtocolState state, MChatResponse res) => {
                if (state == BNSProtocolState.Registration && res.code == 200)
                {
                    this.stateChanged?.Invoke(this, BNSState.Ready, mChatResponseGenerateQRCode.generatedQRCode, res);
                }
                else if (state == BNSProtocolState.PaymentNotification && res.code == 200)
                {
                    this.stateChanged?.Invoke(this, BNSState.PaymentSuccessful, mChatResponseGenerateQRCode.generatedQRCode, res);
                }
                else if (res.code != 200)
                {
                    this.stateChanged?.Invoke(this, BNSState.ErrorOccured, mChatResponseGenerateQRCode.generatedQRCode, res);
                }
            };
            Thread connectionThread = new Thread(() =>
            {
                this.businessNotificationService.connect(mChatResponseGenerateQRCode.generatedQRCode);
            });
            connectionThread.Start();
        }

        public async Task<MChatResponseCheckState> CheckQRCodePaymentState(String generatedQRCode)
        {
            MChatCheckQRCodePaymentRequestBody requestBody = new MChatCheckQRCodePaymentRequestBody
            {
                qrCode = generatedQRCode
            };
            String body = JsonConvert.SerializeObject(requestBody);
            var response = await httpClient.PostAsync("https://" + configBuilder.domain + "/v1/api/worker/onlineqr/status", new StringContent(body, Encoding.UTF8, "application/json"));
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseBody);
                MChatResponseCheckState mChatResponse = JsonConvert.DeserializeObject<MChatResponseCheckState>(responseBody);
                return mChatResponse;
            }
            else
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                MChatResponseCheckState mChatResponse = JsonConvert.DeserializeObject<MChatResponseCheckState>(responseBody);
                return mChatResponse;
            }
        }

        public void DisconnectFromBusinessNotificationService()
        {
            this.businessNotificationService.Disconnect();
        }
    }
}
