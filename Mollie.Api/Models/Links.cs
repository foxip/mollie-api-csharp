namespace Mollie.Api.Models
{
    public class Links
    {
        public string paymentUrl { get; set; }
        public string redirectUrl { get; set; }
        public string webhookUrl  { get; set; }
        public string settlement  { get; set; }
        public string refunds { get; set; }
    }
}