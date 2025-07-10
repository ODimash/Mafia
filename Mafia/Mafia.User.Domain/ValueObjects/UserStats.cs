namespace Mafia.User.Domain.ValueObjects
{
    public class UserStats
    {
        public int GamesPlayed { get; private set; }
        public int GamesWon { get; private set; }
        public int GamesLost { get; private set; }
        public double Rating { get; private set; }

        public UserStats(int gamesPlayed = 0, int gamesWon = 0, int gamesLost = 0)
        {
            GamesPlayed = gamesPlayed;
            GamesWon = gamesWon;
            GamesLost = gamesLost;
            UpdateRating();
        }

        public void RegisterGame(bool isWin)
        {
            GamesPlayed++;
            if (isWin)
                GamesWon++;
            else
                GamesLost++;
            UpdateRating();
        }

        private void UpdateRating()
        {
            if (GamesPlayed == 0)
                Rating = 0.0;
            else
                Rating = (double)GamesWon / GamesPlayed * 100.0;
        }
    }
}
