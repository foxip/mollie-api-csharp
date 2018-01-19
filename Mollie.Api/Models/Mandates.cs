using System.Collections.Generic;

namespace Mollie.Api.Models
{
    /// <summary>
    /// Refunds set
    /// </summary>
    public class Mandates : BaseList
    {
        public List<Mandate> data { get; set; }
    }
}