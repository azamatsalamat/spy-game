using SpyGame.Models;
using SpyGame.Services;
using System.Collections.ObjectModel;

namespace SpyGame.Views
{
    public partial class CustomPacksPage : ContentPage
    {
        private WordPackService _wordPackService;
        public ObservableCollection<WordPack> CustomPacks { get; set; }

        public CustomPacksPage()
        {
            InitializeComponent();
            _wordPackService = WordPackService.Instance;
            CustomPacks = new ObservableCollection<WordPack>();
            
            BindingContext = this;
            LoadCustomPacks();
        }

        private void LoadCustomPacks()
        {
            CustomPacks.Clear();
            var customPacks = _wordPackService.GetCustomPacks();
            
            foreach (var pack in customPacks)
            {
                CustomPacks.Add(pack);
            }
            
            UpdateNoPacksVisibility();
        }

        private void UpdateNoPacksVisibility()
        {
            NoPacksLabel.IsVisible = CustomPacks.Count == 0;
            CustomPacksCollection.IsVisible = CustomPacks.Count > 0;
        }

        private async void OnAddPack(object sender, EventArgs e)
        {
            var packName = PackNameEntry.Text?.Trim();
            var wordsText = WordsEditor.Text?.Trim();

            if (string.IsNullOrWhiteSpace(packName))
            {
                await DisplayAlert("Error", "Please enter a pack name", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(wordsText))
            {
                await DisplayAlert("Error", "Please enter some words", "OK");
                return;
            }

            var words = wordsText.Split('\n', StringSplitOptions.RemoveEmptyEntries)
                                .Select(w => w.Trim())
                                .Where(w => !string.IsNullOrWhiteSpace(w))
                                .ToList();

            if (words.Count < 2)
            {
                await DisplayAlert("Error", "Pack must contain at least 2 words", "OK");
                return;
            }

            try
            {
                var newPack = new WordPack(packName, words);
                await _wordPackService.AddCustomPack(newPack);
                
                CustomPacks.Add(newPack);
                UpdateNoPacksVisibility();
                
                PackNameEntry.Text = string.Empty;
                WordsEditor.Text = string.Empty;
                
                await DisplayAlert("Success", "Custom pack added successfully!", "OK");
            }
            catch (ArgumentException ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
            catch
            {
                await DisplayAlert("Error", "Failed to save custom pack", "OK");
            }
        }

        private async void OnDeletePack(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is string packName)
            {
                var result = await DisplayAlert("Confirm Delete", 
                    $"Are you sure you want to delete '{packName}'?", "Delete", "Cancel");
                
                if (result)
                {
                    try
                    {
                        await _wordPackService.DeleteCustomPack(packName);
                        
                        var packToRemove = CustomPacks.FirstOrDefault(p => p.Name == packName);
                        if (packToRemove != null)
                        {
                            CustomPacks.Remove(packToRemove);
                            UpdateNoPacksVisibility();
                        }
                        
                        await DisplayAlert("Success", "Pack deleted successfully!", "OK");
                    }
                    catch
                    {
                        await DisplayAlert("Error", "Failed to delete pack", "OK");
                    }
                }
            }
        }

        private void OnBackToMenu(object sender, EventArgs e)
        {
            NavigationService.NavigateToMainMenu();
        }
    }
}
