namespace Puzzle.Pages;

public partial class ThemePage : ContentPage
{
    public ThemePage ()
    {
        InitializeComponent ();
    }
    protected override void OnAppearing ()
    {
        base.OnAppearing ();
        RadioButtonGroup.SetSelectedValue ( group, Settings.CurrentUser.ColorTheme );
    }

    private void RadioClick ( object sender, CheckedChangedEventArgs e )
    {
        object value = RadioButtonGroup.GetSelectedValue ( group );
        Application.Current.UserAppTheme = ( AppTheme ) value;
    }
    private void SaveClick ( object sender, EventArgs e )
    {
        object value = RadioButtonGroup.GetSelectedValue ( group );
        Settings.CurrentUser.ColorTheme = ( AppTheme ) value;
        Settings.SaveUser ();
        Shell.Current.Navigation.PopAsync ();
    }
    private void CloseClick ( object sender, EventArgs e )
    {
        Settings.CurrentUser.ColorTheme = Settings.CurrentUser.ColorTheme;
        Shell.Current.Navigation.PopAsync ();
    }


}