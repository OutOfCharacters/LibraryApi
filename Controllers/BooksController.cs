using LibraryApi.Domain;
using LibraryApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApi.Controllers
{
    [Produces("application/json")]
    public class BooksController : Controller
    {
        LibraryDataContext Context;

        public BooksController(LibraryDataContext context)
        {
            Context = context;
        }

        //Get /books/{id}
        [HttpGet("/books/{id:int}", Name = "books#getabook")]
        public async Task<IActionResult> GetABook(int id)
        {
            //Single or default returns either one value, or default(null) or throws an exception if multiple objects are to be returned 
            var result = await Context.Books
                .Where(b=>b.InInventory == true)
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
        public async Task<IActionResult> AddABook([FromBody] PostBookRequest bookToAdd)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var book = new Book
            {
                Title = bookToAdd.Title,
                Author = bookToAdd.Author,
                Genre = bookToAdd.Genre ?? "Unknown"
            };
            Context.Books.Add(book);
            await Context.SaveChangesAsync();

            var bookToReturn = new GetBookResponseDocument
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                Genre = book.Genre
            };
            return CreatedAtRoute("books#getabook", new { id = book.Id }, bookToReturn);
        }

        //Delete almost never gets used, usually just enables/disables certain rows with property
        /// <summary>
        /// Removes a book from the inventory
        /// </summary>
        /// <param name="id">Id of the book you want to remove</param>
        /// <returns></returns>
        [HttpDelete("/books/{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> RemoveABook(int id)
        {
            var book = await Context.Books.Where(b => b.Id == id && b.InInventory).SingleOrDefaultAsync();
            if (book != null)
            {
                book.InInventory = false;
                await Context.SaveChangesAsync();
            }
            //It doesn't make sense to return a 404 from a delete... it's already gone!!
            return NoContent();
        }

        [HttpPut("/books/{id:int}/genre")]
        public async Task<IActionResult> UpdateGenre(int id,[FromBody] string newGenre)
        {
            var book = await Context.Books.Where(b => b.Id == id && b.InInventory).SingleOrDefaultAsync();
            if (book == null)
            {
                return NotFound();
            }
            else
            {
                book.Genre = newGenre;
                await Context.SaveChangesAsync();
                return NoContent();
            }
        }

        [HttpGet("/books")]
        public async Task<IActionResult> GetAllBooks([FromQuery] string genre = "all")
        {
            var response = new GetBooksResponseCollection();
            var allBooks = Context.Books
                .Where(b=>b.InInventory == true)
                .Select(b => new BookSummaryItem
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
