# MChatSDK
Монгол чатын хөгжүүлэгчдэд зориулсан SDK. 

### Dependency

`.NETFramework4.5+`

`.NETStandart2.0+`

`Newtonsoft.Json`

### Яаж суулгах вэ?

Nuget дээр манай sdk-ийн хувилбарууд тавигдаж байгаа бөгөөд та шууд `visual studio` дээрээсээ суулгах боломжтой.

Доорх холбоосоор орж сонирхоно уу

https://www.nuget.org/packages/mchat.sdk/

### Яаж хэрэглэх вэ?

Хамгийн түрүүнд тохиргоог хийх хэрэгтэй. Тохиргоо хийгээгүй үед Exception заахыг анхаарна уу!

```c#
MChatWorkerConfiguration.Instance.Configure("API_KEY", MChatWorkerConfiguration.MChatWorkerType.MChatWorkerKey, "WORKER_KEY");
```

Утсаар төлөх дээрх кодыг уншуулан төлбөр авах

```c#
MChatRequestReceipt receipt = new MChatRequestReceipt();
receipt.totalPrice = 2;
receipt.title = "receipt title";
receipt.subTitle = "receipt subtitle";
receipt.noat = "20";
receipt.nhat = "0";
receipt.ttd = "ttd";
ArrayList products = new ArrayList();
products.Add(new MChatProduct("Coca Cola", 100, 1));
products.Add(new MChatProduct("Hiam", 1000, 1));
receipt.products = products;
MChatResponseChargeByQRCode responseCharge = await client.ChargeByQRCode(new MChatRequestChargeByQRCode(receipt, "token", "ref_number"));
```

Гүйлгээнд НӨАТ-ын мэдээлэлийг хавсаргах

```c#

MChatResponse updateResponse = await client.UpdateTransaction(new MChatRequestUpdateTransaction("IBNK-563101", "ddtd", "billType", "lotteryId", "qrCode"));

```

Хэрэглэгч уншуулж болох QR код гаргах

```c#
MChatRequestReceipt receipt = new MChatRequestReceipt();
receipt.totalPrice = 2;
receipt.title = "receipt title";
receipt.subTitle = "receipt subtitle";
receipt.noat = "20";
receipt.nhat = "0";
receipt.ttd = "ttd";
ArrayList products = new ArrayList();
products.Add(new MChatProduct("Coca Cola", 100, 1));
products.Add(new MChatProduct("Hiam", 1000, 1));
receipt.products = products;
MChatResponseGenerateQRCode responseGenerateQRCode = await client.GeneratePaymentQRCode(new MChatRequestGenerateQRCode(receipt, false, "dynamic_link_callback", "tag", "branch_id" "ref_number")); // dynamicLink үүсгэхийг хүсвэл true утга явуулна
```

Хэрэглэгч уншуулж болох QR код гарган `notification service` - тэй холбогдон төлбөр төлөгдсөн эсхийг мэдэх.

```c#
MChatRequestReceipt receipt = new MChatRequestReceipt();
receipt.totalPrice = 2;
receipt.title = "Laviva";
receipt.subTitle = "Welcome to Laviva";
receipt.noat = "20";
receipt.nhat = "0";
receipt.ttd = "ttd";
ArrayList products = new ArrayList();
products.Add(new MChatProduct("Coca Cola", 100, 1));
products.Add(new MChatProduct("Hiam", 1000, 1));
receipt.products = products;
MChatResponseGenerateQRCode response = await client.GeneratePaymentQRCode(new MChatRequestGenerateQRCode(receipt, false, "dynamic_link_callback", "tag", "branch_id" "ref_number")), (MChatWorkerClient scanPayment, BNSState state, String generatedQRCode, String dynamicLink, MChatResponse res) =>
{
    if (state == BNSState.Ready)
    {
        // Succesfully connected and ready to receive notification from notification service
    }
    else if (state == BNSState.Connected)
    {
        // Successfully connected to notification service
    }
    else if (state == BNSState.Disconnected)
    {
        // Disconnected from notification service
    }
    else if (state == BNSState.PaymentSuccessful)
    {
        // Got response from payment notification service
        // Notification service - ээс хариу ирсэний дараа заавал статусийг нь шалгах хэрэгтэй.
    }
    else if (state == BNSState.ErrorOccured)
    {
        // Error Occured when connection notification service
        // Timeout эсвэл ямар наг алдаа гарсаны дараа статус шалгах хэрэгтэй
    }
});
```

Гүйлгээг reference_number - аар шалгах

```c#
MChatResponse checkTransactionByRefResponse = await client.CheckTransactionByRefNumber("ref_number");
```

Хэрэглэгчид харуулсан QR кодны статусийг шалгах

```c#
MChatResponseCheckStatus responseStatus = await client.CheckQRCodePaymentStatus(generatedQRCode);
```

Тухайн Worker - ийн хийсэн гүйлгээний жагсаалтыг авах.

```c#
MChatResponseTransactionList responseTransactionList = await client.GetTransactionList(0, 20);
```

Гүйлгээний дэлгэрэнгүй мэдээлэл авах.

```c#
MChatResponseTransactionDetail responseTransactionDetail = await client.GetTransactionDetail("IBNK-563101");
```

Settlement upload хийх

```c#
//Max Count 10
ArrayList settlements = new ArrayList();
settlements.Add(new MChatRequestSettlementTransaction("reference_number", 1000));
MChatResponseSettlementUpload settlementResponse = await client.SettleUpload(new MChatRequestSettleUpload(settlements));
```
