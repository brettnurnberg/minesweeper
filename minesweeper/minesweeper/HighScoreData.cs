using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using minesweeper.Properties;

namespace minesweeper
{
    class HighScoreData
    {
        private List<HighScoreEntry> scores;
        private string filename;

        public HighScoreData()
        {
            filename = "hs";
            scores = new List<HighScoreEntry>();
            initData();
        }

        private void initData()
        {
            for(int i = 0; i < 3; i++)
            {
                HighScoreEntry e = new HighScoreEntry("Anonymous", 999);
                scores.Add(e);
            }
            readScores();
        }

        private void readScores()
        {
            if (File.Exists(filename))
            {
                int idx = 0;
                foreach (string line in File.ReadLines(filename))
                {
                    if (line != string.Empty)
                    {
                        string[] record = line.Split('\u001f');
                        scores[idx].name = record[0];
                        scores[idx].time = Double.Parse(record[1]);
                        idx++;
                    }
                }
            }
            else
            {
                File.Create(filename);
                writeScores();
            }
        }

        private void writeScores()
        {
            /*using (StreamWriter sw = File.CreateText(filename))
            {
                foreach (HighScoreEntry e in scores)
                {
                    sw.WriteLine(e.name + '\u001f' + e.time.ToString());
                }
            }*/
        }

        public void changeScore(string name, double time, GameDifficulty d)
        {
            scores[(int)d].name = name;
            scores[(int)d].time = time;
            writeScores();
        }

        public bool isHighscore(double time, GameDifficulty d)
        {
            return time < scores[(int)d].time;
        }
    }
}
