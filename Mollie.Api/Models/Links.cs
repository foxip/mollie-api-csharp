namespace Mollie.Api.Models
{
    public class Links
    {
        public Link self { get; set; }
        public Link documentation { get; set; }
        public Link previous { get; set; }
        public Link next { get; set; }
        public Link checkout { get; set; }
        public Link redirectUrl { get; set; }
        public Link webhookUrl  { get; set; }
        public Link settlement  { get; set; }
        public Link refunds { get; set; }
        public Link chargebacks { get; set; }
        public Link status { get; set; }
        public Link payOnline { get; set; }
        public Link payments { get; set; }
    }
}