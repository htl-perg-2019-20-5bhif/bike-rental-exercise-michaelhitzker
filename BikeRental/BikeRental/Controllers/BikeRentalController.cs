using Microsoft.AspNetCore.Mvc;

namespace BikeRental.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BikeRentalController : ControllerBase
    {

        public BikeRentalController()
        {
        }
        /*
        [HttpGet]
        [Route("/customers", Name = "GetCustomers")]
        public IActionResult GetCustomers([FromQuery] string lastName)
        {
            if (string.IsNullOrEmpty(lastName))
            {
                return Ok(context.Customers);
            }
            var filteredCustomers = from customer in context.Customers
                                    where customer.Lastname.ToLower().Contains(lastName.ToLower())
                                    select customer;
            if (filteredCustomers.Count() <= 0)
            {
                return NotFound();
            }

            return Ok(filteredCustomers);
        }

        [HttpPost]
        [Route("/customers", Name = "AddCustomer")]
        public async Task<IActionResult> AddCustomer([FromBody] Customer customer)
        {
            context.Customers.Add(customer);
            await context.SaveChangesAsync();
            return Ok(customer.CustomerId);
        }

        [HttpPut]
        [Route("/customers/{id}", Name = "UpdateCustomer")]
        public async Task<IActionResult> UpdateCustomer(int id, [FromBody] Customer customer)
        {
            if (customer.CustomerId != id)
            {
                return BadRequest("Id in url and customerId don't mathc!");
            }
            context.Customers.Update(customer);
            await context.SaveChangesAsync();
            return Ok(customer);
        }

        [HttpDelete]
        [Route("/customers/{id}", Name = "DeleteCustomer")]
        public async Task<IActionResult> UpdateCustomer(int id)
        {
            Customer foundCustomer;
            try
            {
                foundCustomer = context.Customers.Single(customer => customer.CustomerId == id);
            }
            catch (Exception ex)
            {
                return NotFound();
            }

            context.Customers.Remove(foundCustomer);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet]
        [Route("/customers/{id}", Name = "GetActiveRentalsForCustomer")]
        public IActionResult GetActiveRentalsForCustomer(int id)
        {
            var rentals = context.Rentals.Where(rental => rental.Customer.CustomerId == id).ToArray();
            if (rentals.Count() <= 0)
            {
                return NotFound();
            }
            return Ok(rentals);
        }

        [HttpGet]
        [Route("/bikes", Name = "GetBikes")]
        public IActionResult GetBikes([FromQuery] string sortedBy)
        {
            var availableBikes = GetAvailableBikes();

            switch (sortedBy)
            {
                case "firstHour":
                    availableBikes.Sort((a, b) => (int)Math.Round(a.RentalPriceFirstHour - b.RentalPriceFirstHour, 0));
                    return Ok(availableBikes);
                case "additionalHours":
                    availableBikes.Sort((a, b) => (int)Math.Round(a.RentalPriceAdditionalHour - b.RentalPriceAdditionalHour, 0));
                    return Ok(availableBikes);
                case "purchaseDate":
                    availableBikes.Sort((a, b) => DateTime.Compare(a.PurchaseDate, b.PurchaseDate));
                    return Ok(availableBikes);
                default:
                    return Ok(availableBikes);
            }
        }

        [HttpPost]
        [Route("/bikes", Name = "AddBike")]
        public async Task<IActionResult> AddBike([FromBody] Bike bike)
        {
            context.Bikes.Add(bike);
            await context.SaveChangesAsync();
            return Ok(bike.BikeId);
        }

        [HttpPut]
        [Route("/bikes/{id}", Name = "UpdateBike")]
        public async Task<IActionResult> UpdateBike(int id, [FromBody] Bike bike)
        {
            if (bike.BikeId != id)
            {
                return BadRequest("Id in url and bikeId don't mathc!");
            }
            context.Bikes.Update(bike);
            await context.SaveChangesAsync();
            return Ok(bike);
        }

        [HttpDelete]
        [Route("/bikes/{id}", Name = "DeleteBike")]
        public async Task<IActionResult> DeleteBike(int id)
        {
            var rentalsForBikeExist = context.Rentals.Where(rental => rental.Bike.BikeId == id).Count() > 0;
            if (rentalsForBikeExist)
            {
                return BadRequest("Cannot delete bike because rentals for that bike exist!");
            }

            Bike foundBike;
            try
            {
                foundBike = context.Bikes.Single(bikes => bikes.BikeId == id);
            }
            catch (Exception ex)
            {
                return NotFound();
            }

            context.Bikes.Remove(foundBike);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost]
        [Route("/rentals/start", Name = "StartRental")]
        public async Task<IActionResult> StartRental([FromQuery] int customerId, [FromQuery] int bikeId)
        {

            var customerHasActiveRental = context.Rentals.Where(rental => rental.RentalEnd == null && rental.CustomerId == customerId).Count() > 0;
            if (customerHasActiveRental)
            {
                return BadRequest("Customer has already an active rental!");
            }

            var bikeIsNotAvailable = context.Rentals.Where(rental => rental.RentalEnd == null && rental.BikeId == bikeId).Count() > 0;
            if (bikeIsNotAvailable)
            {
                return BadRequest("The bike is currently not available!");
            }

            Rental rental = new Rental { BikeId = bikeId, CustomerId = customerId, RentalBegin = DateTime.Now, Paid = false };
            context.Rentals.Add(rental);
            await context.SaveChangesAsync();
            return Ok(rental);
        }

        [HttpPost]
        [Route("/rentals/end", Name = "EndRental")]
        public async Task<IActionResult> EndRental([FromQuery] int rentalId)
        {
            Rental rental;
            try
            {
                rental = context.Rentals.Single(curRental => curRental.RentalId == rentalId);
            }
            catch (Exception ex)
            {
                return NotFound("A rental with that id does not exist!");
            }
            rental.RentalEnd = DateTime.Now;
            var rentalCosts = CalculateCosts(rental);
            rental.Total = rentalCosts;
            context.Rentals.Update(rental);
            await context.SaveChangesAsync();
            return Ok(rental);
        }

        [HttpPost]
        [Route("/rentals/pay", Name = "PayRental")]
        public async Task<IActionResult> PayRental([FromQuery] int rentalId)
        {
            Rental rental;
            try
            {
                rental = context.Rentals.Single(curRental => curRental.RentalId == rentalId);
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
            context.Rentals.Update(rental);
            await context.SaveChangesAsync();
            return Ok(rental);
        }

        [HttpGet]
        [Route("/rentals/unpaid", Name = "GetUnpaidRentals")]
        public IActionResult GetUnpaidRentals()
        {
            var unpaidRentals = from rental in context.Rentals
                                where rental.RentalEnd != null && !rental.Paid && rental.Total > 0
                                select new { rental.CustomerId, rental.Customer.Firstname, rental.Customer.Lastname, rental.RentalId, rental.RentalBegin, rental.RentalEnd, rental.Total };

            if (unpaidRentals.Count() <= 0)
            {
                return NotFound();
            }
            return Ok(unpaidRentals);
        }

        private double CalculateCosts(Rental rental)
        {
            var minutes = (rental.RentalEnd - rental.RentalBegin).Minutes;
            var firstHourCosts = rental.Bike.RentalPriceFirstHour;
            if (minutes <= 15)
            {
                return 0.0;
            }
            if (minutes <= 60)
            {
                return firstHourCosts;
            }
            var additionalHours = (int)Math.Ceiling((minutes - 60.0) / 60.0);
            var additionalCosts = additionalHours * rental.Bike.RentalPriceAdditionalHour;
            var total = firstHourCosts + additionalCosts;
            return total;
        }

        private List<Bike> GetAvailableBikes()
        {
            var bikes = from bike in context.Bikes
                        join rental in context.Rentals on bike.BikeId equals rental.BikeId into rs
                        from rental in rs.DefaultIfEmpty()
                        where rental.RentalEnd == null
                        select bike;
            return bikes.ToList();
        }*/
    }
}
