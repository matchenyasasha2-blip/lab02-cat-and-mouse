using System;

namespace CatAndMouseGame
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.Write("Введите номер теста (1, 2, 3 или 4): ");
            string choice = Console.ReadLine()?.Trim();

            if (choice != "1" && choice != "2" && choice != "3" && choice != "4")
            {
                Console.WriteLine(
                    "\nОшибка: Введён неверный номер теста! " +
                    "Допустимые значения: 1, 2, 3 или 4.");

                return;
            }

            string inputFile = $"{choice}.ChaseData.txt";
            string outputFile = $"{choice}.PursuitLog.txt";

            GameFileProcessor processor = new GameFileProcessor();

            if (processor.Run(inputFile, outputFile))
            {
                Console.WriteLine(
                    $"Обработка завершена. Результаты сохранены в {outputFile}");
            }
        }
    }
}
