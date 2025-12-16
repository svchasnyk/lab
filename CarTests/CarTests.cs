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

        [TestMethod]
        public void TryParse_InvalidString_ShouldReturnFalse()
        {
            string input = "WrongData";
            bool result = Car.TryParse(input, out Car car);
            Assert.IsFalse(result);
            Assert.IsNull(car);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Brand_InvalidCharacters_ShouldThrowException()
        {
            var car = new Car("B1", "Test", DateTime.Now.AddYears(-1), 10000, CarType.Sedan);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Price_InvalidValue_ShouldThrowException()
        {
            var car = new Car("BMW", "X5", DateTime.Now.AddYears(-1), -500, CarType.SUV);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ManufactureDate_InvalidValue_ShouldThrowException()
        {
            var car = new Car("Audi", "A4", new DateTime(1950, 1, 1), 20000, CarType.Sedan);
        }

        [TestMethod]
        public void ToString_ShouldReturnCorrectFormat()
        {
            var car = new Car("Honda", "Civic", new DateTime(2018, 5, 20), 18000, CarType.Sedan);
            string expected = "Honda;Civic;20.05.2018;18000;Sedan";
            Assert.AreEqual(expected, car.ToString());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AveragePrice_EmptyList_ShouldThrowException()
        {
            var cars = new List<Car>();
            Car.AveragePrice(cars);
        }

        [TestMethod]
        public void TestCsvSerialization()
        {
            var cars = new List<Car> { new Car("BMW", "X5", DateTime.Now, 50000, CarType.SUV) };
            CarSerializer.SaveToCsv(cars, "test.csv");
            var loaded = CarSerializer.LoadFromCsv("test.csv");
            Assert.AreEqual(1, loaded.Count);
            Assert.AreEqual("BMW", loaded[0].Brand);
        }

        [TestMethod]
        public void TestJsonSerialization()
        {
            var cars = new List<Car> { new Car("Audi", "A4", DateTime.Now, 30000, CarType.Sedan) };
            CarJsonSerializer.SaveToJson(cars, "test.json");
            var loaded = CarJsonSerializer.LoadFromJson("test.json");
            Assert.AreEqual(1, loaded.Count);
            Assert.AreEqual("Audi", loaded[0].Brand);
        }


    }
}