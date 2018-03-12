using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace minesweeper
{
    public class GameData
    {
        public int width { get; set; }
        public int height { get; set; }
        public int mineCount { get; set; }
        public int flagCount { get; set; }
        public int clickedCount { get; set; }
        public int[,] mines { get; set; }
        public MineStatus[,] status { get; set; }
        public GameStatus gameStatus { get; set; }
        public DateTime startTime { get; set; }
        public int currTime { get; set; }
        public double gameTime { get; set; }

        public GameData()
        {
            newGame(9, 9, 10);
        }

        /* Generate minesweeper gameboard */
        private void genMines()
        {
            mines = new int[width, height];
            int numMines = mineCount;
            int x, y;
            Random r = new Random();

            /* Generate mine locations */
            while(numMines != 0)
            {
                x = r.Next(0, width);
                y = r.Next(0, height);

                if(mines[x, y] != -1)
                {
                    mines[x, y] = -1;
                    numMines--;
                }
            }

            /* Generate mine counts */
            for(int i = 0; i < width; i++)
            {
                for(int j = 0; j < height; j++)
                {
                    if(mines[i, j] != -1)
                    {
                        mines[i, j] = countMines(i, j);
                    }
                }
            }
        }

        /* Count number of mines around given spot */
        private int countMines(int x, int y)
        {
            int count = 0;

            for(int i = Math.Max(0, x-1); i <= Math.Min(x+1, width-1); i++)
            {
                for(int j = Math.Max(0, y-1); j <= Math.Min(y+1, height-1); j++)
                {
                    if(mines[i, j] == -1)
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        /* Create new game */
        public void newGame(int width_, int height_, int mines_)
        {
            width = width_;
            height = height_;
            mineCount = mines_;
            flagCount = 0;
            clickedCount = 0;
            currTime = 0;
            gameStatus = GameStatus.PLAY;
            status = new MineStatus[width, height];
            genMines();
        }

    }
}
