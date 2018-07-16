using CityInfo.API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace CityInfo.API.Controllers
{
    [Route("api/cities")]
    public class PointsOfInterestController : Controller
    {
        [HttpGet("pointsofinterest")]
        public IActionResult GetPointsOfInterest()
        {
            var result = CitiesDataStore.Current.Cities.SelectMany(c => c.PointsOfInterest);
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpGet("pointsofinterest/{id}")]
        public IActionResult GetPointOfInterestById(int id)
        {
            var result = CitiesDataStore.Current.Cities.SelectMany(c => c.PointsOfInterest).Where(c => c.Id == id).FirstOrDefault();
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpGet("{cityId}/pointsofinterest")]
        public IActionResult GetPointsOfInterestByCityId(int cityId)
        {
            var result = CitiesDataStore.Current.Cities.Where(c => c.Id == cityId).FirstOrDefault();
            if (result == null)
                return NotFound();

            return Ok(result.PointsOfInterest);
        }

        [HttpGet("{cityId}/pointsofinterest/{id}", Name = "GetPointOfInterestByCityIdAndId")]
        public IActionResult GetPointOfInterestByCityIdAndId(int cityId, int id)
        {
            var result = CitiesDataStore.Current.Cities.Where(c => c.Id == cityId).FirstOrDefault();
            if (result == null)
                return NotFound();
            var pointOfInterest = result.PointsOfInterest.Where(c => c.Id == id).FirstOrDefault();

            if (pointOfInterest == null)
                return NotFound();

            return Ok(pointOfInterest);
        }

       [HttpPost("{cityId}/pointsofinterest")]
       public IActionResult CreatePointOfInterest(int cityId, 
           [FromBody] PointOfInterestForCreationDto pointOfInterest)
        {
            if (pointOfInterest == null)
                return BadRequest();

            var city = CitiesDataStore.Current.Cities.Where(c => c.Id == cityId).FirstOrDefault();
            if (city == null)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var id = CitiesDataStore.Current.Cities.SelectMany(c => c.PointsOfInterest).Max(poI => poI.Id);

            var newPointOfInterest = new PointOfInterestDto()
            {
                Id = ++id,
                Name = pointOfInterest.Name,
                Description = pointOfInterest.Description,
            };

            city.PointsOfInterest.Add(newPointOfInterest);

            return CreatedAtRoute("GetPointOfInterestByCityIdAndId",
                new { cityId, id },
                newPointOfInterest);
        }
    }
}
