using System;
using System.ComponentModel.DataAnnotations;

namespace BikeRental.models
{
    public class Customer
    {
        public string CustomerId { get; set; }

        [Required]
        public Genders Gender { get; set; }

        [Required]
        [MaxLength(50)]
        public string Firstname { get; set; }

        [Required]
        [MaxLength(75)]
        public string Lastname { get; set; }

        [Required]
        public DateTime Birthday { get { return Birthday; } set { Birthday = value.Date; } }

        [Required]
        [MaxLength(75)]
        public string Street { get; set; }

        [MaxLength(10)]
        public int HouseNumber { get; set; }

        [Required]
        [MaxLength(10)]
        public int ZipCode { get; set; }

        [Required]
        [MaxLength(75)]
        public string Town { get; set; }

    }
}
