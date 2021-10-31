using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Library.IdentityAuth;

namespace Library.Models
{
    public class LibraryContext : IdentityDbContext<ApplicationUser>
    {
        public LibraryContext(DbContextOptions<LibraryContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public DbSet<User> User { get; set; }
        public DbSet<UserType> UserType { get; set; }
        public DbSet<Book> Book { get; set; }
        public DbSet<BookType> BookType { get; set; }
        public DbSet<Reservation> Reservation { get; set; }
        public DbSet<ReserveState> ReserveState { get; set; }
    }
}
