using System;

namespace Mollie.Api.Models
{
    public class RefundStatus
    {
        public string id { get; set; }
        public PaymentStatus payment { get; set; }
        public decimal amount { get; set; }
        public DateTime? refundedDatetime { get; set; }
    }
}