using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CarApp
{
    public static class CarSerializer
    {
        public static void SaveToCsv(List<Car> cars, string filePath)
        {
            using (var writer = new StreamWriter(filePath))
            {
                writer.WriteLine("Brand;Model;Date;Price;Type;Color;VIN");
                foreach (var car in cars)
                {
                    writer.WriteLine($"{car.Brand};{car.Model};{car.ManufactureDate:dd.MM.yyyy};{car.Price};{car.Type};{car.Color};{car.VIN}");
                }
            }
        }

        public static List<Car> LoadFromCsv(string filePath)
        {
            var cars = new List<Car>();
            var lines = File.ReadAllLines(filePath).Skip(1); // пропускаємо заголовок
            foreach (var line in lines)
            {
                try
                {
                    var parts = line.Split(';');
                    if (parts.Length < 5) continue;

                    string brand = parts[0];
                    string model = parts[1];
                    DateTime date = DateTime.Parse(parts[2]);
                    double price = double.Parse(parts[3]);
                    CarType type = (CarType)Enum.Parse(typeof(CarType), parts[4], true);

                    var car = new Car(brand, model, date, price, type);
                    if (parts.Length > 5) car.Color = parts[5];
                    cars.Add(car);
                }
                catch { continue; }
            }
            Console.WriteLine($"✅ Завантажено {cars.Count} авто з CSV.");
            return cars;
        }
    }
}
