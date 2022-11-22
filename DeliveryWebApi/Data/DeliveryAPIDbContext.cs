using DeliveryWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DeliveryWebApi.Data
{
    public class DeliveryAPIDbContext:DbContext
    {
        public DeliveryAPIDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Delivery> Deliveries { get; set; }
    }
}
