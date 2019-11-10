using BikeRental.models;
using Microsoft.EntityFrameworkCore;

namespace BikeRental
{
    public class BikeRentalDataContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=MICHAS-BLECHHAU;Database=BikeRental;Integrated Security=SSPI");
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Bike> Bikes { get; set; }

        public DbSet<Rental> Rentals { get; set; }

    }
}
