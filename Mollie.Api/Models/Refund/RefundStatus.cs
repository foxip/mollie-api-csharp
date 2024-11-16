using System;

namespace Mollie.Api.Models.Refund
{
    public class RefundStatus
    {
        public string id { get; set; }
        public string mode { get; set; }
        public string description { get; set; }
        public Amount amount { get; set; }
        public Amount settlementAmount { get; set; }
        public string status { get; set; }
        public string metadata { get; set; }
        public string paymentId { get; set; }
        public string settlementId { get; set; }
        public DateTime? createdAt { get; set; }

        public Links _links { get; set; } = new Links();

    }
}