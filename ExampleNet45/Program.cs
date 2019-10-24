using System;
using System.Collections;
using System.Net.Http;
using System.Threading.Tasks;
using MChatSDK;

namespace ExampleNet45
{
    class Program
    {
        static void Main(string[] args)
        {
            //ConfigBuilder configBuilder = new ConfigBuilder();
            //configBuilder.url = "18.144.43.141";
            //configBuilder.port = 8790;
            //MChatBusinessNotificationService mchatservice =  configBuilder.build();
            //mchatservice.connect("Test");
            //var t = Task.Run(async () => {
            //    MChatScanPaymentBuilder paymentBuilder = new MChatScanPaymentBuilder();
            //    paymentBuilder.domain = "developer.mongolchat.com";
            //    paymentBuilder.apiKey = "";
            //    paymentBuilder.workerKey = "";
            //    MChatScanPayment payment = paymentBuilder.Build();
            //    MChatGenerateQRCodeRequestBody body = new MChatGenerateQRCodeRequestBody();
            //    body.totalPrice = 7;
            //    body.title = "Laviva";
            //    body.subTitle = "Welcome to Laviva";
            //    body.noat = "200";
            //    body.nhat = "0";
            //    body.ttd = "ttd";
            //    ArrayList products = new ArrayList();
            //    MChatProduct product = new MChatProduct();
            //    product.name = "Coca Cola";
            //    product.quantity = 1;
            //    product.unitPrice = 2000;
            //    products.Add(product);
            //    body.products = products;
            //MChatResponseGenerateQRCode response = await payment.GenerateNewCodeAsync(body, (MChatScanPayment scanPayment, BNSState state, String generatedQRCode, MChatResponse res) =>
            //{
            //    if (state == BNSState.Ready)
            //    {
            //            // Succesfully connected and ready to receive notification from notification service
            //            Console.WriteLine("Ready to display QRCode: " + generatedQRCode);
            //    }
            //    else if (state == BNSState.Connected)
            //    {
            //            // Successfully connected to notification service
            //            Console.WriteLine("Connected: " + res);
            //    }
            //    else if (state == BNSState.Disconnected)
            //    {
            //            // Disconnected from notification service
            //            Console.WriteLine("Disconnected: " + res);
            //    }
            //    else if (state == BNSState.PaymentSuccessful)
            //    {
            //            // Got response from payment notification service
            //            Console.WriteLine("PaymentSuccesfull: " + generatedQRCode + "\n" + res);
            //        var t2 = Task.Run(async () =>
            //        {
            //            MChatResponseCheckState responseStateSuccesfull = await payment.CheckQRCodePaymentState(generatedQRCode);
            //            Console.WriteLine(responseStateSuccesfull.ToString());
            //        });
            //    }
            //    else if (state == BNSState.ErrorOccured)
            //    {
            //            // Error Occured when connection notification service
            //            Console.WriteLine("ErrorOccured: " + res);
            //    }
            //    if (res != null)
            //    {
            //        Console.WriteLine(res.ToString());
            //    }
            //});
            //    Console.WriteLine(response.ToString());

            //    MChatResponseCheckState responseState = await payment.CheckQRCodePaymentState("pay://71698b23f949e980fdc842e84879a68a59e5970047f3feb9720eb81894a9646b");
            //    Console.WriteLine(responseState.ToString());
            //});
            //try
            //{
            //    t.Wait();
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e.ToString());
            //}
            ////Console.WriteLine("Start");
            //var t1 = Task.Run(async () =>
            //{
            //    await Task.Delay(1200000);
            //});
            //t1.Wait();
            //MChatWorkerConfiguration.Instance.Configure("1rWWASBspGSVdbGE+I9QHGt4Sse644Box5Haj7sBQJQ=", MChatWorkerConfiguration.MChatWorkerType.MChatWorkerKey, "8660bb3ef61fa597a1fe1b92ebe83315fc76dd46d52506f50b188ba5cbfd669f");
            MChatWorkerConfiguration.Instance.Configure("7QhJgkkjStjue8SIsWVqQD2EMgRm8CzsPifTVhm4Q0M=", MChatWorkerConfiguration.MChatWorkerType.MChatWorkerKey, "1bcc1e71123a255541923661aab62a7fa7a17b607045320ce05c04de400566f7");
            Console.WriteLine(MChatWorkerConfiguration.Instance.showInfo);
            MChatWorkerClient client = new MChatWorkerClient();
            var t1 = Task.Run(async () =>
            {
                MChatRequestReceipt receipt = new MChatRequestReceipt();
                receipt.totalPrice = 500;
                receipt.title = "Laviva";
                receipt.subTitle = "Welcome to Laviva";
                receipt.noat = "20";
                receipt.nhat = "0";
                receipt.ttd = "ttd";
                ArrayList products = new ArrayList();
                products.Add(new MChatProduct("Coca Cola", 100, 1));
                products.Add(new MChatProduct("Hiam", 1000, 1));
                receipt.products = products;
                //MChatResponseGenerateQRCode response = await client.GeneratePaymentQRCode(new MChatRequestGenerateQRCode(receipt, false, "MUNHUU", "windows12", new String[] { "IS", "munkhuu2" }), (MChatWorkerClient scanPayment, BNSState state, String generatedQRCode, String dynamicLink, MChatResponse res) =>
                //{
                //    if (state == BNSState.Ready)
                //    {
                //        // Succesfully connected and ready to receive notification from notification service
                //        Console.WriteLine("Ready to display QRCode: " + generatedQRCode);
                //    }
                //    else if (state == BNSState.Connected)
                //    {
                //        // Successfully connected to notification service
                //        Console.WriteLine("Connected: " + res);
                //    }
                //    else if (state == BNSState.Disconnected)
                //    {
                //        // Disconnected from notification service
                //        Console.WriteLine("Disconnected: " + res);
                //    }
                //    else if (state == BNSState.PaymentSuccessful)
                //    {
                //        // Got response from payment notification service
                //        Console.WriteLine("PaymentSuccesfull: " + generatedQRCode + "\n" + res);
                //        var t2 = Task.Run(async () =>
                //        {
                //            MChatResponseCheckStatus responseStatus = await client.CheckQRCodePaymentStatus(generatedQRCode);
                //            Console.WriteLine(responseStatus.ToString());
                //        });
                //    }
                //    else if (state == BNSState.ErrorOccured)
                //    {
                //        // Error Occured when connection notification service
                //        Console.WriteLine("ErrorOccured: " + res);
                //    }
                //    if (res != null)
                //    {
                //        Console.WriteLine(res.ToString());
                //    }
                //});
                //Console.WriteLine(response.ToString());
                //DEWA-211973
                //MChatResponseChargeByQRCode responseCharge = await client.ChargeByQRCode(new MChatRequestChargeByQRCode(receipt, "651096209132526398", "","window3", new String[] { "IS", "munkhuu2" }));
                //Console.WriteLine(responseCharge.ToString());
                //MChatResponseGenerateQRCode responseGenerateQRCode = await client.GeneratePaymentQRCode(new MChatRequestGenerateQRCode(receipt, false));
                //Console.WriteLine(responseGenerateQRCode.ToString());
                //MChatResponseCheckStatus responseStatus = await client.CheckQRCodePaymentStatus(responseGenerateQRCode.generatedQRCode);
                //Console.WriteLine(responseStatus.ToString());

                //MChatResponseTransactionList responseTransactionList = await client.GetTransactionList(0, 20);
                //Console.WriteLine(responseTransactionList.ToString());

                //MChatResponseTransactionDetail responseTransactionDetail = await client.GetTransactionDetail("IBNK-563101");
                //Console.WriteLine(responseTransactionDetail.ToString());

                //MChatResponse updateResponse = await client.UpdateTransaction(new MChatRequestUpdateTransaction("IBNK-563101", "ddtd", "billType", "lotteryId", "qrCode"));
                //Console.WriteLine(updateResponse.ToString());

                //MChatResponse refundResponse = await client.RefundTransaction("YAJS-619871");
                //Console.WriteLine(refundResponse.ToString());

                //MChatResponse checkTransactionByRefResponse = await client.CheckTransactionByRefNumber("windows4");
                //Console.WriteLine(checkTransactionByRefResponse.ToString());

                MChatResponseSettlement settlementResponse = await client.GetSettlement(new String[] { "IS", "munkhuu2" });
                Console.WriteLine(settlementResponse.ToString());
            });
            t1.Wait();
            Console.ReadLine();
        }
    }
}
