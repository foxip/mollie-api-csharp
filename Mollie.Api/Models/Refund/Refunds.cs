using System.Collections.Generic;

namespace Mollie.Api.Models.Refund
{
    /// <summary>
    /// Refunds
    /// </summary>
    public class Refunds
    {
        public List<RefundStatus> refunds { get; set; } = new List<RefundStatus>();
    }
}