using Puzzle.Pages;
using Puzzle.Repo;

namespace Puzzle.Pages
{
    public partial class MainPage : ContentPage
    {
        public MainPage ()
        {
            InitializeComponent ();
        }
        // check user input and find repo by name
        private async void LogClick ( object sender, EventArgs e )
        {
            string user = entry.Text?.Trim ();
            if ( user?.Length > 1 )
                if ( user.All ( c => Char.IsLetterOrDigit ( c ) ) )
                {
                    Settings.UserEntered ( user );
                    await Shell.Current.Navigation.PushAsync ( new FirstPage () );
                    return;
                }
            await DisplayAlert ( "Name", "Name must be 2 or more letters and contans only letters and digits","Close" );
        }
    }

}
