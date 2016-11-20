using System.Collections.Generic;

namespace Mollie.Api.Models
{
    /// <summary>
    /// Ideal issuer set
    /// </summary>
    public class Issuers
    {
        public int totalCount { get; set; }
        public int offset { get; set; }
        public int count { get; set; }
        public List<Issuer> data { get; set; }
    }
}