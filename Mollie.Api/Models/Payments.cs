using System.Collections.Generic;

namespace Mollie.Api.Models
{
    public class Payments : BaseList
    {
        public List<PaymentStatus> data { get; set; }
    }
}