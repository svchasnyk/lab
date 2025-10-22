using System;
using System.Text.RegularExpressions;

namespace CarApp
{
    public class Car
    {
        private string brand;
        private string model;
        private DateTime manufactureDate;
        private double price;
        private CarType type;

        public string Color { get; set; } = "Black";
        public int Age => DateTime.Now.Year - ManufactureDate.Year;
        public string VIN { get; private set; }

        public string Brand
        {
            get { return brand; }
            set
            {
                if (value.Length < 2 || value.Length > 15 || !Regex.IsMatch(value, @"^[A-Za-z]+$"))
                    throw new ArgumentException("Марка повинна містити лише латинські літери (2-15).");
                brand = char.ToUpper(value[0]) + value.Substring(1).ToLower();
            }
        }

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
            Console.WriteLine("🔹 Спрацював конструктор без параметрів!");
        }
        public Car(string brand, string model) : this()
        {
            Brand = brand;
            Model = model;
            Console.WriteLine("🔹 Спрацював конструктор з 2 параметрами (brand, model)!");
        }
        public Car(string brand, string model, DateTime manufactureDate, double price, CarType type)
        {
            Brand = brand;
            Model = model;
            ManufactureDate = manufactureDate;
            Price = price;
            Type = type;
            VIN = GenerateVIN();
            Console.WriteLine("🔹 Спрацював повний конструктор!");
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

        public void ShowInfo()
        {
            Console.WriteLine(FormatInfo());
        }

        public void StartEngine()
        {
            Console.WriteLine($"Двигун {Brand} {Model} завівся!");
        }
        public void StartEngine(string mode)
        {
            Console.WriteLine($"Двигун {Brand} {Model} завівся у режимі: {mode}!");
        }

        public void StopEngine()
        {
            Console.WriteLine($"Двигун {Brand} {Model} заглох.");
        }
        public void StopEngine(bool withDelay)
        {
            if (withDelay)
            {
                Console.WriteLine($"Двигун {Brand} {Model} заглох після затримки...");
            }
            else StopEngine();
        }
    }
}