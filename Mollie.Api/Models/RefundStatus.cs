using System;

namespace Mollie.Api.Models
{
    public class RefundStatus
    {
        public string id { get; set; }
        public PaymentStatus payment { get; set; }
        public decimal amountRefunded { get; set; }
        public decimal amountRemaining { get; set; }
        public DateTime? refundedDatetime { get; set; }
    }
}