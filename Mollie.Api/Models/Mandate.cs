using System;

namespace Mollie.Api.Models
{
    public class Mandate
    {
        public string resource { get; set; }
        public string id { get; set; }
        public MandateStatus status { get; set; }
        public Method method { get; set; }
        public string customerId { get; set; }
        public PaymentDetails details { get; set; }
        public string mandateReference { get; set; }
        public DateTime? signatureDate { get; set; }
        public DateTime? createdDatetime { get; set; }

    }
}