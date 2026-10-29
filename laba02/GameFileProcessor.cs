using System;
using System.IO;

namespace CatAndMouseGame
{
    public class GameFileProcessor
    {
        private const string Header =
            "Cat and Mouse\n\nCat   Mouse Distance\n-------------------\n";

        public bool Run(string inputFile, string outputFile)
        {
            File.WriteAllText(outputFile, Header);

            if (!File.Exists(inputFile))
            {
                WriteError(
                    outputFile,
                    $"Ошибка: Файл {inputFile} не найден!",
                    $"File {inputFile} not found!");

                return false;
            }

            string[] lines = File.ReadAllLines(inputFile);

            if (lines.Length == 0)
            {
                WriteError(
                    outputFile,
                    $"Ошибка: Файл {inputFile} пуст!",
                    "File is empty!");

                return false;
            }

            if (!int.TryParse(lines[0].Trim(), out int boardSize) || boardSize <= 0)
            {
                WriteError(
                    outputFile,
                    "Ошибка: Первая строка файла должна содержать размер поля!",
                    "Game failed — missing or invalid board size in the first line.");

                return false;
            }

            Game game = new Game(boardSize);

            for (int i = 1; i < lines.Length && !game.IsEnded; i++)
            {
                string[] parts = lines[i].Trim().Split(
                    new[] { ' ', '\t' },
                    StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length == 0)
                {
                    continue;
                }

                char command = char.ToUpperInvariant(parts[0][0]);

                if (command == 'P')
                {
                    File.AppendAllText(outputFile, game.GetPositionLog());
                }
                else if ((command == 'M' || command == 'C') &&
                         parts.Length > 1 &&
                         int.TryParse(parts[1], out int steps))
                {
                    game.MovePlayer(command, steps);
                }
            }

            game.Finish();

            if (!game.HasBothPlayersJoined)
            {
                WriteError(
                    outputFile,
                    "Ошибка: Игра невозможна! В файле отсутствует информация об одном из игроков.",
                    "Game failed — missing player position data.");

                return false;
            }

            File.AppendAllText(outputFile, game.GetFinalReport());
            return true;
        }

        private void WriteError(string outputFile, string consoleMessage, string logMessage)
        {
            Console.WriteLine("\n" + consoleMessage);
            File.AppendAllText(outputFile, $"\n[ERROR]: {logMessage}");
        }
    }
}
