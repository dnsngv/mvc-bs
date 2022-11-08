using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Mvc.Models
{
    public class CartViewModel
    {
        public CartEntryViewModel[] Entries { get; }
        [Display(Name = "Итого")]
        public decimal Total { get; }

        public CartViewModel(CartEntryViewModel[] entries)
        {
            Entries = entries;
            Total = Entries.Select(entry => entry.Total).Sum();
        }
    }
}
