using System;

namespace Mollie.Api.Models
{
    public class PaymentDetails
    {
        public string consumerName { get; set; }
        public string consumerAccount { get; set; }
        public string consumerBic { get; set; }

        public string cardHolder { get; set; }
        public string cardNumber { get; set; }
        public string cardSecurity { get; set; }
        public string cardFingerprint { get; set; }
        public string cardAudience { get; set; }
        public string cardLabel { get; set; }
        public string cardCountry  { get; set; }
        public string cardCountryCode  { get; set; }
        public DateTime? cardExpiryDate { get; set; }
        public string feeRegion { get; set; }

        public string bankName { get; set; }
        public string bankAccount { get; set; }
        public string bankBic { get; set; }
        public string bankReason { get; set; }
        public string bankReasonCode { get; set; }
        public string transferReference { get; set; }

        public string bitcoinAddress { get; set; }
        public string bitcoinAmount { get; set; }
        public string bitcoinRate { get; set; }
        public string bitcoinUri { get; set; }

        public string paypalReference { get; set; }

        public string customerReference { get; set; }
        public string creditorIdentifier { get; set; }
        public string signatureDate { get; set; }

        public string endToEndIdentifier { get; set; }
        public string mandateReference  { get; set; }
        public string batchReference  { get; set; }
        public string fileReference  { get; set; }

    }
}