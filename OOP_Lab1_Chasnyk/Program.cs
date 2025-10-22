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
                Console.WriteLine("1 – Додати об’єкт (випадковим конструктором)");
                Console.WriteLine("2 – Переглянути всі об’єкти");
                Console.WriteLine("3 – Знайти об’єкт");
                Console.WriteLine("4 – Продемонструвати поведінку (перевантажені методи)");
                Console.WriteLine("5 – Видалити об’єкт");
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
                                DateTime date = DateTime.Parse(Console.ReadLine());

                                Console.Write("Ціна: ");
                                double price = double.Parse(Console.ReadLine());

                                Console.Write("Тип авто (0-Sedan,1-SUV,2-Hatchback,3-Coupe,4-Pickup,5-Van): ");
                                CarType type = (CarType)int.Parse(Console.ReadLine());

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
                        }
                        break;

                    case 3:
                        Console.Write("Введіть марку для пошуку: ");
                        string search = Console.ReadLine();
                        var found = cars.FindAll(c => c.Brand.Equals(search, StringComparison.OrdinalIgnoreCase));

                        if (found.Count == 0)
                            Console.WriteLine("Не знайдено!");
                        else
                        {
                            Console.WriteLine($"Знайдено {found.Count} авто(ів):");
                            foreach (var c in found)
                                c.ShowInfo();
                        }
                        break;

                    case 4:
                        if (cars.Count == 0)
                        {
                            Console.WriteLine("Список порожній!");
                            break;
                        }

                        Console.Write("Введіть номер авто для демонстрації: ");
                        int idx;
                        if (int.TryParse(Console.ReadLine(), out idx) && idx > 0 && idx <= cars.Count)
                        {
                            var car = cars[idx - 1];
                            Console.WriteLine("\n=== Демонстрація поведінки ===");
                            car.StartEngine();          
                            car.StartEngine("Спорт");     
                            car.StopEngine(true);           
                        }
                        else
                        {
                            Console.WriteLine("Невірний номер!");
                        }
                        break;

                    case 5:
                        if (cars.Count == 0)
                        {
                            Console.WriteLine("Список порожній!");
                            break;
                        }

                        Console.Write("Введіть номер авто для видалення: ");
                        int delIdx;
                        if (int.TryParse(Console.ReadLine(), out delIdx) && delIdx > 0 && delIdx <= cars.Count)
                        {
                            cars.RemoveAt(delIdx - 1);
                            Console.WriteLine("Авто видалено.");
                        }
                        else
                            Console.WriteLine("Невірний номер!");
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