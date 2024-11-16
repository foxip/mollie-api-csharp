using System.Collections.Generic;

namespace Mollie.Api.Models.Payment
{
    public class PaymentStatuses
    {
        public List<PaymentStatus> payments { get; set; } = new List<PaymentStatus>();
    }
}