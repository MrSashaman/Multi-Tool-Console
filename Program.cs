using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

class MultiToolApp
{
    static void Main(string[] args)
    {
        Console.WriteLine("Добро пожаловать в Мультитул!");
        Console.WriteLine("Выберите функцию:");
        Console.WriteLine("1. Калькулятор");
        Console.WriteLine("2. Заметки");
        Console.WriteLine("3. Генератор паролей");
        Console.WriteLine("4. Конвертер валют");
        Console.WriteLine("5. Таймер");
        Console.WriteLine("0. Выход");

        bool running = true;
        while (running)
        {
            Console.Write("\nВведите номер функции (0-5): ");
            string input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    Calculator();
                    break;
                case "2":
                    Notes();
                    break;
                case "3":
                    PasswordGenerator();
                    break;
                case "4":
                    CurrencyConverter();
                    break;
                case "5":
                    Timer();
                    break;
                case "0":
                    running = false;
                    break;
                default:
                    Console.WriteLine("Неверный ввод. Попробуйте снова.");
                    break;
            }
        }

        Console.WriteLine("Спасибо за использование Мультитула. До свидания!");
    }

    static void Calculator()
    {
        Console.WriteLine("\n=== Калькулятор ===");
        Console.WriteLine("Доступные операции: +, -, *, /, ^ (степень), % (остаток)");

        try
        {
            Console.Write("Введите первое число: ");
            double num1 = double.Parse(Console.ReadLine());

            Console.Write("Введите операцию: ");
            char op = Console.ReadLine()[0];

            Console.Write("Введите второе число: ");
            double num2 = double.Parse(Console.ReadLine());

            double result = 0;
            switch (op)
            {
                case '+':
                    result = num1 + num2;
                    break;
                case '-':
                    result = num1 - num2;
                    break;
                case '*':
                    result = num1 * num2;
                    break;
                case '/':
                    if (num2 == 0)
                    {
                        Console.WriteLine("Ошибка: деление на ноль!");
                        return;
                    }
                    result = num1 / num2;
                    break;
                case '^':
                    result = Math.Pow(num1, num2);
                    break;
                case '%':
                    result = num1 % num2;
                    break;
                default:
                    Console.WriteLine("Неверная операция!");
                    return;
            }

            Console.WriteLine($"Результат: {num1} {op} {num2} = {result}");
        }
        catch (FormatException)
        {
            Console.WriteLine("Ошибка: неверный формат числа!");
        }
    }

    static void Notes()
    {
        Console.WriteLine("\n=== Заметки ===");
        Console.WriteLine("1. Просмотреть заметки");
        Console.WriteLine("2. Добавить заметку");
        Console.WriteLine("3. Удалить заметку");
        Console.WriteLine("4. Назад");

        string notesFile = "notes.txt";

        if (!File.Exists(notesFile))
        {
            File.Create(notesFile).Close();
        }

        Console.Write("Выберите действие: ");
        string choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                string[] notes = File.ReadAllLines(notesFile);
                if (notes.Length == 0)
                {
                    Console.WriteLine("Заметок нет.");
                }
                else
                {
                    Console.WriteLine("\nСписок заметок:");
                    for (int i = 0; i < notes.Length; i++)
                    {
                        Console.WriteLine($"{i + 1}. {notes[i]}");
                    }
                }
                break;
            case "2":
                Console.Write("Введите текст заметки: ");
                string newNote = Console.ReadLine();
                File.AppendAllText(notesFile, newNote + Environment.NewLine);
                Console.WriteLine("Заметка добавлена.");
                break;
            case "3":
                string[] allNotes = File.ReadAllLines(notesFile);
                if (allNotes.Length == 0)
                {
                    Console.WriteLine("Нет заметок для удаления.");
                    break;
                }

                Console.WriteLine("Выберите номер заметки для удаления:");
                for (int i = 0; i < allNotes.Length; i++)
                {
                    Console.WriteLine($"{i + 1}. {allNotes[i]}");
                }

                if (int.TryParse(Console.ReadLine(), out int noteNumber) && noteNumber > 0 && noteNumber <= allNotes.Length)
                {
                    var notesList = new List<string>(allNotes);
                    notesList.RemoveAt(noteNumber - 1);
                    File.WriteAllLines(notesFile, notesList);
                    Console.WriteLine("Заметка удалена.");
                }
                else
                {
                    Console.WriteLine("Неверный номер заметки.");
                }
                break;
            case "4":
                return;
            default:
                Console.WriteLine("Неверный выбор.");
                break;
        }
    }

    static void PasswordGenerator()
    {
        Console.WriteLine("\n=== Генератор паролей ===");
        Console.WriteLine("1. Сгенерировать случайный пароль");
        Console.WriteLine("2. Сгенерировать PIN-код");
        Console.WriteLine("3. Назад");

        Console.Write("Выберите действие: ");
        string choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                Console.Write("Введите длину пароля (8-32): ");
                if (int.TryParse(Console.ReadLine(), out int length) && length >= 8 && length <= 32)
                {
                    const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()";
                    StringBuilder password = new StringBuilder();
                    using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
                    {
                        byte[] uintBuffer = new byte[sizeof(uint)];

                        for (int i = 0; i < length; i++)
                        {
                            rng.GetBytes(uintBuffer);
                            uint num = BitConverter.ToUInt32(uintBuffer, 0);
                            password.Append(validChars[(int)(num % (uint)validChars.Length)]);
                        }
                    }
                    Console.WriteLine($"Сгенерированный пароль: {password}");
                }
                else
                {
                    Console.WriteLine("Неверная длина пароля.");
                }
                break;
            case "2":
                Console.Write("Введите длину PIN-кода (4-8): ");
                if (int.TryParse(Console.ReadLine(), out int pinLength) && pinLength >= 4 && pinLength <= 8)
                {
                    Random rnd = new Random();
                    StringBuilder pin = new StringBuilder();
                    for (int i = 0; i < pinLength; i++)
                    {
                        pin.Append(rnd.Next(0, 9));
                    }
                    Console.WriteLine($"Сгенерированный PIN: {pin}");
                }
                else
                {
                    Console.WriteLine("Неверная длина PIN-кода.");
                }
                break;
            case "3":
                return;
            default:
                Console.WriteLine("Неверный выбор.");
                break;
        }
    }

    static void CurrencyConverter()
    {
        Console.WriteLine("\n=== Конвертер валют ===");
        Console.WriteLine("Доступные валюты: USD, EUR, RUB, GBP, JPY");

        // Курсы валют (условные)
        Dictionary<string, double> exchangeRates = new Dictionary<string, double>
        {
            {"USD", 1.0},
            {"EUR", 0.85},
            {"RUB", 75.0},
            {"GBP", 0.73},
            {"JPY", 110.0}
        };

        try
        {
            Console.Write("Введите исходную валюту: ");
            string fromCurrency = Console.ReadLine().ToUpper();

            if (!exchangeRates.ContainsKey(fromCurrency))
            {
                Console.WriteLine("Неизвестная валюта.");
                return;
            }

            Console.Write("Введите сумму: ");
            double amount = double.Parse(Console.ReadLine());

            Console.Write("Введите целевую валюту: ");
            string toCurrency = Console.ReadLine().ToUpper();

            if (!exchangeRates.ContainsKey(toCurrency))
            {
                Console.WriteLine("Неизвестная валюта.");
                return;
            }

            double convertedAmount = amount * (exchangeRates[toCurrency] / exchangeRates[fromCurrency]);
            Console.WriteLine($"{amount} {fromCurrency} = {convertedAmount:F2} {toCurrency}");
        }
        catch (FormatException)
        {
            Console.WriteLine("Ошибка: неверный формат числа!");
        }
    }

    static void Timer()
    {
        Console.WriteLine("\n=== Таймер ===");
        Console.WriteLine("1. Таймер обратного отсчета");
        Console.WriteLine("2. Секундомер");
        Console.WriteLine("3. Назад");

        Console.Write("Выберите действие: ");
        string choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                Console.Write("Введите время в секундах: ");
                if (int.TryParse(Console.ReadLine(), out int seconds) && seconds > 0)
                {
                    Console.WriteLine("Таймер запущен. Нажмите любую клавишу для отмены.");
                    DateTime endTime = DateTime.Now.AddSeconds(seconds);

                    while (DateTime.Now < endTime)
                    {
                        if (Console.KeyAvailable)
                        {
                            Console.ReadKey(true);
                            Console.WriteLine("Таймер отменен.");
                            return;
                        }

                        TimeSpan remaining = endTime - DateTime.Now;
                        Console.Write($"\rОсталось: {remaining:mm\\:ss} ");
                        System.Threading.Thread.Sleep(200);
                    }

                    Console.WriteLine("\nВремя вышло!");
                    Console.Beep();
                }
                else
                {
                    Console.WriteLine("Неверное время.");
                }
                break;
            case "2":
                Console.WriteLine("Секундомер запущен. Нажмите любую клавишу для остановки.");
                DateTime startTime = DateTime.Now;

                while (!Console.KeyAvailable)
                {
                    TimeSpan elapsed = DateTime.Now - startTime;
                    Console.Write($"\rПрошло: {elapsed:mm\\:ss\\.ff} ");
                    System.Threading.Thread.Sleep(50);
                }

                Console.ReadKey(true);
                Console.WriteLine("\nСекундомер остановлен.");
                break;
            case "3":
                return;
            default:
                Console.WriteLine("Неверный выбор.");
                break;
        }
    }
}