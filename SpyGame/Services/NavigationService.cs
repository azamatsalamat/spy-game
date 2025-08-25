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
                    if (Application.Current.MainPage is NavigationPage navigationPage)
                    {
                        navigationPage.PushAsync(page);
                    }
                    else
                    {
                        Application.Current.MainPage = new NavigationPage(page);
                    }
                }
                catch
                {
                    Application.Current.MainPage = new NavigationPage(page);
                }
            }
        }

        public static void NavigateToMainMenu()
        {
            if (Application.Current?.MainPage != null)
            {
                if (Application.Current.MainPage is NavigationPage navigationPage)
                {
                    navigationPage.PopToRootAsync();
                }
                else
                {
                    Application.Current.MainPage = new NavigationPage(new MainMenuPage());
                }
            }
        }

        public static void NavigateToCustomPacks()
        {
            NavigateTo(new CustomPacksPage());
        }

        public static void NavigateToAddPack()
        {
            NavigateTo(new AddPackPage());
        }

        public static void NavigateToGame(GameState gameState)
        {
            NavigateTo(new GamePage(gameState));
        }
    }
}
