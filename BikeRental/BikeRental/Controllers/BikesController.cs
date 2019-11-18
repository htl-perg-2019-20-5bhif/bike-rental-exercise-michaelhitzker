using BikeRental.models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeRental.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BikesController : ControllerBase
    {
        private readonly BikeRentalDataContext _context;

        public BikesController(BikeRentalDataContext context)
        {
            _context = context;
        }

        // GET: api/Bikes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Bike>>> GetBikes([FromQuery] string sortedBy)
        {
            var availableBikes = await GetAvailableBikes();

            switch (sortedBy)
            {
                case "firstHour":
                    availableBikes.Sort((a, b) => (int)Math.Round(a.RentalPriceFirstHour - b.RentalPriceFirstHour, 0));
                    break;
                case "additionalHours":
                    availableBikes.Sort((a, b) => (int)Math.Round(a.RentalPriceAdditionalHour - b.RentalPriceAdditionalHour, 0));
                    break;
                case "purchaseDate":
                    availableBikes.Sort((a, b) => DateTime.Compare(a.PurchaseDate, b.PurchaseDate));
                    break;
                default:
                    break;
            }

            return availableBikes;
        }

        // GET: api/Bikes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Bike>> GetBike(int id)
        {
            var bike = await _context.Bikes.FindAsync(id);

            if (bike == null)
            {
                return NotFound();
            }

            return bike;
        }

        // PUT: api/Bikes/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBike(int id, Bike bike)
        {
            if (id != bike.Id)
            {
                return BadRequest();
            }

            _context.Entry(bike).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BikeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Bikes
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Bike>> PostBike(Bike bike)
        {
            _context.Bikes.Add(bike);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBike", new { id = bike.Id }, bike);
        }

        // DELETE: api/Bikes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Bike>> DeleteBike(int id)
        {
            var rentalsForBikeExist = _context.Rentals.Where(rental => rental.Bike.Id == id).Count() > 0;
            if (rentalsForBikeExist)
            {
                return BadRequest("Cannot delete bike because rentals for that bike exist!");
            }

            var bike = await _context.Bikes.FindAsync(id);
            if (bike == null)
            {
                return NotFound();
            }

            _context.Bikes.Remove(bike);
            await _context.SaveChangesAsync();

            return bike;
        }

        private bool BikeExists(int id)
        {
            return _context.Bikes.Any(e => e.Id == id);
        }

        private async Task<List<Bike>> GetAvailableBikes()
        {
            var bikesInUse = await _context.Rentals.Where(r => r.RentalEnd == DateTime.MaxValue).Select(r => r.BikeId).ToListAsync();
            var availableBikes = _context.Bikes.Where(b => !bikesInUse.Contains(b.Id));
            return await availableBikes.ToListAsync();
        }
    }
}
