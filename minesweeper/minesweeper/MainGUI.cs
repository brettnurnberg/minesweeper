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
        private List<Bitmap> sels;
        private List<Bitmap> digs;
        private Bitmap borderImg;
        private Bitmap spotsImg;

        public MainGUI(GameData data_, SpotUpdater setFlag_, SpotUpdater searchMine_)
        {
            data = data_;
            setFlag = setFlag_;
            searchMine = searchMine_;
            initImageLists();
            mouseDown = false;
            InitializeComponent();
            setDigital(mineCount, data.mineCount);
        }

        /* Update image of correct spot */
        public void updateSpot(int x, int y)
        {
            /* Set given spot to correct image */
            Bitmap image = null;

            switch(data.status[x, y])
            {
                case MineStatus.UNCLICKED:
                    image = Resources.unsel;
                    break;
                case MineStatus.FLAGGED:
                    image = Resources.flag;
                    break;
                case MineStatus.QUESTION:
                    image = Resources.question;
                    break;
                case MineStatus.CLICKED:
                    if(data.mines[x, y] == -1)
                    {
                        image = Resources.mine_sel;
                    }
                    else
                    {
                        image = sels[data.mines[x, y]];
                    }
                    break;
            }
            drawSpot(x, y, image);
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
        private void onMouseMove(Object sender, EventArgs e)
        {
            if(mouseDown && data.gameStatus == GameStatus.PLAY)
            {
                int prevX = mouseX;
                int prevY = mouseY;

                setMouseCoords((MouseEventArgs)e);

                if(prevX != mouseX || prevY != mouseY)
                {
                    if (mouseX >= 0 && mouseX < data.width
                      && mouseY >= 0 && mouseY < data.height)
                    {
                        if ((data.status[mouseX, mouseY] == MineStatus.UNCLICKED
                          || data.status[mouseX, mouseY] == MineStatus.QUESTION)
                          && mouseB == MouseButtons.Left)
                        {
                            drawSpot(mouseX, mouseY, Resources.sel_0);
                        }
                    }
                    if (prevX >= 0 && prevX < data.width
                     && prevY >= 0 && prevY < data.height)
                    {
                        updateSpot(prevX, prevY);
                    }
                }
            }
        }

        /* Used for when the control leaves the form */
        private void leftForm(Object sender, EventArgs e)
        {
            mouseDown = false;
            updateHeader();
        }

        /* Runs when the mouse is released over a square (change to mouse release on bitmap) */
        private void onSpotRelease(Object sender, EventArgs e)
        {

            if ((mouseX >= 0 && mouseX < data.width && mouseY >= 0 && mouseY < data.height)
             && (data.gameStatus == GameStatus.PLAY && mouseDown == true))
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
            }

            updateHeader();
            mouseDown = false;
        }

        /* Runs when the mouse is pressed over a square (change to press on bitmap) */
        private void onSpotPress(Object sender, EventArgs e)
        {
            mouseDown = true;

            MouseEventArgs me = (MouseEventArgs)e;
            mouseB = me.Button;

            setMouseCoords(me);

            if (data.gameStatus == GameStatus.PLAY && mouseB == MouseButtons.Left)
            {
                face.Image = Resources.face_click;
                if(data.status[mouseX, mouseY] == MineStatus.UNCLICKED || data.status[mouseX, mouseY] == MineStatus.QUESTION)
                {
                    drawSpot(mouseX, mouseY, Resources.sel_0);
                }
            }
        }

        /* Sets coordinates of mouse */
        private void setMouseCoords(MouseEventArgs me)
        {
            /* get coordinates of clicked box */
            mouseX = me.X / 16;
            mouseY = me.Y / 16;
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
                        drawSpot(i, j, Resources.mine);
                    }
                    else if (data.status[i, j] == MineStatus.FLAGGED && data.mines[i, j] != -1)
                    {
                        drawSpot(i, j, Resources.mine_not);
                    }
                    idx++;
                }
            }
        }

        /* Show all flags on the board */
        public void showFlags()
        {
            for (int j = 0; j < data.height; j++)
            {
                for (int i = 0; i < data.width; i++)
                {
                    if (data.mines[i, j] == -1)
                    {
                        drawSpot(i, j, Resources.flag);
                    }
                }
            }
        }

        /* Event handler to create a new game of same size */
        private void newGame(Object sender, EventArgs e)
        {
            newGameSize(data.width, data.height, data.mineCount);
        }

        private void onFacePress(Object sender, EventArgs e)
        {
            face.Image = Resources.face_down;
        }

        private void newBeginnerGame(Object sender, EventArgs e)
        {
            data.level = GameDifficulty.BEGINNER;
            newGameSize(9, 9, 10);
        }

        private void newIntermediateGame(Object sender, EventArgs e)
        {
            data.level = GameDifficulty.INTERMEDIATE;
            newGameSize(16, 16, 40);
        }

        private void newExpertGame(Object sender, EventArgs e)
        {
            data.level = GameDifficulty.EXPERT;
            newGameSize(31, 16, 99);
        }

        /* Initialize new game with given size */
        private void newGameSize(int width, int height, int mines)
        {
            int oldw = data.width;
            int oldh = data.height;
            timer.Enabled = false;

            data.newGame(width, height, mines);

            if(oldw == data.width && oldh == data.height)
            {
                resetSpots();
            }
            else
            {
                drawGame();
            }

            updateHeader();
        }

        private void exitGame(Object sender, EventArgs e)
        {
            this.Close();
        }

        /* Draw the given image on the spots image */
        private void drawSpot(int x, int y, Bitmap img)
        {
            drawOnBitmap(x * 16, y * 16, img, spotsImg);
            spots.Image = spotsImg;
        }

        /* Draw an image on bitmap at given location */
        private void drawOnBitmap(int xpixel, int ypixel, Bitmap overImage, Bitmap underImage)
        {
            for (int i = 0; i < overImage.Width; i++)
            {
                for (int j = 0; j < overImage.Height; j++)
                {
                    underImage.SetPixel(i + xpixel, j + ypixel, overImage.GetPixel(i, j));
                }
            }
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
            sels = new List<Bitmap>();
            digs = new List<Bitmap>();

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
