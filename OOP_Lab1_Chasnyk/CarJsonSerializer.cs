using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace CarApp
{
    public static class CarJsonSerializer
    {
        public static void SaveToJson(List<Car> cars, string filePath)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            string json = JsonSerializer.Serialize(cars, options);
            File.WriteAllText(filePath, json);
        }

        public static List<Car> LoadFromJson(string filePath)
        {
            string json = File.ReadAllText(filePath);
            var cars = JsonSerializer.Deserialize<List<Car>>(json);
            Console.WriteLine($"✅ Завантажено {cars.Count} авто з JSON.");
            return cars;
        }
    }
}
