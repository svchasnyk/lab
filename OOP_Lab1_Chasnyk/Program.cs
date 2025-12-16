using System;
using System.Collections.Generic;

namespace CarApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.Write("Введіть кількість автомобілів N: ");
            int N;
            while (!int.TryParse(Console.ReadLine(), out N) || N <= 0)
            {
                Console.Write("Помилка! Введіть додатнє число: ");
            }

            List<Car> cars = new List<Car>();
            int choice;
            Random rand = new Random();

            do
            {
                Console.WriteLine("\nМеню:");
                Console.WriteLine("1 – Додати об’єкт");
                Console.WriteLine("2 – Переглянути всі об’єкти");
                Console.WriteLine("3 – Знайти об’єкт");
                Console.WriteLine("4 – Продемонструвати поведінку (перевантажені методи)");
                Console.WriteLine("5 – Видалити об’єкт");
                Console.WriteLine("6 – Продемонструвати static-методи");
                Console.WriteLine("7 – Зберегти колекцію об’єктів у файлі");
                Console.WriteLine("8 – Зчитати колекцію об’єктів з файлу");
                Console.WriteLine("9 – Очистити колекцію");
                Console.WriteLine("0 – Вийти");

                Console.Write("Ваш вибір: ");
                if (!int.TryParse(Console.ReadLine(), out choice))
                {
                    Console.WriteLine("Помилка вводу!");
                    continue;
                }

                switch (choice)
                {
                    case 1:
                        if (cars.Count >= N)
                        {
                            Console.WriteLine("Досягнуто максимум N об’єктів!");
                            break;
                        }

                        Console.Write("Хочете додати авто через рядок? (y/n): ");
                        string ans = Console.ReadLine();
                        if (ans?.ToLower() == "y")
                        {
                            Console.Write("Введіть дані (Brand;Model;dd.MM.yyyy;Price;Type): ");
                            string input = Console.ReadLine();
                            if (Car.TryParse(input, out Car parsedCar))
                            {
                                cars.Add(parsedCar);
                                Console.WriteLine("✅ Авто додано через TryParse!");
                                parsedCar.ShowInfo();
                            }
                            else Console.WriteLine("❌ Невірний формат!");
                        }
                        else
                        {
                            try
                            {
                                int constructorChoice = rand.Next(1, 4);
                                Car car;

                                if (constructorChoice == 1)
                                {
                                    car = new Car();
                                }
                                else if (constructorChoice == 2)
                                {
                                    Console.Write("Марка: ");
                                    string brand = Console.ReadLine();

                                    Console.Write("Модель: ");
                                    string model = Console.ReadLine();

                                    car = new Car(brand, model);
                                }
                                else
                                {
                                    Console.Write("Марка: ");
                                    string brand = Console.ReadLine();

                                    Console.Write("Модель: ");
                                    string model = Console.ReadLine();

                                    Console.Write("Дата випуску (дд.MM.рррр): ");
                                    if (!DateTime.TryParse(Console.ReadLine(), out DateTime date))
                                    {
                                        Console.WriteLine("❌ Невірний формат дати!");
                                        break;
                                    }

                                    Console.Write("Ціна: ");
                                    if (!double.TryParse(Console.ReadLine(), out double price))
                                    {
                                        Console.WriteLine("❌ Невірний формат ціни!");
                                        break;
                                    }

                                    Console.Write("Тип авто (0-Sedan,1-SUV,2-Hatchback,3-Coupe,4-Pickup,5-Van): ");
                                    if (!int.TryParse(Console.ReadLine(), out int typeInt) || typeInt < 0 || typeInt > 5)
                                    {
                                        Console.WriteLine("❌ Невірний тип!");
                                        break;
                                    }
                                    CarType type = (CarType)typeInt;

                                    car = new Car(brand, model, date, price, type);
                                }

                                Console.Write("Колір (Enter для дефолтного): ");
                                string color = Console.ReadLine();
                                if (!string.IsNullOrWhiteSpace(color))
                                    car.Color = color;

                                cars.Add(car);
                                Console.WriteLine("✅ Автомобіль додано!");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"❌ Помилка: {ex.Message}");
                            }
                        }
                        break;

                    case 2:
                        if (cars.Count == 0)
                            Console.WriteLine("Список порожній.");
                        else
                        {
                            Console.WriteLine("\nСписок автомобілів:");
                            int i = 1;
                            foreach (var c in cars)
                            {
                                Console.Write($"{i}. ");
                                c.ShowInfo();
                                i++;
                            }
                            Console.WriteLine($"🔸 Всього авто: {Car.CarCount}, Категорія: {Car.Category}");
                        }
                        break;

                    case 3:
                        Console.Write("Введіть VIN або марку для пошуку: ");
                        string query = Console.ReadLine();
                        var found = cars.Find(c => c.VIN.Equals(query, StringComparison.OrdinalIgnoreCase)
                                                || c.Brand.Equals(query, StringComparison.OrdinalIgnoreCase));
                        if (found != null) found.ShowInfo();
                        else Console.WriteLine("❌ Авто не знайдено!");
                        break;

                    case 4:
                        if (cars.Count > 0)
                        {
                            var car = cars[0];
                            car.StartEngine();
                            car.StartEngine("Sport");
                            car.StopEngine();
                            car.StopEngine(true);
                        }
                        else Console.WriteLine("Список порожній!");
                        break;

                    case 5:
                        Console.Write("Введіть номер авто для видалення: ");
                        if (int.TryParse(Console.ReadLine(), out int index) && index > 0 && index <= cars.Count)
                        {
                            cars.RemoveAt(index - 1);
                            Console.WriteLine("✅ Авто видалено!");
                        }
                        else
                        {
                            Console.WriteLine("❌ Невірний номер!");
                        }
                        break;

                    case 6:
                        if (cars.Count == 0)
                        {
                            Console.WriteLine("Список порожній!");
                            break;
                        }

                        try
                        {
                            double avg = Car.AveragePrice(cars);
                            Console.WriteLine($"📊 Середня ціна: {avg} USD");

                            Car expensive = Car.FindMostExpensive(cars);
                            Console.WriteLine("💎 Найдорожче авто:");
                            expensive.ShowInfo();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"❌ Помилка: {ex.Message}");
                        }
                        break;

                    case 7:
                        Console.WriteLine("1 – зберегти у CSV");
                        Console.WriteLine("2 – зберегти у JSON");
                        string saveChoice = Console.ReadLine();
                        if (saveChoice == "1") CarSerializer.SaveToCsv(cars, "cars.csv");
                        else if (saveChoice == "2") CarJsonSerializer.SaveToJson(cars, "cars.json");
                        break;

                    case 8:
                        Console.WriteLine("1 – зчитати з CSV");
                        Console.WriteLine("2 – зчитати з JSON");
                        string loadChoice = Console.ReadLine();
                        if (loadChoice == "1") cars.AddRange(CarSerializer.LoadFromCsv("cars.csv"));
                        else if (loadChoice == "2") cars.AddRange(CarJsonSerializer.LoadFromJson("cars.json"));
                        break;

                    case 9:
                        cars.Clear();
                        Console.WriteLine("✅ Колекцію очищено!");
                        break;

                    case 0:
                        Console.WriteLine("Вихід...");
                        break;

                    default:
                        Console.WriteLine("Невірний вибір!");
                        break;
                }
            } while (choice != 0);
        }
    }
}