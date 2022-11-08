using System.Linq;

namespace Mvc.Models
{
    public class CartActionResponseModel
    {
        public CartEntryViewModel BookEntry { get; }
        public int Length { get; }
        public decimal Total { get; }

        public CartActionResponseModel(CartViewModel cart, int BookId)
        {
            BookEntry = cart.Entries.FirstOrDefault(entry => entry.Book.Id == BookId);
            Length = cart.Entries.Select(entry => entry.Count).Sum();
            Total = cart.Total;
        }
    }
}
