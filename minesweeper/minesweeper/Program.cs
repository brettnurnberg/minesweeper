using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

/* Master To Do list:
 * Figure out how to speed up loading images
 *   Images are preloaded as resources - somewhat sped loading.
 *   Could look into treating entire image as one large bitmap.
 * Implement statistics
 *   Simply add three "name" strings and three "time" strings in resources.
 * Implement custom level selection
 */

namespace minesweeper
{
    public delegate void Updater();
    public delegate void TimeUpdater(bool enabled);
    public delegate void SpotUpdater(int x, int y);

    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            GameData data = new GameData();
            GameController c = new GameController(data);

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
