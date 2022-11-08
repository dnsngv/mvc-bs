namespace Mvc.Models
{
    public class OrderEntry
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public int BookId { get; set; }
        public Book Book { get; set; }
        public int Count { get; set; }
    }
}
