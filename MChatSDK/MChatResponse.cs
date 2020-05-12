using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace MChatSDK
{
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

    public class MChatResponseChargeByQRCode : MChatResponse
    {
        [JsonProperty("id")]
        public String transactionID;
        [JsonProperty("who_paid")]
        public String whoPaid;
        [JsonProperty("user_ref_id")]
        public String userRefID;

        public override string ToString()
        {
            return base.ToString() + "\ntransaction ID: " + transactionID + "\nwho paid: " + whoPaid + "\nuser ref ID: " + userRefID;
        }
    }

    public class MChatResponseGenerateQRCode : MChatResponse
    {
        [JsonProperty("dynamic_link")]
        public String dynamicLink;
        [JsonProperty("qr")]
        public String generatedQRCode;

        public override string ToString()
        {
            return base.ToString() + "\nqr: " + generatedQRCode + "\ndynamicLink: " + dynamicLink;
        }
    }

    public class MChatResponseCheckState : MChatResponse
    {
        [JsonProperty("status")]
        public String status;

        public override string ToString()
        {
            return base.ToString() + "\nstatus: " + status;
        }
    }

    public class MChatResponseCheckTransactionByRefNumber : MChatResponse
    {
        [JsonProperty("id")]
        public String transactionID;

        public override string ToString()
        {
            return base.ToString() + "\ntransactionID: " + transactionID;
        }
    }


    public class MChatResponseCheckStatus : MChatResponse
    {
        [JsonProperty("status")]
        public String status;
        [JsonProperty("id")]
        public String transactionID;

        public override string ToString()
        {
            return base.ToString() + "\nstatus: " + status + "\ntransactionID: " + transactionID;
        }
    }

    public class MChatResponseTransaction
    {
        [JsonProperty("id")]
        public String transactionID = "";
        [JsonProperty("amount")]
        public double amount = 0;
        [JsonProperty("date")]
        private String date = "";

        public DateTime transactionDate
        {
            get
            {
                return DateTime.Parse(this.date, null, System.Globalization.DateTimeStyles.RoundtripKind);
            }
        }
        public override string ToString()
        {
            return base.ToString() + "\ntransactionID: " + transactionID + "\namount: " + amount + "\ndate: " + date;
        }
    }

    public class MChatResponseTransactionList : MChatResponse
    {
        [JsonProperty("transactions")]
        public List<MChatResponseTransaction> transactions;
        public override string ToString()
        {
            return base.ToString() + "\ntransactions: " + transactions.Count;
        }
    }

    class MChatResponseTransactionDetailBill
    {
        [JsonProperty("total_price")]
        public double totalPrice = 0;
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
        [JsonProperty("ddtd")]
        public String ddtd = "";
        [JsonProperty("bill_type")]
        public String billType = "";
        [JsonProperty("lottery_id")]
        public String lotteryID = "";
        [JsonProperty("qr_code")]
        public String qrCode = "";
    }

    public class MChatResponseReceipt
    {
        public double totalPrice;
        public String title = "";
        public String subTitle = "";
        public String noat = "";
        public String nhat = "";
        public String ttd = "";
        public String billID = "";
        public String ddtd = "";
        public String billType = "";
        public String lotteryID = "";
        public String qrCode = "";
        public List<MChatProduct> products;
    }

    public class MChatResponseTransactionDetail : MChatResponse
    {
        [JsonProperty("id")]
        public String transactionID = "";

        [JsonProperty("date")]
        private String date = "";

        [JsonProperty("bill")]
        private MChatResponseTransactionDetailBill bill = null;

        public DateTime transactionDate
        {
            get
            {
                return DateTime.Parse(this.date, null, System.Globalization.DateTimeStyles.RoundtripKind);
            }
        }

        [JsonProperty("products")]
        private List<MChatProduct> products = null;

        public MChatResponseReceipt receipt {
            get
            {
                MChatResponseReceipt receipt = new MChatResponseReceipt()
                {
                    totalPrice = this.bill.totalPrice,
                    title = this.bill.title,
                    subTitle = this.bill.subTitle,
                    noat = this.bill.noat,
                    nhat = this.bill.nhat,
                    ttd = this.bill.ttd,
                    billID = this.bill.billID,
                    ddtd = this.bill.ddtd,
                    billType = this.bill.billType,
                    lotteryID = this.bill.lotteryID,
                    qrCode = this.bill.qrCode,
                    products = this.products
                };
                return receipt;
            }
        }

        public override string ToString()
        {
            return base.ToString() + "\ntransactionID: " + transactionID + "\nproducts: " + products.Count;
        }
    }

    public class MChatResponseSettlementUpload : MChatResponse
    {
        [JsonProperty("failed")]
        public String[] failed;
        [JsonProperty("success")]
        public String[] success;
        public override string ToString()
        {
            return base.ToString() + "\nfailed: " + failed.Length + "\nsuccess:" + success.Length;
        }
    }
}
