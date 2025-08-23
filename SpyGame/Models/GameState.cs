namespace SpyGame.Models
{
    public class GameState
    {
        public WordPack SelectedPack { get; set; } = new WordPack();
        public int PlayerCount { get; set; } = 0;
        public int SpyCount { get; set; } = 1;
        public string CurrentWord { get; set; } = string.Empty;
        public List<int> SpyPlayerIndices { get; set; } = new List<int>();
        public int CurrentPlayerIndex { get; set; } = 0;
        public bool IsGameActive { get; set; } = false;
        public DateTime GameStartTime { get; set; } = DateTime.MinValue;
        public bool AllPlayersHaveSeenCard { get; set; } = false;

        public void StartNewGame()
        {
            if (SelectedPack.Words.Count == 0 || PlayerCount <= 0)
                return;

            var random = new Random();
            CurrentWord = SelectedPack.Words[random.Next(SelectedPack.Words.Count)];
            
            SpyPlayerIndices.Clear();
            var availablePlayers = Enumerable.Range(0, PlayerCount).ToList();
            
            for (int i = 0; i < SpyCount && availablePlayers.Count > 0; i++)
            {
                int randomIndex = random.Next(availablePlayers.Count);
                SpyPlayerIndices.Add(availablePlayers[randomIndex]);
                availablePlayers.RemoveAt(randomIndex);
            }
            
            CurrentPlayerIndex = 0;
            IsGameActive = true;
            GameStartTime = DateTime.Now;
            AllPlayersHaveSeenCard = false;
        }

        public void NextPlayer()
        {
            CurrentPlayerIndex = (CurrentPlayerIndex + 1) % PlayerCount;
            
            // Check if all players have seen their cards
            if (CurrentPlayerIndex == 0)
            {
                AllPlayersHaveSeenCard = true;
            }
        }

        public bool IsCurrentPlayerSpy()
        {
            return SpyPlayerIndices.Contains(CurrentPlayerIndex);
        }

        public TimeSpan GetGameElapsedTime()
        {
            if (GameStartTime == DateTime.MinValue)
                return TimeSpan.Zero;
            
            return DateTime.Now - GameStartTime;
        }

        public void ResetGame()
        {
            CurrentWord = string.Empty;
            SpyPlayerIndices.Clear();
            CurrentPlayerIndex = 0;
            IsGameActive = false;
            GameStartTime = DateTime.MinValue;
            AllPlayersHaveSeenCard = false;
        }
    }
}
