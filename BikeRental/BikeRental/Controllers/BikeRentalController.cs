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

        [HttpGet]
        public IActionResult Get()
        {
            return Ok();
        }
    }
}
