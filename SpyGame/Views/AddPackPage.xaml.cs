using SpyGame.Models;
using SpyGame.Services;

namespace SpyGame.Views
{
    public partial class AddPackPage : ContentPage
    {
        private WordPackService _wordPackService;
        private WordPack? _editingPack;
        private bool _isEditing;

        public AddPackPage()
        {
            InitializeComponent();
            _wordPackService = WordPackService.Instance;
            _isEditing = false;
        }

        public AddPackPage(WordPack pack)
        {
            InitializeComponent();
            _wordPackService = WordPackService.Instance;
            _editingPack = pack;
            _isEditing = true;
            LoadPackForEditing();
        }

        private void LoadPackForEditing()
        {
            PageTitleLabel.Text = "Edit Pack";
            PageSubtitleLabel.Text = "Modify your custom word pack";
            SaveButton.Text = "SAVE CHANGES";
            
            PackNameEntry.Text = _editingPack.Name;
            WordsEditor.Text = string.Join("\n", _editingPack.Words);
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
                if (_isEditing)
                {
                    await _wordPackService.UpdateCustomPack(_editingPack.Name, packName, words);
                    await DisplayAlert("Success", "Custom pack updated successfully!", "OK");
                }
                else
                {
                    var newPack = new WordPack(packName, words);
                    await _wordPackService.AddCustomPack(newPack);
                    await DisplayAlert("Success", "Custom pack added successfully!", "OK");
                }
                
                await Navigation.PopAsync();
            }
            catch (ArgumentException ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
            catch
            {
                await DisplayAlert("Error", _isEditing ? "Failed to update custom pack" : "Failed to save custom pack", "OK");
            }
        }
    }
}
