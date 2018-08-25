using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

/* 
 * Master To Do list:
 * Implement statistics
 *   Create file implementation.
 * Implement custom level selection
 */

namespace minesweeper
{
    public delegate void Updater();
    public delegate void TimeUpdater(bool enabled);
    public delegate void SpotUpdater(int x, int y);
    public delegate bool HSChecker(double t, GameDifficulty d);
    public delegate void HSSetter(string n, double t, GameDifficulty d);

    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            GameData data = new GameData();
            HighScoreData hsData = new HighScoreData();
            
            HighScoreController hsc = new HighScoreController(hsData);
            GameController c = new GameController(data, hsc.isHighscore, hsc.updateHighscore);

            MainGUI gui = new MainGUI(data, c.setFlag, c.searchMine);

            c.registerUpdater(gui.updateHeader);
            c.registerUpdater(gui.updateSpot);
            c.registerDisplayFlags(gui.showFlags);
            c.registerDisplayMines(gui.showMines);
            c.registerTimer(gui.timerRunning);

            Application.Run(gui);
        }
    }
}
