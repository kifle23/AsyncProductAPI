using AsyncProductAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AsyncProductAPI.Data
{
    public class AppDbContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<ListingRequest> ListingRequests => Set<ListingRequest>();
    }
}
