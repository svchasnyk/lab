using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace CarApp
{
    public class Car
    {
        private static int carCount = 0;
        public static int CarCount => carCount;
        public static string Category { get; set; } = "Легкові автомобілі";

        private string brand;
        private string model;
        private DateTime manufactureDate;
        private double price;
        private CarType type;

        [JsonPropertyName("color")]
        public string Color { get; set; } = "Black";

        [JsonIgnore]
        public int Age => DateTime.Now.Year - ManufactureDate.Year;

        [JsonIgnore]
        public string VIN { get; private set; }

        [JsonPropertyName("brand")]
        public string Brand
        {
            get { return brand; }
            set
            {
                if (value.Length < 2 || value.Length > 15 || !Regex.IsMatch(value, @"^[A-Za-z]+$"))
                    throw new ArgumentException("Марка повинна містити лише латинські літери (2-15).");
                brand = char.ToUpper(value[0]) + value.Substring(1);
            }
        }

        [JsonPropertyName("model")]
        public string Model
        {
            get { return model; }
            set
            {
                if (value.Length < 1 || value.Length > 20)
                    throw new ArgumentException("Модель повинна містити від 1 до 20 символів.");
                model = value;
            }
        }

        [JsonPropertyName("date")]
        public DateTime ManufactureDate
        {
            get { return manufactureDate; }
            set
            {
                if (value < new DateTime(1960, 1, 1) || value > DateTime.Now)
                    throw new ArgumentException("Дата випуску має бути між 01.01.1960 і сьогодні.");
                manufactureDate = value;
            }
        }

        [JsonPropertyName("price")]
        public double Price
        {
            get { return price; }
            set
            {
                if (value <= 0 || value > 200000)
                    throw new ArgumentOutOfRangeException(nameof(Price), "Ціна має бути в діапазоні (0; 200000].");
                price = value;
            }
        }

        [JsonPropertyName("type")]
        public CarType Type
        {
            get { return type; }
            set { type = value; }
        }

        public Car()
        {
            Brand = "Default";
            Model = "ModelX";
            ManufactureDate = new DateTime(2020, 1, 1);
            Price = 10000;
            Type = CarType.Sedan;
            VIN = GenerateVIN();
            carCount++;
        }

        public Car(string brand, string model) : this()
        {
            Brand = brand;
            Model = model;
        }

        public Car(string brand, string model, DateTime manufactureDate, double price, CarType type)
        {
            Brand = brand;
            Model = model;
            ManufactureDate = manufactureDate;
            Price = price;
            Type = type;
            VIN = GenerateVIN();
            carCount++;
        }

        private string GenerateVIN()
        {
            return Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
        }

        private string FormatInfo()
        {
            return $"Марка: {Brand}, Модель: {Model}, Дата: {ManufactureDate:dd.MM.yyyy}, " +
                   $"Ціна: {Price} USD, Тип: {Type}, Колір: {Color}, Вік: {Age} років, VIN: {VIN}";
        }

        public string GetInfo() => FormatInfo();

        public void ShowInfo() => Console.WriteLine(GetInfo());

        public string StartEngineMessage() => $"Двигун {Brand} {Model} завівся!";
        public string StartEngineMessage(string mode) => $"Двигун {Brand} {Model} завівся у режимі: {mode}!";
        public string StopEngineMessage() => $"Двигун {Brand} {Model} заглох.";
        public string StopEngineMessage(bool withDelay) =>
            withDelay ? $"Двигун {Brand} {Model} заглох після затримки..." : StopEngineMessage();
        public void StartEngine() => Console.WriteLine(StartEngineMessage());
        public void StartEngine(string mode) => Console.WriteLine(StartEngineMessage(mode));
        public void StopEngine() => Console.WriteLine(StopEngineMessage());
        public void StopEngine(bool withDelay) => Console.WriteLine(StopEngineMessage(withDelay));

        public static double AveragePrice(List<Car> cars)
        {
            if (cars == null || cars.Count == 0)
                throw new ArgumentException("Список порожній!");
            double sum = 0;
            foreach (var c in cars) sum += c.Price;
            return sum / cars.Count;
        }

        public static Car FindMostExpensive(List<Car> cars)
        {
            if (cars == null || cars.Count == 0)
                throw new ArgumentException("Список порожній!");
            Car max = cars[0];
            foreach (var c in cars)
                if (c.Price > max.Price) max = c;
            return max;
        }

        public static Car Parse(string input)
        {
            try
            {
                string[] parts = input.Split(';');
                if (parts.Length != 5)
                    throw new FormatException("Невірний формат. Приклад: Brand;Model;dd.MM.yyyy;10000;SUV");

                string brand = parts[0];
                string model = parts[1];
                DateTime date = DateTime.Parse(parts[2]);
                double price = double.Parse(parts[3]);
                CarType type = (CarType)Enum.Parse(typeof(CarType), parts[4], true);

                return new Car(brand, model, date, price, type);
            }
            catch (Exception ex)
            {
                throw new FormatException($"Помилка парсингу: {ex.Message}");
            }
        }

        public static bool TryParse(string input, out Car result)
        {
            try
            {
                result = Parse(input);
                return true;
            }
            catch
            {
                result = null;
                return false;
            }
        }

        public override string ToString()
        {
            return $"{Brand};{Model};{ManufactureDate:dd.MM.yyyy};{Price};{Type}";
        }
    }
}