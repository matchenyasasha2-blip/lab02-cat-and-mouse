using System;
using System.IO;
using System.Text;

namespace CatAndMouseGame
{
    public class GameFileProcessor
    {
        private const string Header =
            "Cat and Mouse\n\n  Cat   Mouse   Distance\n------------------------\n";

        public string? ErrorMessage { get; private set; }

        public bool Run(string inputFile, string outputFile)
        {
            ErrorMessage = null;

            try
            {
                StringBuilder report = new StringBuilder(Header);
                Game game;

                using (StreamReader input = new StreamReader(inputFile))
                {
                    if (!int.TryParse(input.ReadLine(), out int boardSize) ||
                        boardSize < 1 || boardSize > 10000)
                    {
                        throw new FormatException("Размер поля должен быть от 1 до 10000.");
                    }

                    game = new Game(boardSize);
                    string? line;

                    while (!game.IsEnded && (line = input.ReadLine()) != null)
                    {
                        string[] parts = line.Split(
                            new[] { ' ', '\t' },
                            StringSplitOptions.RemoveEmptyEntries);

                        if (parts.Length == 0) continue;

                        string command = parts[0].ToUpperInvariant();
                        if (command == "P" && parts.Length == 1)
                        {
                            report.Append(game.GetPositionLog());
                        }
                        else if ((command == "M" || command == "C") &&
                                 parts.Length == 2 &&
                                 int.TryParse(parts[1], out int steps))
                        {
                            game.MovePlayer(command[0], steps);
                        }
                        else
                        {
                            throw new FormatException($"Некорректная команда: {line}");
                        }
                    }
                }

                game.Finish();
                report.Append(game.GetFinalReport());
                File.WriteAllText(outputFile, report.ToString());
                return true;
            }
            catch (Exception error) when (error is IOException ||
                                          error is UnauthorizedAccessException ||
                                          error is FormatException ||
                                          error is ArgumentException)
            {
                ErrorMessage = error.Message;
                return false;
            }
        }
    }
}
