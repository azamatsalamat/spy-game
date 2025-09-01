using System.Text.Json;
using SpyGame.Models;
using SpyGame.Services;

namespace SpyGame.Views
{
    public partial class MainMenuPage : ContentPage
    {
        private WordPackService _wordPackService;
        private List<WordPack>? _wordPacks;
        private WordPack? _selectedPack;
        private int _playerCount = 4;
        private int _spyCount = 1;
        private readonly string _lastSelectedFileName = "last_selected.json";

        public MainMenuPage()
        {
            InitializeComponent();
            _wordPackService = WordPackService.Instance;
            LoadWordPacks();
            
            _ = InitializeWordPackPicker();
            UpdatePlayerCountDisplay();
            UpdateSpyCountDisplay();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _wordPackService.RefreshCustomPacks();
            LoadWordPacks();
            await InitializeWordPackPicker();
        }

        private void LoadWordPacks()
        {
            _wordPacks = _wordPackService.GetAllPacks();
        }

        private async Task InitializeWordPackPicker()
        {
            if (_wordPacks == null) return;
            
            var currentSelection = WordPackPicker.SelectedItem as string;
            var packNames = _wordPacks.Select(pack => pack.IsCustom ? $"{pack.Name} (Custom)" : pack.Name).ToList();
            WordPackPicker.ItemsSource = packNames;
            
            if (packNames.Count > 0)
            {
                if (!string.IsNullOrEmpty(currentSelection))
                {
                    var matchingPack = _wordPacks.FirstOrDefault(p => 
                        (p.IsCustom ? $"{p.Name} (Custom)" : p.Name) == currentSelection);
                    if (matchingPack != null)
                    {
                        WordPackPicker.SelectedItem = currentSelection;
                        _selectedPack = matchingPack;
                    }
                    else
                    {
                        await LoadSelectedPack();
                    }
                }
                else
                {
                    await LoadSelectedPack();
                }
            }
        }



        private void OnWordPackSelected(object sender, EventArgs e)
        {
            if (_wordPacks != null && WordPackPicker.SelectedItem is string selectedItem)
            {
                var selectedPackName = selectedItem.Replace(" (Custom)", "");
                _selectedPack = _wordPacks.FirstOrDefault(p => p.Name == selectedPackName);
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

            await SaveSelectedPack();
            NavigationService.NavigateToGame(gameState);
        }

        private void OnCustomPacks(object sender, EventArgs e)
        {
            NavigationService.NavigateToCustomPacks();
        }

        private async Task LoadSelectedPack()
        {
            try
            {
                var filePath = Path.Combine(FileSystem.AppDataDirectory, _lastSelectedFileName);
                if (File.Exists(filePath))
                {
                    var json = await File.ReadAllTextAsync(filePath);
                    _selectedPack = JsonSerializer.Deserialize<WordPack>(json) ?? _wordPacks[0];
                } else 
                {
                    _selectedPack = _wordPacks[0];
                }
            }
            catch 
            {
                _selectedPack = _wordPacks[0];
            }

            WordPackPicker.SelectedIndex = _wordPacks.FindIndex(p => p.Name == _selectedPack?.Name);
        }

        private async Task SaveSelectedPack()
        {
            try
            {
                var filePath = Path.Combine(FileSystem.AppDataDirectory, _lastSelectedFileName);
                var json = JsonSerializer.Serialize(_selectedPack, new JsonSerializerOptions { WriteIndented = true });
                await File.WriteAllTextAsync(filePath, json);
            }
            catch
            {
                // Handle save error silently for now
            }
        }
    }
}
