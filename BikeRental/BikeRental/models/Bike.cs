using System;
using System.ComponentModel.DataAnnotations;

namespace BikeRental.models
{
    public class Bike
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(25)]
        public string Brand { get; set; }

        private DateTime PurchaseDateValue = DateTime.MaxValue;

        [Required]
        public DateTime PurchaseDate { get { return PurchaseDateValue; } set { PurchaseDateValue = value.Date; } }

        [MaxLength(1000)]
        public string Notes { get; set; }

        private DateTime DateOfLastServiceValue = DateTime.MaxValue;

        public DateTime DateOfLastService { get { return DateOfLastServiceValue; } set { DateOfLastServiceValue = value.Date; } }

        private double RentalPriceFirstHourValue = 0;

        [Required]
        [Range(minimum: 0, maximum: double.MaxValue)]
        public double RentalPriceFirstHour
        {
            get { return RentalPriceFirstHourValue; }
            set
            {
                RentalPriceFirstHourValue = Math.Round(value, 2);
            }
        }

        private double RentalPriceAdditionalHourValue = 0;

        [Required]
        [Range(minimum: 0, maximum: double.MaxValue)]
        public double RentalPriceAdditionalHour
        {
            get { return RentalPriceAdditionalHourValue; }
            set
            {
                RentalPriceAdditionalHourValue = Math.Round(value, 2);
            }
        }

        private string BikeCategoryValue = "";

        [Required]
        public string BikeCategory
        {
            get
            {
                return BikeCategoryValue;
            }

            set
            {
                if (!value.Equals("StandardBike") && !value.Equals("MountainBike") && !value.Equals("TreckingBike") && !value.Equals("RacingBike"))
                {
                    throw new ArgumentException("Not a valid BikeCategory");
                }
                BikeCategoryValue = value;
            }
        }
    }
}
