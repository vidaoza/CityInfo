using CityInfo.API.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Models
{
    public class PointOfInterestDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class PointOfInterestForCreationDto
    {   
        [Required(ErrorMessage ="The {0} is required.")]
        [MaxLength(50, ErrorMessage = "The maximum length for {0} is {1} characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The {0} is required.")]
        [MaxLength(250, ErrorMessage = "The maximum length for {0} is {1} characters.")]
        [NotEqualTo("Name", ErrorMessage = "{0} should not match {1}.")]
        public string Description { get; set; }

    }

    public class PointOfInterestForUpdateDto
    {
        [Required(ErrorMessage = "The {0} is required.")]
        [MaxLength(50, ErrorMessage = "The maximum length for {0} is {1} characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The {0} is required.")]
        [MaxLength(250, ErrorMessage = "The maximum length for {0} is {1} characters.")]
        [NotEqualTo("Name", ErrorMessage = "{0} should not match {1}.")]
        public string Description { get; set; }

    }
}
