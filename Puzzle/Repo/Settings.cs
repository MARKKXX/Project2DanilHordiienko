global using Puzzle.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Puzzle.Repo
{
    internal static class Settings
    {
        static string SettFolder;
        static string UsersFolder;

        internal static List<string> Words;
        internal static User CurrentUser;
        internal static Random Rnd = new Random ();

        static Settings ()
        {
            SettFolder = FileSystem.Current.AppDataDirectory;
            UsersFolder = Path.Combine ( SettFolder, "users" );
            if ( !Directory.Exists ( UsersFolder ) )
                Directory.CreateDirectory ( UsersFolder );

            // load word list from http
            string wordfile = Path.Combine ( SettFolder, "words.txt" );
            if ( !File.Exists ( wordfile ) )
            {
                try
                {
                    HttpClient http = new ();
                    var response = http.GetAsync ( "https://raw.githubusercontent.com/DonH-ITS/jsonfiles/main/words.txt" ).Result;
                    response.EnsureSuccessStatusCode ();
                    var words = response.Content.ReadAsStringAsync ().Result;
                    File.WriteAllText ( wordfile, words.ToLower () );
                }
                catch ( Exception ex ) { throw new Exception ( "Error loading words from http\r\n" + ex.Message ); }
            }
            try
            {
                Words = File.ReadAllLines ( wordfile ).ToList ();
            }
            catch ( Exception ex ) { throw new Exception ( "Error loading words list\r\n" + ex.Message ); }
        }

        internal static void UserEntered ( string name )
        {
            string exists = GetUsersList ().FirstOrDefault ( n => n.ToLower () == name.ToLower () );
            User user = null;

            if ( exists == null ) // no such user - create new            
                user = new User ( name );
            else
            {
                
                string json = File.ReadAllText ( Path.Combine ( UsersFolder, $"{name}.json" ) );
                user = JsonSerializer.Deserialize<User> ( json );
            }
            CurrentUser = user;
        }

        static List<string> GetUsersList ()
        {
            var list = Directory.GetFiles ( UsersFolder ).Select ( file => Path.GetFileNameWithoutExtension ( file ).ToLower () ).ToList ();
            return list;
        }

        internal static void SaveUser ()
        {
            using Stream fs = File.Create ( Path.Combine ( UsersFolder, $"{CurrentUser.Name}.json" ) );
            JsonSerializer.Serialize ( fs, CurrentUser, new JsonSerializerOptions () { WriteIndented = true } );
        }
    }
}
