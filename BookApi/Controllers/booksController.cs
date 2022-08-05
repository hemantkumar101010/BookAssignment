using BookApi.Data;
using BookApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class booksController : ControllerBase
    {
        private readonly BookDbContext _context;

        public booksController(BookDbContext context)
        {
            _context = context;
        }

        // GET: api/books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<book>>> GetBooks()
        {
          if (_context.Books == null)
          {
              return NotFound();
          }
            return await _context.Books.ToListAsync();
        }

        // GET: api/books/5
        [HttpGet("{id}")]
        public async Task<ActionResult<book>> Getbook(int id)
        {
          if (_context.Books == null)
          {
              return NotFound();
          }
            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            return book;
        }

        // GET: api/Employees?searchByNameOrCity=
        [HttpGet("{searchByNameOrZoner}")]
        public async Task<ActionResult<IEnumerable<book>>> Search(string searchByNameOrZoner)
        {

            if (searchByNameOrZoner == null)
                return NotFound();
            var books = _context.Books.Where(e => e.Name.Contains(searchByNameOrZoner) || e.Zoner.Contains(searchByNameOrZoner)).ToListAsync();
            if (books != null)
                return await books;
            return NotFound();
        }

        // PUT: api/books/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> Putbook(book book)
        {

            _context.Entry(book).State = EntityState.Modified;

            await _context.SaveChangesAsync();
            return Ok("Updated");
        }

        // POST: api/books
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<book>> Postbook(book book)
        {
          if (_context.Books == null)
          {
              return Problem("Entity set 'BookDbContext.Books'  is null.");
          }
            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Getbook", new { id = book.Id }, book);
        }

        // DELETE: api/books/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletebook(int id)
        {
            if (_context.Books == null)
            {
                return NotFound();
            }
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool bookExists(int id)
        {
            return (_context.Books?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
