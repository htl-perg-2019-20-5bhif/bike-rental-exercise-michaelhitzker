using BikeRental.models;
using System;

namespace BikeRental
{
    public class PriceCalculator : ICalculatePrices
    {
        public double CalculateCosts(Rental rental)
        {
            var minutes = (rental.RentalEnd - rental.RentalBegin).TotalMinutes;

            Console.WriteLine(minutes + "######################## asdfasdfasdfdsdaf");
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
            Console.WriteLine(additionalHours + "######################## ");
            var additionalCosts = additionalHours * rental.Bike.RentalPriceAdditionalHour;
            var total = firstHourCosts + additionalCosts;
            return total;
        }
    }
}
