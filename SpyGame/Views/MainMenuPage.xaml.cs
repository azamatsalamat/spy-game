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

        public MainMenuPage()
        {
            InitializeComponent();
            _wordPackService = new WordPackService();
            _wordPacks = _wordPackService.GetAllPacks();
            
            InitializeWordPackPicker();
            UpdatePlayerCountDisplay();
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

            var gameState = new GameState
            {
                SelectedPack = _selectedPack,
                PlayerCount = _playerCount
            };

            // Simple page replacement to avoid NavigationPage issues
            Application.Current!.MainPage = new GamePage(gameState);
        }
    }
}
