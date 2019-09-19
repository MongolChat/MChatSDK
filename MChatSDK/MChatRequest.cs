using System;
using System.Collections;
using Newtonsoft.Json;

namespace MChatSDK
{
    public class MChatRequest
    {
        public String json()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class MChatProduct : MChatRequest
    {
        [JsonProperty("product_name")]
        public String name;
        [JsonProperty("price")]
        public double unitPrice;
        [JsonProperty("quantity")]
        public int quantity;
        [JsonProperty("tag")]
        public String tag = "";

        public MChatProduct()
        {
        }

        public MChatProduct(String name, double unitPrice, int quantity)
        {
            this.name = name;
            this.unitPrice = unitPrice;
            this.quantity = quantity;
        }

        public MChatProduct(String name, double unitPrice, int quantity, String tag)
        {
            this.name = name;
            this.unitPrice = unitPrice;
            this.quantity = quantity;
            this.tag = tag;
        }
    }


    public class MChatRequestReceipt
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

    class MChatCheckQRCodePaymentRequestBody : MChatRequest
    {
        [JsonProperty("qr")]
        public String qrCode = "";
    }

    class MChatRequestCheckQRCodePayment : MChatRequest
    {
        [JsonProperty("qr")]
        public String qrCode = "";
    }


    public class MChatRequestGenerateQRCode : MChatRequest
    {

        [JsonProperty("dynamic_link")]
        private Boolean withDynamicLink = false;
        [JsonProperty("tag")]
        private String tag = "";

        [JsonProperty("total_price")]
        private double totalPrice;
        [JsonProperty("products")]
        private ArrayList products = new ArrayList();
        [JsonProperty("title")]
        private String title = "";
        [JsonProperty("sub_title")]
        private String subTitle = "";
        [JsonProperty("noat")]
        private String noat = "";
        [JsonProperty("nhat")]
        private String nhat = "";
        [JsonProperty("ttd")]
        private String ttd = "";
        [JsonProperty("bill_id")]
        private String billID = "";

        public MChatRequestGenerateQRCode(MChatRequestReceipt receipt, Boolean withDynamicLink, String tag)
        {
            this.tag = tag;
            this.withDynamicLink = withDynamicLink;

            this.totalPrice = receipt.totalPrice;
            this.products = receipt.products;
            this.title = receipt.title;
            this.subTitle = receipt.subTitle;
            this.noat = receipt.noat;
            this.nhat = receipt.nhat;
            this.ttd = receipt.ttd;
            this.billID = receipt.billId;
        }

        public MChatRequestGenerateQRCode(MChatRequestReceipt receipt, Boolean withDynamicLink) : this(receipt, withDynamicLink, "")
        {

        }

        public MChatRequestGenerateQRCode(MChatRequestReceipt receipt) : this(receipt, false)
        {

        }

    }

    public class MChatRequestChargeByQRCode : MChatRequest
    {

        [JsonProperty("token")]
        public String token = "";
        [JsonProperty("tag")]
        public String tag = "";

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

        public MChatRequestChargeByQRCode(MChatRequestReceipt receipt, String token, String tag)
        {
            this.token = token;
            this.tag = tag;

            this.totalPrice = receipt.totalPrice;
            this.products = receipt.products;
            this.title = receipt.title;
            this.subTitle = receipt.subTitle;
            this.noat = receipt.noat;
            this.nhat = receipt.nhat;
            this.ttd = receipt.ttd;
        }

        public MChatRequestChargeByQRCode(MChatRequestReceipt receipt, String token) : this(receipt, token, "")
        {

        }
    }

    public class MChatRequestUpdateTransaction : MChatRequest
    {
        [JsonProperty("id")]
        public String transactionID;
        [JsonProperty("ddtd")]
        public String ddtd;
        [JsonProperty("bill_type")]
        public String billType;
        [JsonProperty("lottery_id")]
        public String lotteryID;
        [JsonProperty("qr_code")]
        public String qrCode;

        public MChatRequestUpdateTransaction(String transactionID, String ddtd, String billType, String lotteryId, String qrCode)
        {
            this.transactionID = transactionID;
            this.ddtd = ddtd;
            this.billType = billType;
            this.lotteryID = lotteryId;
            this.qrCode = qrCode;
        }
    }

    public class MChatRequestRefundTransaction : MChatRequest
    {
        [JsonProperty("id")]
        public String transactionID;

        public MChatRequestRefundTransaction(String transactionID)
        {
            this.transactionID = transactionID;
        }
    }
}
