using System;
using System.Collections;
using System.Collections.Generic;
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
        public double amount;
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
        [JsonProperty("dynamic_link_callback")]
        private String withDynamicLinkCallback = "";
        [JsonProperty("tag")]
        private String tag = "";

        [JsonProperty("branch_id")]
        public String branch_id = "";

        [JsonProperty("amount")]
        private double amount;
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
        [JsonProperty("reference_number")]
        private String refNumber = "";

        public MChatRequestGenerateQRCode(MChatRequestReceipt receipt, Boolean withDynamicLink, String withDynamicLinkCallback, String tag, String branchId, String refNumber)
        {
            this.tag = tag == null ? "" : tag;
            this.withDynamicLink = withDynamicLink;
            this.withDynamicLinkCallback = withDynamicLinkCallback;
            this.branch_id = branchId == null ? "" : branchId;

            this.amount = receipt.amount;
            this.products = receipt.products;
            this.title = receipt.title;
            this.subTitle = receipt.subTitle;
            this.noat = receipt.noat;
            this.nhat = receipt.nhat;
            this.ttd = receipt.ttd;
            this.billID = receipt.billId;
            this.refNumber = refNumber == null ? "" : refNumber;
        }

        public MChatRequestGenerateQRCode(MChatRequestReceipt receipt, String refNumber) : this(receipt, false, "",null, null, refNumber)
        {

        }

        public MChatRequestGenerateQRCode(MChatRequestReceipt receipt, String branchId, String refNumber) : this(receipt, false, "", null, branchId, refNumber)
        {

        }

        public MChatRequestGenerateQRCode(MChatRequestReceipt receipt, Boolean withDynamicLink, String withDynamicLinkCallback) : this(receipt, withDynamicLink, withDynamicLinkCallback, null, null, null)
        {

        }

        public MChatRequestGenerateQRCode(MChatRequestReceipt receipt) : this(receipt, false, "")
        {

        }

    }

    public class MChatRequestChargeByQRCode : MChatRequest
    {

        [JsonProperty("token")]
        public String token = "";
        [JsonProperty("tag")]
        public String tag = "";
        [JsonProperty("branch_id")]
        public String branch_id = "";

        [JsonProperty("amount")]
        public double amount;
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
        [JsonProperty("reference_number")]
        private String refNumber = "";

        public MChatRequestChargeByQRCode(MChatRequestReceipt receipt, String token, String tag, String branchId, String refNumber) 
        {
            this.token = token;
            this.tag = tag == null ? "" : tag;
            this.branch_id = branchId == null ? "" : branchId;

            this.amount = receipt.amount;
            this.products = receipt.products;
            this.title = receipt.title;
            this.subTitle = receipt.subTitle;
            this.noat = receipt.noat;
            this.nhat = receipt.nhat;
            this.ttd = receipt.ttd;
            this.refNumber = refNumber == null ? "" : refNumber;
        }
        public MChatRequestChargeByQRCode(MChatRequestReceipt receipt, String token, String refNumber) : this(receipt, token, null, null, refNumber)
        {

        }
        public MChatRequestChargeByQRCode(MChatRequestReceipt receipt, String token, String branchId, String refNumber) : this(receipt, token, null, branchId, refNumber)
        {

        }

        public MChatRequestChargeByQRCode(MChatRequestReceipt receipt, String token) : this(receipt, token, null, null, null)
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

    public class MChatRequestSettlementTransaction : MChatRequest
    {
        [JsonProperty("reference_number")]
        private String reference_number = "";
        [JsonProperty("amount")]
        private double amount;

        public MChatRequestSettlementTransaction(String reference_number, double amount)
        {
            this.reference_number = reference_number;
            this.amount = amount;
        }
    }

    public class MChatRequestSettleUpload : MChatRequest
    {
        [JsonProperty("transactions")]
        private ArrayList transactions = new ArrayList();
        public MChatRequestSettleUpload(ArrayList transactions)
        {
            this.transactions = transactions;
        }
    }
}
