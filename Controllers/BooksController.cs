using LibraryApi.Domain;
using LibraryApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApi.Controllers
{
    public class BooksController : Controller
    {
        LibraryDataContext Context;

        public BooksController(LibraryDataContext context)
        {
            Context = context;
        }

        //Get /books/{id}
        [HttpGet("/books/{id:int}")]
        public async Task<IActionResult> GetABook(int id)
        {
            //Single or default returns either one value, or default(null) or throws an exception if multiple objects are to be returned 
            var result = await Context.Books
                .Select(b => new GetBookResponseDocument
                {
                    Id = b.Id,
                    Title = b.Title,
                    Author = b.Author,
                    Genre = b.Genre
                }).SingleOrDefaultAsync(b => b.Id == id);

            if (result == null)
            {
                return NotFound("That book isn't in our library");
            }
            else
            {
                return Ok(result);
            }
        }

        [HttpPost("/books")]
        public async Task<IActionResult> AddABook()
        {
            // Validate the thingy
            // Not? -> Return a 400 status (Bad Request)
            // Is Valid?
            //  Add it to the database
            //  Return a 201 (created) status code.
            //  Add a location header to the response that has the URL for the new baby resource.
            //  And, if you are nice, send them a copy of the new resource as well.



            return BadRequest();
        }

        [HttpGet("/books")]
        public async Task<IActionResult> GetAllBooks([FromQuery] string genre = "all")
        {
            var response = new GetBooksResponseCollection();
            var allBooks = Context.Books.Select(b => new BookSummaryItem
            {
                Id = b.Id,
                Title = b.Title,
                Author = b.Author,
                Genre = b.Genre
            });
            if (genre != "all")
            {
                allBooks = allBooks.Where(b => b.Genre == genre);
            }
            response.Books = await allBooks.ToListAsync();
            response.GenreFilter = genre;

            return Ok(response);
        }
    }
}
