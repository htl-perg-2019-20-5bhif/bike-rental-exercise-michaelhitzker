using BikeRental.models;

namespace BikeRental
{
    public interface ICalculatePrices
    {
        double CalculateCosts(Rental rental);
    }
}
