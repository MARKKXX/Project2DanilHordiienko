namespace Puzzle.Pages;

public partial class FirstPage : ContentPage
{
	public FirstPage()
	{
		InitializeComponent();
	}
    //each appearing update info about user
    protected override void OnAppearing ()
    {
        base.OnAppearing ();
        user.Text = Settings.CurrentUser.Name;
        party.Text = $" ( {Settings.CurrentUser.PlayList.Count} played )";
        Application.Current.UserAppTheme = Settings.CurrentUser.ColorTheme;
    }
    private void PlayClick ( object sender, EventArgs e )
    {
        Shell.Current.Navigation.PushAsync ( new PlayPage () );
    }
    private void ThemeClick ( object sender, EventArgs e )
    {
        Shell.Current.Navigation.PushAsync ( new ThemePage () );
    }

    private void UserClick ( object sender, EventArgs e )
    {
        Shell.Current.Navigation.PushAsync ( new UserPage () );
    }
    private void CloseClick ( object sender, EventArgs e )
    {
        App.Current.Quit ();
    }
}