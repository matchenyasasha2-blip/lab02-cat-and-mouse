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
            if (boardSize <= 0 || boardSize > 10000)
            {
                throw new ArgumentException("Размер поля должен быть от 1 до 10000.");
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

            return $"{catPosition,5} {mousePosition,7} {distance,10}\n";
        }

        public void Finish()
        {
            if (IsEnded) return;

            if (Cat.State == PlayerState.Playing) Cat.MarkAsLoser();
            if (Mouse.State == PlayerState.Playing) Mouse.MarkAsWinner();
            State = GameState.End;
        }

        public string GetFinalReport()
        {
            string result = Mouse.State == PlayerState.Loser
                ? $"Mouse caught at: {Cat.Location}\n"
                : "Mouse evaded Cat\n";

            return
                "------------------------\n\n\n" +
                $"Distance traveled: {"Mouse",12} {"Cat",12}\n" +
                $"{string.Empty,18} {Mouse.DistanceTraveled,12} {Cat.DistanceTraveled,12}\n\n" +
                result;
        }
    }
}
