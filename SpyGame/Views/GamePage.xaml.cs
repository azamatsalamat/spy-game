using SpyGame.Models;
using SpyGame.Services;

namespace SpyGame.Views
{
    public partial class GamePage : ContentPage
    {
        private GameState _gameState;
        private bool _cardRevealed = false;
        private IDispatcherTimer? _timer;
        private bool _gameStarted = false;

        public GamePage(GameState gameState)
        {
            InitializeComponent();
            _gameState = gameState;
            InitializeGame();
        }

        private void InitializeGame()
        {
            _gameState.StartNewGame();
            UpdatePlayerDisplay();
            SetupTimer();
        }

        private void SetupTimer()
        {
            _timer = Application.Current?.Dispatcher.CreateTimer();
            if (_timer != null)
            {
                _timer.Interval = TimeSpan.FromSeconds(1);
                _timer.Tick += OnTimerTick;
                _timer.Start();
            }
        }

        private void OnTimerTick(object? sender, EventArgs e)
        {
            if (_gameState.IsGameActive)
            {
                var elapsed = _gameState.GetGameElapsedTime();
                var timeString = $"{elapsed.Minutes:D2}:{elapsed.Seconds:D2}";
                
                if (_gameState.AllPlayersHaveSeenCard)
                {
                    FinalTimerLabel.Text = $"Game Time: {timeString}";
                    GameTimerLabel.Text = $"Time: {timeString}";
                    GameTimerLabel.IsVisible = true;
                }
            }
        }

        private void UpdatePlayerDisplay()
        {
            CurrentPlayerLabel.Text = $"Player {_gameState.CurrentPlayerIndex + 1}";
        }

        private void OnCardTapped(object sender, EventArgs e)
        {
            if (!_gameStarted)
            {
                _gameStarted = true;
            }

            if (!_cardRevealed)
            {
                RevealCard();
            }
            else
            {
                HideCard();
            }
        }

        private void RevealCard()
        {
            _cardRevealed = true;
            
            if (_gameState.IsCurrentPlayerSpy())
            {
                CardContentLabel.Text = "YOU ARE\nTHE SPY!";
                CardContentLabel.TextColor = Colors.Red;
                CardFooterLabel.Text = "Don't let them find you!";
            }
            else
            {
                CardContentLabel.Text = _gameState.CurrentWord;
                CardContentLabel.TextColor = Colors.White;
                CardFooterLabel.Text = "Remember this word!";
            }

            CardContentLabel.IsVisible = true;
            CardFooterLabel.IsVisible = true;
            CardHeaderLabel.Text = "Tap to hide card";
            InstructionsLabel.Text = "Tap the card to hide it, then pass to next player";
        }

        private void HideCard()
        {
            _cardRevealed = false;
            
            CardContentLabel.IsVisible = false;
            CardFooterLabel.IsVisible = false;
            CardHeaderLabel.Text = "Tap to reveal your card";
            InstructionsLabel.Text = "Pass the phone to the next player and tap the card to reveal";

            // Move to next player
            _gameState.NextPlayer();
            UpdatePlayerDisplay();

            // Check if all players have seen their cards
            if (_gameState.AllPlayersHaveSeenCard)
            {
                ShowGameEndSection();
            }
        }

        private void ShowGameEndSection()
        {
            GameEndSection.IsVisible = true;
            CardFrame.IsVisible = false;
            InstructionsLabel.IsVisible = false;
            GameActiveLabel.IsVisible = true;
            CurrentPlayerLabel.Text = "Game Active - Discuss and find the spy!";
        }

        private async void OnEndGame(object sender, EventArgs e)
        {
            var result = await DisplayAlert("End Game", 
                "Are you sure you want to end the game?", 
                "Yes", "No");
            
            if (result)
            {
                _timer?.Stop();
                
                // Simple page replacement back to main menu
                NavigationService.NavigateToMainMenu();
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            _timer?.Stop();
        }
    }
}
