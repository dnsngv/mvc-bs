using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Mvc.Data;
using Mvc.Models;

namespace Mvc.Controllers
{
    public class BookController : Controller
    {
        private const string CookieKeyCity = "City";
        private const string SessionKeyCart = "Cart";

        private readonly MvcBookContext _context;

        public BookController(MvcBookContext context)
        {
            _context = context;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            ViewData["SelectedCity"] = Request.Cookies[CookieKeyCity];
            ViewData["InCart"] = GetCartLength(GetSessionCart());
        }

        public async Task<IActionResult> Index(string sortBy = "Id", string sortDirection = "asc", string search = "")
        {
            return View(new BookListViewModel
            {
                Books = await BooksList(sortBy, sortDirection, search),
                SortBy = sortBy,
                SortDirection = sortDirection,
                Search = search,
            });
        }

        [EnableCors("Localhost")]
        [HttpGet]
        public async Task<IActionResult> IndexJson(string sortBy = "Id", string sortDirection = "asc", string search = "")
        {
            return Json(await BooksList(sortBy, sortDirection, search));
        }

        [EnableCors("Localhost")]
        [HttpGet]
        public IActionResult CartLengthJson()
        {
            var cartLength = GetCartLength(GetSessionCart());
            Console.WriteLine(cartLength);
            return Json(cartLength);
        }

        private Task<List<Book>> BooksList(string sortBy, string sortDirection, string search)
        {
            var filteredQuery = string.IsNullOrEmpty(search)
                ? _context.Book
                : _context.Book.Where(book => book.Name.Contains(search));

            var sortedQuery = sortBy switch
            {
                "Name" => sortDirection == "asc"
                    ? filteredQuery.OrderBy(book => book.Name)
                    : filteredQuery.OrderByDescending(book => book.Name),
                "Author" => sortDirection == "asc"
                    ? filteredQuery.OrderBy(book => book.Author)
                    : filteredQuery.OrderByDescending(book => book.Author),
                "Price" => sortDirection == "asc"
                    ? filteredQuery.OrderBy(book => book.Price)
                    : filteredQuery.OrderByDescending(book => book.Price),
                //"Type" => sortDirection == "asc"
                //    ? filteredQuery.OrderBy(book => book.Type)
                //    : filteredQuery.OrderByDescending(book => book.Type),
                //"Genre" => sortDirection == "asc"
                //    ? filteredQuery.OrderBy(book => book.Genre)
                //    : filteredQuery.OrderByDescending(book => book.Genre),
                _ => sortDirection == "asc"
                    ? filteredQuery.OrderBy(book => book.Id)
                    : filteredQuery.OrderByDescending(book => book.Id),
            };

            return sortedQuery.ToListAsync();
        }

        [HttpPost]
        public IActionResult SelectCity(int id)
        {
            Response.Cookies.Append(CookieKeyCity, id.ToString());
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Cart()
        {
            var cart = await GetCart(GetSessionCart());
            return View(cart);
        }

        [EnableCors("Localhost")]
        [HttpGet]
        public async Task<IActionResult> CartJson()
        {
            return Json(await GetCart(GetSessionCart()));
        }

        [EnableCors("Localhost")]
        [HttpPost]
        public async Task<IActionResult> AddToCart([FromBody] int bookId)
        {
            if (!BookExists(bookId)) return NotFound();

            var sessionCart = GetSessionCart();

            var selectedBookEntry = sessionCart.FirstOrDefault(entry => entry.BookId == bookId);
            if (selectedBookEntry is { } foundSelectedBookEntry)
            {
                foundSelectedBookEntry.Count++;
            }
            else
            {
                sessionCart.Add(new CartEntrySessionJsonModel(bookId));
            }

            var cart = await GetCart(sessionCart);

            SaveSessionCart(sessionCart);
            return Ok(new CartActionResponseModel(cart, bookId));
        }

        [HttpPost]
        public async Task<IActionResult> CartDecreaseEntry([FromBody] int bookId)
        {
            if (!BookExists(bookId)) return NotFound();

            var sessionCart = GetSessionCart();

            var selectedBookEntry = sessionCart.FirstOrDefault(entry => entry.BookId == bookId);
            if (selectedBookEntry is { Count: > 1 } foundSelectedBookEntry)
            {
                foundSelectedBookEntry.Count--;
            }

            var cart = await GetCart(sessionCart);

            SaveSessionCart(sessionCart);
            return Ok(new CartActionResponseModel(cart, bookId));
        }

        [HttpPost]
        public async Task<IActionResult> CartRemoveEntry([FromBody] int bookId)
        {
            if (!BookExists(bookId)) return NotFound();

            var sessionCart = GetSessionCart();

            var selectedBookEntry = sessionCart.FirstOrDefault(entry => entry.BookId == bookId);
            if (selectedBookEntry is { } foundSelectedBookEntry)
            {
                sessionCart.Remove(foundSelectedBookEntry);
            }

            var cart = await GetCart(sessionCart);

            SaveSessionCart(sessionCart);
            return Ok(new CartActionResponseModel(cart, bookId));
        }

        [EnableCors("Localhost")]
        [HttpPost]
        public async Task<IActionResult> CheckoutJson()
        {
            return Json(await CheckoutSave());
        }

        [HttpPost]
        public async Task<IActionResult> Checkout()
        {
            return await CheckoutSave()
                ? RedirectToAction(nameof(Index))
                : RedirectToAction(nameof(Cart));
        }

        private async Task<bool> CheckoutSave()
        {
            var sessionCart = GetSessionCart();
            var cityIdCookie = Request.Cookies[CookieKeyCity] ?? "14";
            if (sessionCart.Count == 0) return false;

            var cityId = int.Parse(cityIdCookie);

            var order = new Order
            {
                CityId = cityId,
                Entries = sessionCart.Select(cartEntry => new OrderEntry
                {
                    BookId = cartEntry.BookId,
                    Count = cartEntry.Count,
                }).ToList(),
            };
            _context.Orders.Add(order);

            await _context.SaveChangesAsync();

            SaveSessionCart(null);
            return true;
        }

        private List<CartEntrySessionJsonModel> GetSessionCart()
        {
            var cartJson = HttpContext.Session.GetString(SessionKeyCart);
            Console.WriteLine(cartJson);
            return cartJson is null
                ? new List<CartEntrySessionJsonModel>()
                : JsonSerializer.Deserialize<List<CartEntrySessionJsonModel>>(cartJson);
        }

        private int GetCartLength(IEnumerable<CartEntrySessionJsonModel> cart) => cart.Select(entry => entry.Count).Sum();

        private void SaveSessionCart(IEnumerable<CartEntrySessionJsonModel> cart)
        {
            var cartJson = cart is null
                ? "[]"
                : JsonSerializer.Serialize(cart);
            HttpContext.Session.SetString(SessionKeyCart, cartJson);
        }

        private async Task<CartViewModel> GetCart(List<CartEntrySessionJsonModel> sessionCart)
        {
            var cartViewList = (await _context.Book.ToListAsync())
                .Join(
                    sessionCart,
                    book => book.Id,
                    cartEntry => cartEntry.BookId,
                    (book, cartEntry) => new CartEntryViewModel(book, cartEntry)
                )
                .ToArray();

            var cart = new CartViewModel(cartViewList);
            return cart;
        }

        private bool BookExists(int id)
        {
            return _context.Book.Any(book => book.Id == id);
        }
    }
}
