using SpyGame.Models;
using SpyGame.Services;

namespace SpyGame.Views
{
    public partial class MainMenuPage : ContentPage
    {
        private WordPackService _wordPackService;
        private List<WordPack> _wordPacks;
        private WordPack? _selectedPack;
        private int _playerCount = 4;
        private int _spyCount = 1;

        public MainMenuPage()
        {
            InitializeComponent();
            _wordPackService = new WordPackService();
            _wordPacks = _wordPackService.GetAllPacks();
            
            InitializeWordPackPicker();
            UpdatePlayerCountDisplay();
            UpdateSpyCountDisplay();
        }

        private void InitializeWordPackPicker()
        {
            var packNames = _wordPacks.Select(pack => pack.Name).ToList();
            WordPackPicker.ItemsSource = packNames;
            
            if (packNames.Count > 0)
            {
                WordPackPicker.SelectedIndex = 0;
                _selectedPack = _wordPacks[0];
            }
        }

        private void OnWordPackSelected(object sender, EventArgs e)
        {
            if (WordPackPicker.SelectedIndex >= 0 && WordPackPicker.SelectedIndex < _wordPacks.Count)
            {
                _selectedPack = _wordPacks[WordPackPicker.SelectedIndex];
            }
        }

        private void OnDecreasePlayers(object sender, EventArgs e)
        {
            if (_playerCount > 3)
            {
                _playerCount--;
                UpdatePlayerCountDisplay();
            }
        }

        private void OnIncreasePlayers(object sender, EventArgs e)
        {
            if (_playerCount < 20)
            {
                _playerCount++;
                UpdatePlayerCountDisplay();
            }
        }

        private void UpdatePlayerCountDisplay()
        {
            PlayerCountLabel.Text = _playerCount.ToString();
            UpdateSpyCountDisplay();
        }

        private void OnDecreaseSpies(object sender, EventArgs e)
        {
            if (_spyCount > 1)
            {
                _spyCount--;
                UpdateSpyCountDisplay();
            }
        }

        private void OnIncreaseSpies(object sender, EventArgs e)
        {
            if (_spyCount < _playerCount)
            {
                _spyCount++;
                UpdateSpyCountDisplay();
            }
        }

        private void UpdateSpyCountDisplay()
        {
            SpyCountLabel.Text = _spyCount.ToString();
        }

        private async void OnStartGame(object sender, EventArgs e)
        {
            if (_selectedPack == null)
            {
                await DisplayAlert("Error", "Please select a word pack", "OK");
                return;
            }

            if (_playerCount < 2)
            {
                await DisplayAlert("Error", "You need at least 2 players", "OK");
                return;
            }

            if (_spyCount >= _playerCount)
            {
                await DisplayAlert("Error", "Number of spies must be less than number of players", "OK");
                return;
            }

            var gameState = new GameState
            {
                SelectedPack = _selectedPack,
                PlayerCount = _playerCount,
                SpyCount = _spyCount
            };

            // Simple page replacement to avoid NavigationPage issues
            Application.Current!.MainPage = new GamePage(gameState);
        }
    }
}
