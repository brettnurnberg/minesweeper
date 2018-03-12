using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace minesweeper
{
    public partial class MainGUI : Form
    {
        private GameData data;
        private SpotUpdater searchMine;
        private SpotUpdater setFlag;
        private bool mouseDown;
        private int mouseX;
        private int mouseY;
        private MouseButtons mouseB;

        public MainGUI(GameData data_, SpotUpdater setFlag_, SpotUpdater searchMine_)
        {
            data = data_;
            setFlag = setFlag_;
            searchMine = searchMine_;
            mouseDown = false;
            InitializeComponent(true);
            setDigital(mineCount, data.mineCount);
        }

        /* Update image of correct spot */
        public void updateSpot(int x, int y)
        {
            /* Set given spot to correct image */
            int idx = data.width * y + x;
            switch(data.status[x, y])
            {
                case MineStatus.UNCLICKED:
                    spots[idx].ImageLocation = "res/unsel.png";
                    break;
                case MineStatus.FLAGGED:
                    spots[idx].ImageLocation = "res/flag.png";
                    break;
                case MineStatus.QUESTION:
                    spots[idx].ImageLocation = "res/question.png";
                    break;
                case MineStatus.CLICKED:
                    if(data.mines[x, y] == -1)
                    {
                        spots[idx].ImageLocation = "res/mine_sel.png";
                    }
                    else
                    {
                        spots[idx].ImageLocation = "res/sel_" + data.mines[x, y] + ".png";
                    }
                    break;
            }
        }

        /* Update minesweeper header */
        public void updateHeader()
        {
            /* Update face image */
            switch(data.gameStatus)
            {
                case GameStatus.LOST:
                    face.ImageLocation = "res/face_lost.png";
                    break;
                case GameStatus.WON:
                    face.ImageLocation = "res/face_won.png";
                    break;
                case GameStatus.PLAY:
                    face.ImageLocation = "res/face_play.png";
                    break;
            }

            /* Update mine count */
            setDigital(mineCount, Math.Max(0, data.mineCount - data.flagCount));
            setDigital(time, data.currTime);
        }

        /* Used to hold down button on a drag */
        private void onSpotEnter(Object sender, EventArgs e)
        {
            if(mouseDown)
            {
                PictureBox p = (PictureBox)sender;

                setMouseCoords(p);
                if ((data.status[mouseX, mouseY] == MineStatus.UNCLICKED
                    || data.status[mouseX, mouseY] == MineStatus.QUESTION)
                    && mouseB == MouseButtons.Left)
                {
                    p.ImageLocation = "res/sel_0.png";
                }
            }
        }

        /* Used to hold down button on a drag */
        private void onSpotLeave(Object sender, EventArgs e)
        {
            if(mouseDown)
            {
                updateSpot(mouseX, mouseY);
                this.Capture = false;
            }
        }

        /* Used for when the control leaves the form */
        private void leftForm(Object sender, EventArgs e)
        {
            mouseDown = false;
            updateHeader();
        }

        /* Runs when the mouse is released over a square */
        private void onSpotRelease(Object sender, EventArgs e)
        {
            /* Fix so control doesn't capture the mouse */
            PictureBox p = (PictureBox)sender;
            p.Capture = true;

            if (data.gameStatus == GameStatus.PLAY && mouseDown == true)
            {
                MouseEventArgs me = (MouseEventArgs)e;

                /* call correct function */
                if (me.Button == MouseButtons.Left)
                {
                    searchMine(mouseX, mouseY);
                }
                else if (me.Button == MouseButtons.Right)
                {
                    setFlag(mouseX, mouseY);
                }
                updateHeader();
            }
            mouseDown = false;
        }

        /* Runs when the mouse is pressed over a square */
        private void onSpotPress(Object sender, EventArgs e)
        {
            mouseDown = true;

            PictureBox p = (PictureBox)sender;
            setMouseCoords(p);
            p.Capture = false;
            MouseEventArgs me = (MouseEventArgs)e;
            mouseB = me.Button;

            if (data.gameStatus == GameStatus.PLAY && mouseB == MouseButtons.Left)
            {
                face.ImageLocation = "res/face_click.png";
                if(data.status[mouseX, mouseY] == MineStatus.UNCLICKED || data.status[mouseX, mouseY] == MineStatus.QUESTION)
                {
                    p.ImageLocation = "res/sel_0.png";
                }
            }
        }

        private void setMouseCoords(PictureBox p)
        {
            int idx = spots.IndexOf(p);

            /* get coordinates of clicked box */
            mouseX = idx % data.width;
            mouseY = idx / data.width;
        }

        /* Set the digital number to given value */
        private void setDigital(List<PictureBox> p, int val)
        {
            int hun, ten, one;

            hun = val / 100;
            ten = (val % 100) / 10;
            one = val % 10;

            p[0].ImageLocation = "res/dig_" + hun + ".png";
            p[1].ImageLocation = "res/dig_" + ten + ".png";
            p[2].ImageLocation = "res/dig_" + one + ".png";
        }

        /* Show all mines on the board */
        public void showMines()
        {
            int idx = 0;

            for(int j = 0; j < data.height; j++)
            {
                for (int i = 0; i < data.width; i++)
                {
                    if(data.mines[i, j] == -1 && data.status[i, j] != MineStatus.CLICKED)
                    {
                        spots[idx].ImageLocation = "res/mine.png";
                    }
                    else if (data.status[i, j] == MineStatus.FLAGGED)
                    {
                        spots[idx].ImageLocation = "res/mine_not.png";
                    }
                    idx++;
                }
            }
        }

        /* Show all flags on the board */
        public void showFlags()
        {
            int idx = 0;
            
            for (int j = 0; j < data.height; j++)
            {
                for (int i = 0; i < data.width; i++)
                {
                    if (data.mines[i, j] == -1)
                    {
                        spots[idx].ImageLocation = "res/flag.png";
                    }
                    idx++;
                }
            }
        }

        /* Event handler to create a new game of same size */
        private void newGame(Object sender, EventArgs e)
        {
            newGameSize(data.width, data.height, data.mineCount);
        }

        private void newBeginnerGame(Object sender, EventArgs e)
        {
            newGameSize(9, 9, 10);
        }

        private void newIntermediateGame(Object sender, EventArgs e)
        {
            newGameSize(16, 16, 40);
        }

        private void newExpertGame(Object sender, EventArgs e)
        {
            newGameSize(31, 16, 99);
        }

        private void newGameSize(int width, int height, int mines)
        {
            int oldw = data.width;
            int oldh = data.height;
            timer.Enabled = false;

            data.newGame(width, height, mines);
            if(oldw != width || oldh != height)
            {
                InitializeComponent(false);
            }
            else
            {
                for(int i = 0; i < width*height; i++)
                    spots[i].ImageLocation = "res/unsel.png";
            }
            updateHeader();
        }

        private void exitGame(Object sender, EventArgs e)
        {
            this.Close();
        }

        private void onTick(Object sender, EventArgs e)
        {
            data.currTime++;
            setDigital(time, data.currTime);
        }

        public void timerRunning(bool enabled)
        {
            timer.Enabled = enabled;
        }

    }
}
