using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace minesweeper
{
    class GameController
    {
        /* Must be able to get clicked square. Must also know which type of click it was */
        /* Must be able to create a new game */
        private Updater updateHeader;
        private Updater showMines;
        private Updater showFlags;
        private TimeUpdater timerRunning;
        private SpotUpdater updateSpot;
        private HSChecker isHighscore;
        private HSSetter updateHighscore;
        private GameData data;

        public GameController(GameData data_, HSChecker hschk, HSSetter hsset)
        {
            isHighscore = hschk;
            updateHighscore = hsset;
            data = data_;
        }
        
        public void setFlag(int x, int y)
        {
            MineStatus s = data.status[x, y];

            if(s != MineStatus.CLICKED)
            {
                if(s == MineStatus.QUESTION)
                {
                    data.status[x, y] = MineStatus.UNCLICKED;
                }
                else if(s == MineStatus.UNCLICKED)
                {
                    data.status[x, y] = MineStatus.FLAGGED;
                    data.flagCount++;
                }
                else if(s == MineStatus.FLAGGED)
                {
                    data.status[x, y] = MineStatus.QUESTION;
                    data.flagCount--;
                }
            }
            updateSpot(x, y);
        }

        /* Search current spot for a mine */
        public void searchMine(int x, int y)
        {
            MineStatus s = data.status[x, y];

            /* Start timer on first click */
            if(data.clickedCount == 0)
            {
                data.startTime = DateTime.Now;
                timerRunning(true);
                data.currTime = 0;
            }

            /* Update given location status */
            if(s == MineStatus.UNCLICKED || s == MineStatus.QUESTION)
            {
                data.status[x, y] = MineStatus.CLICKED;
                if (data.mines[x, y] == 0)
                {
                    data.clickedCount++;
                    for (int i = Math.Max(0, x - 1); i <= Math.Min(x + 1, data.width - 1); i++)
                    {
                        for (int j = Math.Max(0, y - 1); j <= Math.Min(y + 1, data.height - 1); j++)
                        {
                            searchMine(i, j);
                        }
                    }
                }
                else if(data.mines[x, y] == -1)
                {
                    data.gameStatus = GameStatus.LOST;
                    timerRunning(false);
                    showMines();
                }
                else
                {
                    data.clickedCount++;
                }

                /* Determine if game was won */
                if (data.clickedCount + data.mineCount == data.width * data.height)
                {
                    data.gameStatus = GameStatus.WON;
                    showFlags();
                    data.flagCount = data.mineCount;
                    data.gameTime = (DateTime.Now - data.startTime).TotalSeconds;
                    timerRunning(false);
                    if(isHighscore(data.gameTime, data.level))
                    {
                        string name = "Anonymous";
                        /* prompt user for name */
                        //name = getPlayerName();
                        updateHighscore(name, data.gameTime, data.level);
                    }
                }
                updateSpot(x, y);
            }
        }

        public void registerUpdater(Updater update_)
        {
            updateHeader = update_;
        }

        public void registerUpdater(SpotUpdater update_)
        {
            updateSpot = update_;
        }

        public void registerDisplayFlags(Updater update_)
        {
            showFlags = update_;
        }

        public void registerDisplayMines(Updater update_)
        {
            showMines = update_;
        }

        public void registerTimer(TimeUpdater update_)
        {
            timerRunning = update_;
        }
    }
}
