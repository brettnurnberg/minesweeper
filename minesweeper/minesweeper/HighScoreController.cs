using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace minesweeper
{
    class HighScoreController
    {
        HighScoreData data;

        public HighScoreController(HighScoreData data_)
        {
            data = data_;
        }

        public bool isHighscore(double time, GameDifficulty d)
        {
            return data.isHighscore(time, d);
        }

        public void updateHighscore(string name, double time, GameDifficulty d)
        {
            data.changeScore(name, time, d);
        }
    }
}
