using ClubSystemWebApp.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace ClubSystemWebApp.Data
{
    public class ClubSystemDBContext : DbContext
    {
        public ClubSystemDBContext(DbContextOptions options) : base(options)
        {
           
        }

        public DbSet<Person> Persons { get; set; }

        public DbSet<Membership> Memberships { get; set; }
    }
}
