using BikeRental.models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BikeRental.Tests
{
    [TestClass]
    public class PriceCalculationTests
    {
        [TestMethod]
        public void TestFreeRide()
        {
            PriceCalculator calculator = new PriceCalculator();
            var dateTime = new DateTime();
            var bike = new Bike { RentalPriceFirstHour = 10, RentalPriceAdditionalHour = 20, Id = 1 };
            var rental = new Rental { BikeId = 1, CustomerId = 1, Id = 1, RentalBegin = dateTime, RentalEnd = dateTime.AddMinutes(1), Bike = bike };
            var costs = calculator.CalculateCosts(rental);
            Assert.AreEqual(0, costs);
        }

        [TestMethod]
        public void TestOneHour()
        {
            PriceCalculator calculator = new PriceCalculator();
            var dateTime = new DateTime();
            var bike = new Bike { RentalPriceFirstHour = 10, RentalPriceAdditionalHour = 20, Id = 1 };
            var rental = new Rental { BikeId = 1, CustomerId = 1, Id = 1, RentalBegin = dateTime, RentalEnd = dateTime.AddMinutes(60), Bike = bike };
            var costs = calculator.CalculateCosts(rental);
            Assert.AreEqual(10, costs);
        }

        [TestMethod]
        public void TestAdditionalHours()
        {
            PriceCalculator calculator = new PriceCalculator();
            var dateTime = new DateTime();
            var bike = new Bike { RentalPriceFirstHour = 10, RentalPriceAdditionalHour = 20, Id = 1 };
            var rental = new Rental { BikeId = 1, CustomerId = 1, Id = 1, RentalBegin = dateTime, RentalEnd = dateTime.AddHours(2), Bike = bike };
            var costs = calculator.CalculateCosts(rental);
            Assert.AreEqual(30, costs);
        }
    }
}
