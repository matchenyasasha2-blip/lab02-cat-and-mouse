using System;

namespace CatAndMouseGame
{
    public enum GameState
    {
        Start,
        End
    }

    public class Game
    {
        public int BoardSize { get; }
        public Player Cat { get; }
        public Player Mouse { get; }
        public GameState State { get; private set; }

        public bool IsEnded => State == GameState.End;

        public bool HasBothPlayersJoined => Cat.IsInGame && Mouse.IsInGame;

        public Game(int boardSize)
        {
            if (boardSize <= 0)
            {
                throw new ArgumentException("Размер поля должен быть положительным.");
            }

            BoardSize = boardSize;
            Cat = new Player("Cat");
            Mouse = new Player("Mouse");
            State = GameState.Start;
        }

        public void MovePlayer(char command, int steps)
        {
            if (IsEnded)
            {
                return;
            }

            switch (char.ToUpperInvariant(command))
            {
                case 'C':
                    Cat.Move(steps, BoardSize);
                    break;

                case 'M':
                    Mouse.Move(steps, BoardSize);
                    break;

                default:
                    return;
            }

            CheckForCapture();
        }

        private void CheckForCapture()
        {
            if (Cat.State == PlayerState.Playing &&
                Mouse.State == PlayerState.Playing &&
                Cat.Location == Mouse.Location)
            {
                Cat.MarkAsWinner();
                Mouse.MarkAsLoser();
                State = GameState.End;
            }
        }

        public string GetPositionLog()
        {
            string catPosition = Cat.IsInGame ? Cat.Location.ToString() : "??";
            string mousePosition = Mouse.IsInGame ? Mouse.Location.ToString() : "??";

            string distance = HasBothPlayersJoined
                ? Math.Abs(Cat.Location - Mouse.Location).ToString()
                : "??";

            return $" {catPosition,-5} {mousePosition,-5} {distance}\n";
        }

        public void Finish()
        {
            State = GameState.End;
        }

        public string GetFinalReport()
        {
            string result = Mouse.State == PlayerState.Loser
                ? $"Mouse caught at: {Cat.Location}\n"
                : "Mouse evaded Cat\n";

            return
                "-------------------\n\n\n" +
                "Distance traveled: Mouse Cat\n" +
                $" {Mouse.DistanceTraveled,-14} {Cat.DistanceTraveled}\n\n" +
                result;
        }
    }
}
