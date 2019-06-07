using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading;

namespace MChatSDK
{
    public class MChatWorkerClient
    {
        private static readonly HttpClient httpClient = new HttpClient();
        private static readonly String domain = "developer.mongolchat.com";

        public delegate void StateChanged(MChatWorkerClient scanPayment, BNSState state, String generatedQRCode, MChatResponse response);

        public event StateChanged stateChanged;

        private MChatBusinessNotificationService businessNotificationService;

        public MChatWorkerClient ()
        {
            MChatWorkerConfiguration.Instance.CheckIsConfigured();
            if (MChatWorkerConfiguration.Instance.workerType == MChatWorkerConfiguration.MChatWorkerType.MChatWorkerBasic)
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + MChatWorkerConfiguration.Instance.authorization);
            } else if (MChatWorkerConfiguration.Instance.workerType == MChatWorkerConfiguration.MChatWorkerType.MChatWorkerKey)
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", "WorkerKey " + MChatWorkerConfiguration.Instance.authorization);
            }
            httpClient.DefaultRequestHeaders.Add("Api-Key", MChatWorkerConfiguration.Instance.apiKey);

            MChatBusinessNotificationServiceBuilder builder = new MChatBusinessNotificationServiceBuilder();
            builder.domain = "biznot.mongolchat.com";
            builder.port = 8790;
            builder.apiKey = MChatWorkerConfiguration.Instance.apiKey;
            builder.timeout = MChatWorkerConfiguration.Instance.bnsTimeout == 0 ? 120000 : MChatWorkerConfiguration.Instance.bnsTimeout;
            this.businessNotificationService = builder.Build();
        }

        public async Task<MChatResponseChargeByQRCode> ChargeByQRCode(MChatRequestChargeByQRCode chargeQRCodeBody)
        {
            var response = await httpClient.PostAsync("https://" + domain + "/v1/api/worker/chargeByQR", new StringContent(chargeQRCodeBody.json(), Encoding.UTF8, "application/json"));
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                MChatResponseChargeByQRCode mChatResponseChargeByQRCode = JsonConvert.DeserializeObject<MChatResponseChargeByQRCode>(responseBody);
                return mChatResponseChargeByQRCode;
            }
            else
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                MChatResponseChargeByQRCode mChatResponseChargeByQRCode = JsonConvert.DeserializeObject<MChatResponseChargeByQRCode>(responseBody);
                return mChatResponseChargeByQRCode;
            }
        }

        public async Task<MChatResponseGenerateQRCode> GeneratePaymentQRCode(MChatRequestGenerateQRCode generateQRCodeBody)
        {
            var response = await httpClient.PostAsync("https://" + domain + "/v1/api/worker/onlineqr/generate", new StringContent(generateQRCodeBody.json(), Encoding.UTF8, "application/json"));
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                MChatResponseGenerateQRCode mChatResponseGenerateQRCode = JsonConvert.DeserializeObject<MChatResponseGenerateQRCode>(responseBody);
                return mChatResponseGenerateQRCode;
            }
            else
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                MChatResponseGenerateQRCode mChatResponseGenerateQRCode = JsonConvert.DeserializeObject<MChatResponseGenerateQRCode>(responseBody);
                return mChatResponseGenerateQRCode;
            }
        }

        public async Task<MChatResponseGenerateQRCode> GeneratePaymentQRCode(MChatRequestGenerateQRCode generateQRCodeBody, StateChanged bnsStateChanged)
        {
            this.stateChanged = bnsStateChanged;
            var response = await httpClient.PostAsync("https://" + domain + "/v1/api/worker/onlineqr/generate", new StringContent(generateQRCodeBody.json(), Encoding.UTF8, "application/json"));
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                MChatResponseGenerateQRCode mChatResponseGenerateQRCode = JsonConvert.DeserializeObject<MChatResponseGenerateQRCode>(responseBody);
                ConnectToBusinessNotificationService(mChatResponseGenerateQRCode.generatedQRCode);
                return mChatResponseGenerateQRCode;
            }
            else
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                MChatResponseGenerateQRCode mChatResponseGenerateQRCode = JsonConvert.DeserializeObject<MChatResponseGenerateQRCode>(responseBody);
                return mChatResponseGenerateQRCode;
            }
        }

        public async Task<MChatResponseCheckStatus> CheckQRCodePaymentStatus(String generatedQRCode)
        {
            MChatRequestCheckQRCodePayment requestBody = new MChatRequestCheckQRCodePayment
            {
                qrCode = generatedQRCode
            };
            var response = await httpClient.PostAsync("https://" + domain + "/v1/api/worker/onlineqr/status", new StringContent(requestBody.json(), Encoding.UTF8, "application/json"));
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                MChatResponseCheckStatus mChatResponse = JsonConvert.DeserializeObject<MChatResponseCheckStatus>(responseBody);
                return mChatResponse;
            }
            else
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                MChatResponseCheckStatus mChatResponse = JsonConvert.DeserializeObject<MChatResponseCheckStatus>(responseBody);
                return mChatResponse;
            }
        }

        public async Task<MChatResponse> UpdateTransaction(MChatRequestUpdateTransaction requestBody)
        {
            var response = await httpClient.PostAsync("https://" + domain + "/v1/api/worker/update/transaction", new StringContent(requestBody.json(), Encoding.UTF8, "application/json"));
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                MChatResponse mChatResponse = JsonConvert.DeserializeObject<MChatResponse>(responseBody);
                return mChatResponse;
            }
            else
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                MChatResponse mChatResponse = JsonConvert.DeserializeObject<MChatResponse>(responseBody);
                return mChatResponse;
            }
        }

        public async Task<MChatResponseTransactionList> GetTransactionList(int page, int count)
        {
            return await this.GetTransactionList(page, count, null);
        }

        public async Task<MChatResponseTransactionList> GetTransactionList(int page, int count, String tag)
        {
            var response = await httpClient.GetAsync("https://" + domain + "/v1/api/worker/transaction/list?page=" + page + "&count=" + count + (tag != null ? ("&tag=" + tag) : ""));
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                MChatResponseTransactionList mChatResponse = JsonConvert.DeserializeObject<MChatResponseTransactionList>(responseBody);
                return mChatResponse;
            }
            else
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                MChatResponseTransactionList mChatResponse = JsonConvert.DeserializeObject<MChatResponseTransactionList>(responseBody);
                return mChatResponse;
            }
        }

        public async Task<MChatResponseTransactionDetail> GetTransactionDetail(String transactionID)
        { 
            var response = await httpClient.GetAsync("https://" + domain + "/v1/api/worker/transaction/detail/" + transactionID);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseBody);
                MChatResponseTransactionDetail mChatResponse = JsonConvert.DeserializeObject<MChatResponseTransactionDetail>(responseBody);
                return mChatResponse;
            }
            else
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                MChatResponseTransactionDetail mChatResponse = JsonConvert.DeserializeObject<MChatResponseTransactionDetail>(responseBody);
                return mChatResponse;
            }
        }

        public void ConnectToBusinessNotificationService(String generatedQRCode)
        {

            this.businessNotificationService.connectionState = (MChatBusinessNotificationService service, BNSProtocolConnectionState state) => {
                if (state == BNSProtocolConnectionState.Connected)
                {
                    this.stateChanged?.Invoke(this, BNSState.Connected, generatedQRCode, null);

                }
                else if (state == BNSProtocolConnectionState.Disconnected)
                {
                    this.stateChanged?.Invoke(this, BNSState.Disconnected, generatedQRCode, null);
                }
                else if (state == BNSProtocolConnectionState.Timeout)
                {
                    MChatResponse timeoutRes = new MChatResponse();
                    timeoutRes.code = 408;
                    timeoutRes.message = "Time Out";
                    this.stateChanged?.Invoke(this, BNSState.ErrorOccured, generatedQRCode, timeoutRes);
                }
            };
            this.businessNotificationService.protocolResponse = (BNSProtocolState state, MChatResponse res) => {
                if (state == BNSProtocolState.Registration && res.code == 200)
                {
                    this.stateChanged?.Invoke(this, BNSState.Ready, generatedQRCode, res);
                }
                else if (state == BNSProtocolState.PaymentNotification && res.code == 200)
                {
                    this.stateChanged?.Invoke(this, BNSState.PaymentSuccessful, generatedQRCode, res);
                }
                else if (res.code != 200)
                {
                    this.stateChanged?.Invoke(this, BNSState.ErrorOccured, generatedQRCode, res);
                }
            };
            Thread connectionThread = new Thread(() =>
            {
                this.businessNotificationService.connect(generatedQRCode);
            });
            connectionThread.Start();
        }

        public void DisconnectFromBusinessNotificationService()
        {
            this.businessNotificationService.Disconnect();
        }
    }
}
