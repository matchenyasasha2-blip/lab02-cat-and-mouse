using System;

namespace CatAndMouseGame
{
    internal class Program
    {
        private static int Main(string[] args)
        {
            string? choice;
            if (args.Length == 0)
            {
                Console.Write("Введите номер теста (1, 2, 3 или 4): ");
                choice = Console.ReadLine()?.Trim();
            }
            else if (args.Length == 1)
            {
                choice = args[0];
            }
            else
            {
                Console.Error.WriteLine("Использование: laba02 [номер теста]");
                return 1;
            }

            if (choice != "1" && choice != "2" && choice != "3" && choice != "4")
            {
                Console.Error.WriteLine("Ошибка: Допустимые номера тестов: 1, 2, 3 или 4.");
                return 1;
            }

            GameFileProcessor processor = new GameFileProcessor();
            if (!processor.Run($"{choice}.ChaseData.txt", $"{choice}.PursuitLog.txt"))
            {
                Console.Error.WriteLine($"Ошибка: {processor.ErrorMessage}");
                return 1;
            }

            return 0;
        }
    }
}
