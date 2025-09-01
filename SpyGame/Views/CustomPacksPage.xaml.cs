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
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _wordPackService.RefreshCustomPacks();
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

        private void OnAddNewPack(object sender, EventArgs e)
        {
            NavigationService.NavigateToAddPack();
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

        private void OnPackTapped(object sender, TappedEventArgs e)
        {
            if (e.Parameter is WordPack pack)
            {
                NavigationService.NavigateToEditPack(pack);
            }
        }

    }
}
