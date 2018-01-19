using System.Collections.Generic;

namespace Mollie.Api.Models
{
    /// <summary>
    /// Customers
    /// </summary>
    public class Customers : BaseList
    {
        public List<GetCustomer> data { get; set; }
    }
}