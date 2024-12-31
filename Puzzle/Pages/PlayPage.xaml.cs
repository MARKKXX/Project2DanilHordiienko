using System.Timers;

namespace Puzzle.Pages;

public partial class PlayPage : ContentPage
{
    int WID = 5, HIG = 6;
    Label [,] Items;    // letters in Grid x:Name=root
    List<string> Words; // current added word to table

    private string _info; // info message about user input
    public string Info { get { return _info; } set { _info = value; OnPropertyChanged (); } }

    private string _wordEntry; // user entry
    public string WordEntry { get => _wordEntry; set { _wordEntry = value; OnPropertyChanged (); Info = ""; } }

    private string _timeInfo; // timer
    public string TimeInfo { get { return _timeInfo; } set { _timeInfo = value; OnPropertyChanged (); } }

    string _winword; // Win word. Select randomly at start
    public string WinWord { get => _winword; set { _winword = value; OnPropertyChanged (); } }

    System.Timers.Timer timer;
    GameInfo game; // current GameInfo. Created at start
    LevelInfo level; // current Level. Get from Settings.CurrentUser.Level and LevelInfo.LevelList

    public PlayPage ()
    {
        InitializeComponent ();

        // init cells for letters in root
        Items = new Label [ HIG, WID ];
        for ( int y = 0; y < HIG; y++ )
            for ( int x = 0; x < WID; x++ )
            {
                Label item = new () { Text = $"a", FontSize = 32, TextColor = Colors.Black, FontFamily = "Tahoma", HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment = TextAlignment.Center };
                Border border = new () { Content = item, WidthRequest = 48, HeightRequest = 48 };
                Grid.SetRow ( border, y );
                Grid.SetColumn ( border, x );
                root.Children.Add ( border );
                Items [ y, x ] = item;
            }
        Start ();
    }
    void Start ()
    {
        // make visibility or not element depends on current game state
        // at start:
        winLabel.IsVisible = loseLabel.IsVisible = overBox.IsVisible = false;
        entryBox.IsVisible = true;

        // clear words list
        Words = [];
        // clear all letters in table 
        foreach ( var item in Items )
        {
            item.Text = "";
            item.Background = Colors.Transparent;
            item.FontAttributes = FontAttributes.None;
        }
        // select randomly win word
        WinWord = Settings.Words [ Settings.Rnd.Next ( Settings.Words.Count ) ];

        // hint for debug mode: uncomment Label x:Name=win for hint visible
        //win.Text = WinWord; // hint

        // create current GameInfo        
        game = new GameInfo ( WinWord );
        // store level
        game.Level = Settings.CurrentUser.Level;
        // get LevelInfo object depends on game.Level
        level = LevelInfo.LevelList.FirstOrDefault ( l => l.Name == game.Level );
        StartTimer ();
    }

    void StartTimer ()
    {
        if ( level.Time > 0 )
        {
            // start timer
            timer = new System.Timers.Timer ( 1000 );
            // TimeSpan of time limit
            TimeSpan end = TimeSpan.FromMinutes ( level.Time );

            // handler timer event
            timer.Elapsed += delegate
            {
                // how many time spent
                TimeSpan interval = DateTime.Now - game.Begin;
                
                if ( interval >= end )
                {
                    // time is up: 
                    Dispatcher.Dispatch ( () =>
                    {
                        TimeInfo = "Timeout";
                        GameOver ( false, true );
                    } );
                    timer.Dispose ();
                }
                // display time
                TimeInfo = ( end - interval ).ToString ( "mm\\:ss" );
            };
            timer.Start ();
        }
        else
            TimeInfo = null;
    } 

    // click button OK
    private async void OkClick ( object sender, EventArgs e )
    {
        string word = WordEntry.Trim ().ToLower ();
        if ( word == "" )
            return;

        // word analysis
        if ( word.Length == WID )
        {
            if ( Words.Contains ( word ) )
                Info = "word already exists";
            else
                if ( !Settings.Words.Contains ( word ) )
                Info = "no such word";
            else
            {
                // acceptable word
                int y = Words.Count;
                // display letters and set color depends of state
                for ( int x = 0; x < WID; x++ )
                {
                    Label label = Items [ y, x ];
                    label.Opacity = 0; // for animation
                    await Task.Delay ( 100 ); // delay

                    // user letter at position x
                    char curr = word [ x ];
                    // winword letter at position x
                    char win = WinWord [ x ];
                    // print user letter on table
                    label.Text = curr.ToString ();
                    // if match - green
                    if ( curr == win )
                    {
                        label.Background = Colors.LightGreen;
                        label.FontAttributes = FontAttributes.Bold;
                    } // if contains - orange
                    else if ( WinWord.Contains ( curr ) )
                        label.Background = Colors.Orange;
                    else // not contains - grey
                        label.Background = Colors.LightGrey;

                    // animation
                    label.FadeTo ( 1, 1000 ); // no await!!!
                }

                // add user entry word to words list
                Words.Add ( word );
                WordEntry = "";

                // check end of game:
                if ( word == WinWord ) // win
                    GameOver ( true );
                else if ( Words.Count == HIG ) // lose
                    GameOver ( false );
            }
        }
        else Info = "word must be 5 letters";

        wordentry.Focus ();
    }
    async void GameOver ( bool isWin, bool isTimeout = false )
    {
        await Task.Delay ( 1000 );// delay for wait end animation
        timer?.Dispose ();

        //state-dependent setting of element settings 
        entryBox.IsVisible = false;
        overBox.IsVisible = true;

        if ( isWin )
        {
            game.Result = "Win";
            winLabel.IsVisible = true;
        }
        else
        {
            loseLabel.IsVisible = true;
            if ( isTimeout )
                game.Result = "Timeout";
            else
                game.Result = "Lose";
        }
        wordLabel.IsVisible = !isWin;

        game.Time = ( DateTime.Now - game.Begin ).TotalMinutes;
        game.Steps = Words.Count;
        Settings.CurrentUser.PlayList.Insert ( 0, game );
        Settings.SaveUser ();
    }
    protected override void OnAppearing ()
    {
        base.OnAppearing ();
        levelText.Text = Settings.CurrentUser.Level;
        wordentry.Focus ();
    }
    protected override void OnDisappearing ()
    {
        base.OnDisappearing ();
        timer?.Dispose ();
    }
    private void PlayClick ( object sender, EventArgs e )
    {
        Start ();
    }
    private void InfoClick ( object sender, EventArgs e )
    {
        Shell.Current.Navigation.PushAsync ( new UserPage () );
    }

    private void ExitClick ( object sender, EventArgs e )
    {
        Shell.Current.Navigation.PopAsync ();
    }
}