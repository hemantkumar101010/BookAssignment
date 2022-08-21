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
            optionsBuilder.UseSqlServer(@"Server=tcp:bookapidbserver.database.windows.net,1433;Initial Catalog=BookApi_db;Persist Security Info=False;User ID=hemant;Password=01364261213@Hemu;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");


        }
    }
}
