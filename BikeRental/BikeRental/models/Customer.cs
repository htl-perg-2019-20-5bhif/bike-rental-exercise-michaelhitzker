using System;
using System.ComponentModel.DataAnnotations;

namespace BikeRental.models
{
    public class Customer
    {
        public int Id { get; set; }

        private string GenderValue = "";

        [Required]
        public string Gender
        {
            get
            {
                return GenderValue;
            }
            set
            {
                if (!value.Equals("Male") && !value.Equals("Female") && !value.Equals("Unknown"))
                {
                    throw new ArgumentException("Not a valid gender");
                }
                GenderValue = value;
            }
        }

        [Required]
        [MaxLength(50)]
        public string Firstname { get; set; }

        [Required]
        [MaxLength(75)]
        public string Lastname { get; set; }

        private DateTime BirthdayValue = DateTime.MaxValue;
        [Required]
        public DateTime Birthday { get { return BirthdayValue; } set { BirthdayValue = value.Date; } }

        [Required]
        [MaxLength(75)]
        public string Street { get; set; }

        [Range(minimum: 0, maximum: 1000000000)]
        public int HouseNumber { get; set; }

        [Required]
        [Range(minimum: 0, maximum: 1000000000)]
        public int ZipCode { get; set; }

        [Required]
        [MaxLength(75)]
        public string Town { get; set; }

    }
}
