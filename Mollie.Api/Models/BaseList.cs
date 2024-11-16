namespace Mollie.Api.Models
{
    public abstract class BaseList
    {
        public int count { get; set; }
        public Links _links { get; set; } = new Links();
    }
}