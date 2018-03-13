using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using minesweeper.Properties;

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
        private List<Image> sels;
        private List<Image> digs;

        public MainGUI(GameData data_, SpotUpdater setFlag_, SpotUpdater searchMine_)
        {
            data = data_;
            setFlag = setFlag_;
            searchMine = searchMine_;
            initImageLists();
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
                    spots[idx].Image = Resources.unsel;
                    break;
                case MineStatus.FLAGGED:
                    spots[idx].Image = Resources.flag;
                    break;
                case MineStatus.QUESTION:
                    spots[idx].Image = Resources.question;
                    break;
                case MineStatus.CLICKED:
                    if(data.mines[x, y] == -1)
                    {
                        spots[idx].Image = Resources.mine_sel;
                    }
                    else
                    {
                        spots[idx].Image = sels[data.mines[x, y]];
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
                    face.Image = Resources.face_lost;
                    break;
                case GameStatus.WON:
                    face.Image = Resources.face_won;
                    break;
                case GameStatus.PLAY:
                    face.Image = Resources.face_play;
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
                    p.Image = Resources.sel_0;
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
                face.Image = Resources.face_click;
                if(data.status[mouseX, mouseY] == MineStatus.UNCLICKED || data.status[mouseX, mouseY] == MineStatus.QUESTION)
                {
                    p.Image = Resources.sel_0;
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

            hun = (val % 1000) / 100;
            ten = (val % 100) / 10;
            one = val % 10;

            p[0].Image = digs[hun];
            p[1].Image = digs[ten];
            p[2].Image = digs[one];
        }

        /* Show all mines on the board */
        public void showMines()
        {
            int idx = 0;

            for(int j = 0; j < data.height; j++)
            {
                for (int i = 0; i < data.width; i++)
                {
                    if(data.mines[i, j] == -1 && data.status[i, j] != MineStatus.CLICKED && data.status[i, j] != MineStatus.FLAGGED)
                    {
                        spots[idx].Image = Resources.mine;
                    }
                    else if (data.status[i, j] == MineStatus.FLAGGED && data.mines[i, j] != -1)
                    {
                        spots[idx].Image = Resources.mine_not;
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
                        spots[idx].Image = Resources.flag;
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
                    spots[i].Image = Resources.unsel;
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

        private void initImageLists()
        {
            sels = new List<Image>();
            digs = new List<Image>();

            sels.Add(Resources.sel_0);
            sels.Add(Resources.sel_1);
            sels.Add(Resources.sel_2);
            sels.Add(Resources.sel_3);
            sels.Add(Resources.sel_4);
            sels.Add(Resources.sel_5);
            sels.Add(Resources.sel_6);
            sels.Add(Resources.sel_7);
            sels.Add(Resources.sel_8);

            digs.Add(Resources.dig_0);
            digs.Add(Resources.dig_1);
            digs.Add(Resources.dig_2);
            digs.Add(Resources.dig_3);
            digs.Add(Resources.dig_4);
            digs.Add(Resources.dig_5);
            digs.Add(Resources.dig_6);
            digs.Add(Resources.dig_7);
            digs.Add(Resources.dig_8);
            digs.Add(Resources.dig_9);
        }

    }
}
