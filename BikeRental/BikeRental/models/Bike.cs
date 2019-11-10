using System;
using System.ComponentModel.DataAnnotations;

namespace BikeRental.models
{
    public class Bike
    {
        public int BikeId { get; set; }

        [Required]
        [MaxLength(25)]
        public string Brand { get; set; }

        [Required]
        public DateTime PurchaseDate { get { return PurchaseDate; } set { PurchaseDate = value.Date; } }

        [MaxLength(1000)]
        public string Notes { get; set; }

        public DateTime DateOfLastService { get { return DateOfLastService; } set { DateOfLastService = value.Date; } }

        [Required]
        public double RentalPriceFirstHour
        {
            get { return RentalPriceFirstHour; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Rental price must be greater than 0!");
                }
                RentalPriceFirstHour = Math.Round(value, 2);
            }
        }

        [Required]
        public double RentalPriceAdditionalHour
        {
            get { return RentalPriceAdditionalHour; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Rental price must be greater than 0!");
                }
                RentalPriceAdditionalHour = Math.Round(value, 2);
            }
        }

        [Required]
        public BikeCategories BikeCategory { get; set; }
    }
}
