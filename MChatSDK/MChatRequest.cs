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

        [JsonProperty("branch_id")]
        public String branch_id = "";

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
        [JsonProperty("reference_number")]
        private String refNumber = "";
        [JsonProperty("settlement_ids")]
        private String [] settlementIds = new String[]{};

        public MChatRequestGenerateQRCode(MChatRequestReceipt receipt, Boolean withDynamicLink, String tag, String branchId, String refNumber, String [] settlementIds)
        {
            this.tag = tag == null ? "" : tag;
            this.withDynamicLink = withDynamicLink;
            this.branch_id = branchId == null ? "" : branchId;

            this.totalPrice = receipt.totalPrice;
            this.products = receipt.products;
            this.title = receipt.title;
            this.subTitle = receipt.subTitle;
            this.noat = receipt.noat;
            this.nhat = receipt.nhat;
            this.ttd = receipt.ttd;
            this.billID = receipt.billId;
            this.refNumber = refNumber == null ? "" : refNumber;
            this.settlementIds = settlementIds == null ? new String[] { } : settlementIds;
        }

        public MChatRequestGenerateQRCode(MChatRequestReceipt receipt, Boolean withDynamicLink, String refNumber, String[] settlementIds) : this(receipt, withDynamicLink, null, null, refNumber, settlementIds)
        {

        }

        public MChatRequestGenerateQRCode(MChatRequestReceipt receipt, Boolean withDynamicLink, String branchId, String refNumber, String[] settlementIds) : this(receipt, withDynamicLink, null, branchId, refNumber, settlementIds)
        {

        }

        public MChatRequestGenerateQRCode(MChatRequestReceipt receipt, Boolean withDynamicLink, String refNumber) : this(receipt, withDynamicLink, null, null, refNumber, new String[] {})
        {

        }

        public MChatRequestGenerateQRCode(MChatRequestReceipt receipt, Boolean withDynamicLink) : this(receipt, withDynamicLink, null, null, null, null)
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
        [JsonProperty("branch_id")]
        public String branch_id = "";

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
        [JsonProperty("reference_number")]
        private String refNumber = "";
        [JsonProperty("settlement_ids")]
        private String[] settlementIds = new String[] { };

        public MChatRequestChargeByQRCode(MChatRequestReceipt receipt, String token, String tag, String branchId, String refNumber, String[] settlementIds)
        {
            this.token = token;
            this.tag = tag == null ? "" : tag;
            this.branch_id = branchId == null ? "" : branchId;

            this.totalPrice = receipt.totalPrice;
            this.products = receipt.products;
            this.title = receipt.title;
            this.subTitle = receipt.subTitle;
            this.noat = receipt.noat;
            this.nhat = receipt.nhat;
            this.ttd = receipt.ttd;
            this.refNumber = refNumber == null ? "" : refNumber;
            this.settlementIds = settlementIds == null ? new String[] { } : settlementIds; 
        }
        public MChatRequestChargeByQRCode(MChatRequestReceipt receipt, String token, String refNumber, String[] settlementId) : this(receipt, token, null, null, refNumber, settlementId)
        {

        }
        public MChatRequestChargeByQRCode(MChatRequestReceipt receipt, String token, String branchId, String refNumber, String[] settlementId) : this(receipt, token, null, branchId, refNumber, settlementId)
        {

        }

        public MChatRequestChargeByQRCode(MChatRequestReceipt receipt, String token) : this(receipt, token, null, null, null, null)
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

    public class MChatRequestSettlement : MChatRequest
    {

        [JsonProperty("settlement_ids")]
        private String[] settlementIds = new String[] { };
        [JsonProperty("start_date")]
        private DateTime startDate = DateTime.Today;
        [JsonProperty("end_date")]
        private DateTime endDate = DateTime.Now;
        public MChatRequestSettlement(String[] settlementIds, DateTime startDate, DateTime endDate)
        {
            this.settlementIds = settlementIds;
            this.startDate = startDate;
            this.endDate = endDate;
        }

        public MChatRequestSettlement(String [] settlementIds) : this(settlementIds, DateTime.Today, DateTime.Now)
        {
            this.settlementIds = settlementIds;
        }
    }
}
