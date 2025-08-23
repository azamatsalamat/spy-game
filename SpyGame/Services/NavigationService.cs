using SpyGame.Views;
using SpyGame.Models;

namespace SpyGame.Services
{
    public static class NavigationService
    {
        public static void NavigateTo(Page page)
        {
            if (Application.Current?.MainPage != null)
            {
                try
                {
                    var oldPage = Application.Current.MainPage;
                    Application.Current.MainPage = page;
                    
                    // Dispose of the old page to prevent memory leaks
                    if (oldPage is IDisposable disposable)
                    {
                        disposable.Dispose();
                    }
                }
                catch
                {
                    // Fallback for Android navigation issues
                    Application.Current.MainPage = page;
                }
            }
        }

        public static void NavigateToMainMenu()
        {
            NavigateTo(new MainMenuPage());
        }

        public static void NavigateToCustomPacks()
        {
            NavigateTo(new CustomPacksPage());
        }

        public static void NavigateToGame(GameState gameState)
        {
            NavigateTo(new GamePage(gameState));
        }
    }
}
