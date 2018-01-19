namespace Mollie.Api.Models
{
    public abstract class BaseList
    {
        public int totalCount { get; set; }
        public int offset { get; set; }
        public int count { get; set; }
        public PagingLinks links { get; set; }
    }
}