using BikeRental.models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BikeRental.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        private readonly BikeRentalDataContext _context;
        private readonly ICalculatePrices priceCalculator;

        public RentalsController(BikeRentalDataContext context, ICalculatePrices priceCalculator)
        {
            _context = context;
            this.priceCalculator = priceCalculator;
        }



        [HttpPost("start")]
        public async Task<IActionResult> StartRental([FromQuery] int customerId, [FromQuery] int bikeId)
        {

            var customerHasActiveRental = _context.Rentals.Where(rental => rental.RentalEnd == null && rental.CustomerId == customerId).Count() > 0;
            if (customerHasActiveRental)
            {
                return BadRequest("Customer has already an active rental!");
            }

            var bikeIsNotAvailable = _context.Rentals.Where(rental => rental.RentalEnd == null && rental.BikeId == bikeId).Count() > 0;
            if (bikeIsNotAvailable)
            {
                return BadRequest("The bike is currently not available!");
            }

            Rental rental = new Rental { BikeId = bikeId, CustomerId = customerId, RentalBegin = DateTime.Now, Paid = false };
            _context.Rentals.Add(rental);
            await _context.SaveChangesAsync();
            return Ok(rental);
        }

        [HttpPost("end")]
        public async Task<IActionResult> EndRental([FromQuery] int rentalId)
        {
            Rental rental;
            try
            {
                rental = _context.Rentals.Include(r => r.Bike).Include(r => r.Customer).Single(curRental => curRental.Id == rentalId);
            }
            catch (Exception ex)
            {
                return NotFound("A rental with that id does not exist!");
            }
            rental.RentalEnd = DateTime.Now.AddMinutes(130);
            var rentalCosts = priceCalculator.CalculateCosts(rental);
            rental.Total = rentalCosts;
            _context.Rentals.Update(rental);
            await _context.SaveChangesAsync();
            return Ok(rental);
        }

        [HttpPost("pay")]
        public async Task<IActionResult> PayRental([FromQuery] int rentalId)
        {
            Rental rental;
            try
            {
                rental = _context.Rentals.Include(r => r.Bike).Include(r => r.Customer).Single(curRental => curRental.Id == rentalId);
            }
            catch (Exception ex)
            {
                return NotFound("A rental with that id does not exist!");
            }
            if (rental.Total <= 0)
            {
                return BadRequest("Free rentals cannot be paid!");
            }
            rental.Paid = true;
            _context.Rentals.Update(rental);
            await _context.SaveChangesAsync();
            return Ok(rental);
        }

        [HttpGet("unpaid")]
        public IActionResult GetUnpaidRentals()
        {
            var unpaidRentals = from rental in _context.Rentals
                                where rental.RentalEnd != null && !rental.Paid && rental.Total > 0
                                select new { rental.CustomerId, rental.Customer.Firstname, rental.Customer.Lastname, rental.Id, rental.RentalBegin, rental.RentalEnd, rental.Total };

            if (unpaidRentals.Count() <= 0)
            {
                return NotFound();
            }
            return Ok(unpaidRentals);
        }

        private bool RentalExists(int id)
        {
            return _context.Rentals.Any(e => e.Id == id);
        }
    }
}
