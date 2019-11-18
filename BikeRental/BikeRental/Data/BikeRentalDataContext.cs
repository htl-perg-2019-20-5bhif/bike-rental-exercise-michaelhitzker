using BikeRental.models;
using Microsoft.EntityFrameworkCore;

namespace BikeRental
{
    public class BikeRentalDataContext : DbContext
    {
        public BikeRentalDataContext(DbContextOptions<BikeRentalDataContext> options)
        : base(options)
        { }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Bike> Bikes { get; set; }

        public DbSet<Rental> Rentals { get; set; }

    }
}
