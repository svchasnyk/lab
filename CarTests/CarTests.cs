using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace CarApp.Tests
{
    [TestClass]
    public class CarTests
    {
        private Car defaultCar;

        [TestInitialize]
        public void Setup()
        {
            defaultCar = new Car();
        }

        [TestCleanup]
        public void Cleanup()
        {
            defaultCar = null;
        }

        [TestMethod]
        public void Constructor_DefaultCar_ShouldIncreaseCount()
        {
            int before = Car.CarCount;
            var car = new Car();
            Assert.AreEqual(before + 1, Car.CarCount);
        }

        [TestMethod]
        public void Parse_ValidString_ShouldReturnCar()
        {
            string input = "Toyota;Camry;12.05.2018;15000;Sedan";
            Car car = Car.Parse(input);
            Assert.AreEqual("Toyota", car.Brand);
            Assert.AreEqual("Camry", car.Model);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void Parse_InvalidString_ShouldThrowException()
        {
            string input = "WrongData";
            Car.Parse(input);
        }

        [TestMethod]
        [DataRow("BMW", "X5", "01.01.2020", 50000, CarType.SUV)]
        [DataRow("Audi", "A4", "15.03.2019", 30000, CarType.Sedan)]
        public void FullConstructor_ShouldSetProperties(string brand, string model, string date, double price, CarType type)
        {
            DateTime manufactureDate = DateTime.Parse(date);
            Car car = new Car(brand, model, manufactureDate, price, type);

            Assert.AreEqual(brand, car.Brand);
            Assert.AreEqual(model, car.Model);
            Assert.AreEqual(price, car.Price);
            Assert.AreEqual(type, car.Type);
        }

        [TestMethod]
        public void AveragePrice_ShouldReturnCorrectValue()
        {
            var cars = new List<Car>
            {
                new Car("BMW", "X5", new DateTime(2020,1,1), 50000, CarType.SUV),
                new Car("Audi", "A4", new DateTime(2019,3,15), 30000, CarType.Sedan)
            };

            double avg = Car.AveragePrice(cars);
            Assert.AreEqual(40000, avg);
        }

        [TestMethod]
        public void FindMostExpensive_ShouldReturnCarWithMaxPrice()
        {
            var cars = new List<Car>
            {
                new Car("BMW", "X5", new DateTime(2020,1,1), 50000, CarType.SUV),
                new Car("Audi", "A4", new DateTime(2019,3,15), 30000, CarType.Sedan)
            };

            Car expensive = Car.FindMostExpensive(cars);
            Assert.AreEqual("BMW", expensive.Brand);
        }
    }
}
