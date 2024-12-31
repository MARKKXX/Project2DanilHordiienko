namespace Puzzle.Pages;

public partial class UserPage : ContentPage
{
    List<GameInfo> _items;
    public List<GameInfo> Items { get { return _items; } set { _items = value; OnPropertyChanged (); } }

    public UserPage ()
    {
        InitializeComponent ();
        Items = Settings.CurrentUser.PlayList;

        BindingContext = this;
    }
    protected override void OnAppearing ()
    {
        base.OnAppearing ();
        username.Text = Settings.CurrentUser.Name;
        RadioButtonGroup.SetSelectedValue ( group, Settings.CurrentUser.Level );
    }

    private void SaveClick ( object sender, EventArgs e )
    {
        Settings.CurrentUser.Level = RadioButtonGroup.GetSelectedValue ( group ).ToString ();
        Settings.SaveUser ();
        Shell.Current.Navigation.PopAsync ();
    }

    private void CloseClick ( object sender, EventArgs e )
    {
        Shell.Current.Navigation.PopAsync ();
    }

    private async void ClearClick ( object sender, EventArgs e )
    {
        string result = await DisplayActionSheet ( "Remove history?", null, null, "Yes", "No" );
        if ( result == "Yes" )
        {
            Settings.CurrentUser.PlayList.Clear ();
            Settings.SaveUser ();
            Items = null;
        }
    }
}