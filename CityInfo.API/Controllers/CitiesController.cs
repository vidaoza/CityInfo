using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Controllers
{
    [Route("api/cities")]
    public class CitiesController : Controller
    {
        [HttpGet()]
        public IActionResult GetCities()
        {
            var cities = CitiesDataStore.Current.Cities;

            if (cities == null || cities.Count() == 0)
                return NotFound("No cities were found");

            return Ok(cities);
        }

        [HttpGet("{id}")]
        public IActionResult GetCity(int id)
        {
            var result = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == id);
            if (result == null)
                return NotFound($"City with id {id} was not found.");

            return Ok(result);
        }
    }
}
 