using System.Collections.Generic;

namespace Mollie.Api.Models
{
    /// <summary>
    /// Refunds
    /// </summary>
    public class Refunds : BaseList
    {
        public List<RefundStatus> data { get; set; }
    }
}