namespace SpyGame;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		// Use simple ContentPage to avoid NavigationPage issues
		MainPage = new Views.MainMenuPage();
	}
}
