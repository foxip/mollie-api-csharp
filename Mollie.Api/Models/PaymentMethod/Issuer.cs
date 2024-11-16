namespace Mollie.Api.Models.PaymentMethod
{
    /// <summary>
	/// iDeal issuer
	/// </summary>
	public class Issuer
    {
        public string id { get; set; }
        public string name { get; set; }
        public PaymentMethodImage image { get; set; }
    }
}
