using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Puzzle.Repo
{
    internal class User
    {
        public string Name { get; set; }
        public AppTheme ColorTheme { get; set; }
        public List<GameInfo> PlayList { get; set; } = [];
        public string Level { get; set; } = "Easy";
        public User () { }
        public User ( string name )
        {
            Name = name;
        }
    }
}
