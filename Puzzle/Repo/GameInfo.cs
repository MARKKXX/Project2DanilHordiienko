using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Puzzle.Repo
{
    public class GameInfo
    {
        public string Word{ get; set; }
        public string Date { get; set; }
        public double Time { get; set; }
        public string Level { get; set; }
        public string Result { get; set; }
        public int Steps{ get; set; }

        [JsonIgnore]
        public string DisplayTime => TimeSpan.FromMinutes ( Time ).ToString ( "m\\:ss" );
        [JsonIgnore]
        internal DateTime Begin;

        public GameInfo ( string word)
        {
            Word = word;
            Begin = DateTime.Now;
            Date = Begin.ToString ( "dd-MM-yyyy HH:mm" );
        }
    }
}
