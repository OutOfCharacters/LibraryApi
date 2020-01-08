using LibraryApi.Domain;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace LibraryApi.Controllers
{
    public class BooksController : Controller
    {
        LibraryDataContext Context;

        public BooksController(LibraryDataContext context)
        {
            Context = context;
        }

        [HttpGet("/books")]
        public IActionResult GetAllBooks()
        {
            return Ok(Context.Books.ToList());
        }
    }
}
