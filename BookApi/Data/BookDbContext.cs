using BookApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BookApi.Data
{
    public class BookDbContext : DbContext
    {

        public DbSet<book> Books { get; set; }
        public BookDbContext()
        {

        }
        public BookDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=LAPTOP-QM194TV4\SQLEXPRESS;Database=BookSellingDb;Trusted_Connection=True");


        }
    }
}
