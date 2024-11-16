using System.Collections.Generic;

namespace Mollie.Api.Models.Customer
{
    /// <summary>
    /// Customers
    /// </summary>
    public class Customers
    {
        public List<GetCustomer> customers { get; set; } = new List<GetCustomer>();
    }
}