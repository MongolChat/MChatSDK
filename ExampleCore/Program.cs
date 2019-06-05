using System;
using System.Collections;
using System.Net.Http;
using System.Threading.Tasks;
using MChatSDK;

namespace ExampleCore
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
            var t = Task.Run(async () => {
                MChatScanPaymentBuilder paymentBuilder = new MChatScanPaymentBuilder();
                paymentBuilder.domain = "developer.mongolchat.com";
                paymentBuilder.apiKey = "";
                paymentBuilder.workerKey = "";
                MChatScanPayment payment = paymentBuilder.Build();
                MChatGenerateQRCodeRequestBody body = new MChatGenerateQRCodeRequestBody();
                body.totalPrice = 7;
                body.title = "Laviva";
                body.subTitle = "Welcome to Laviva";
                body.noat = "200";
                body.nhat = "0";
                body.ttd = "ttd";
                ArrayList products = new ArrayList();
                MChatProduct product = new MChatProduct();
                product.name = "Coca Cola";
                product.quantity = 1;
                product.unitPrice = 2000;
                products.Add(product);
                body.products = products;
                MChatResponseGenerateQRCode response = await payment.GenerateNewCodeAsync(body, (MChatScanPayment scanPayment, BNSState state, String generatedQRCode, MChatResponse res) =>
                {
                    if (state == BNSState.Ready)
                    {
                        // Succesfully connected and ready to receive notification from notification service
                        Console.WriteLine("Ready to display QRCode: " + generatedQRCode);
                    }
                    else if (state == BNSState.Connected)
                    {
                        // Successfully connected to notification service
                        Console.WriteLine("Connected: " + res);
                    }
                    else if (state == BNSState.Disconnected)
                    {
                        // Disconnected from notification service
                        Console.WriteLine("Disconnected: " + res);
                    }
                    else if (state == BNSState.PaymentSuccessful)
                    {
                        // Got response from payment notification service
                        Console.WriteLine("PaymentSuccesfull: " + generatedQRCode + "\n" + res);
                        var t2 = Task.Run(async () =>
                        {
                            MChatResponseCheckState responseStateSuccesfull = await payment.CheckQRCodePaymentState(generatedQRCode);
                            Console.WriteLine(responseStateSuccesfull.ToString());
                        });
                    }
                    else if (state == BNSState.ErrorOccured)
                    {
                        // Error Occured when connection notification service
                        Console.WriteLine("ErrorOccured: " + res);
                    }
                    if (res != null)
                    {
                        Console.WriteLine(res.ToString());
                    }
                });
                Console.WriteLine(response.ToString());

                MChatResponseCheckState responseState = await payment.CheckQRCodePaymentState("pay://71698b23f949e980fdc842e84879a68a59e5970047f3feb9720eb81894a9646b");
                Console.WriteLine(responseState.ToString());
            });
            try
            {
                t.Wait();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            //Console.WriteLine("Start");
            var t1 = Task.Run(async () =>
            {
                await Task.Delay(1200000);
            });
            t1.Wait();
        }
    }
}
