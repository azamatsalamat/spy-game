using SpyGame.Models;
using SpyGame.Services;

namespace SpyGame.Views
{
    public partial class AddPackPage : ContentPage
    {
        private WordPackService _wordPackService;

        public AddPackPage()
        {
            InitializeComponent();
            _wordPackService = WordPackService.Instance;
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
                
                await DisplayAlert("Success", "Custom pack added successfully!", "OK");
                
                await Navigation.PopAsync();
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
    }
}
