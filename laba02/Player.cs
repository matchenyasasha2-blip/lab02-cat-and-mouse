using System;

namespace CatAndMouseGame
{
    public enum PlayerState
    {
        Winner,
        Loser,
        Playing,
        NotInGame
    }

    public class Player
    {
        public string Name { get; }
        public int Location { get; private set; }
        public PlayerState State { get; private set; }
        public long DistanceTraveled { get; private set; }

        public bool IsInGame => State != PlayerState.NotInGame;

        public Player(string name)
        {
            Name = name;
            Location = -1;
            State = PlayerState.NotInGame;
            DistanceTraveled = 0;
        }

        public void Move(int steps, int boardSize)
        {
            if (boardSize < 1 || boardSize > 10000)
                throw new ArgumentOutOfRangeException(nameof(boardSize), "Размер поля должен быть от 1 до 10000.");

            if (State == PlayerState.NotInGame)
            {
                if (steps < 1 || steps > boardSize)
                    throw new ArgumentOutOfRangeException(nameof(steps), "Начальная позиция должна находиться на поле.");

                Location = steps;
                State = PlayerState.Playing;
            }
            else if (State == PlayerState.Playing && steps != 0)
            {
                DistanceTraveled += Math.Abs((long)steps);
                Location = NormalizePosition((long)Location + steps, boardSize);
            }
        }

        public void MarkAsWinner()
        {
            State = PlayerState.Winner;
        }

        public void MarkAsLoser()
        {
            State = PlayerState.Loser;
        }

        private static int NormalizePosition(long position, int boardSize)
        {
            return (int)(((position - 1) % boardSize + boardSize) % boardSize + 1);
        }
    }
}
