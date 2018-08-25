using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace minesweeper
{
    class HighScoreEntry
    {
        public string name { get; set; }
        public double time { get; set; }

        public HighScoreEntry(string name_, double time_)
        {
            name = name_;
            time = time_;
        }
    }
}
