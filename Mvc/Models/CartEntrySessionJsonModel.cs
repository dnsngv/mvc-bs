namespace Mvc.Models
{
    public class CartEntrySessionJsonModel
    {
        public int BookId { get; set; }
        public int Count { get; set; } = 1;

        public CartEntrySessionJsonModel(int bookId)
        {
            BookId = bookId;
        }
    }
}
