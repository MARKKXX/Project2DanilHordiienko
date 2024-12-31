using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Puzzle.Repo
{
    internal class LevelInfo
    {
        public string Name { get; set; } // Easy Default Hard
        public int Time { get; set; }

        public static List<LevelInfo> LevelList =
            [
              new (){ Name = "Easy", Time=0},
              new (){ Name = "Default", Time=10},
              new (){ Name = "Hard", Time=5},
            ];
    }
}
