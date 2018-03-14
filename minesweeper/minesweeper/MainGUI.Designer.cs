using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using minesweeper.Properties;

namespace minesweeper
{
    partial class MainGUI
    {
        private System.ComponentModel.IContainer components = null;
        private Timer timer;
        private MenuStrip menu;
        private ToolStripMenuItem gameMenu;
        private ToolStripMenuItem newGameItem;
        private ToolStripMenuItem beginnerItem;
        private ToolStripMenuItem intermediateItem;
        private ToolStripMenuItem expertItem;
        private ToolStripMenuItem statisticsItem;
        private ToolStripMenuItem exitItem;
        private MainGUIDimensions dims;

        private List<PictureBox> mineCount;
        private List<PictureBox> time;
        private PictureBox face;
        private PictureBox border;
        private PictureBox spots;


        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }


        private void InitializeComponent()
        {
            /* Create main window */
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Text = "Minesweeper";
            this.BackColor = Color.FromArgb(189, 189, 189);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Capture = false;
            this.MouseLeave += new EventHandler(leftForm);

            /* Create timer */
            timer = new Timer(components);
            timer.Interval = 1000;
            timer.Stop();
            timer.Tick += new EventHandler(onTick);

            /* Create menus */
            newGameItem = new ToolStripMenuItem();
            newGameItem.Text = "New Game";
            newGameItem.Click += new EventHandler(newGame);

            beginnerItem = new ToolStripMenuItem();
            beginnerItem.Text = "Beginner";
            beginnerItem.Click += new EventHandler(newBeginnerGame);

            intermediateItem = new ToolStripMenuItem();
            intermediateItem.Text = "Intermediate";
            intermediateItem.Click += new EventHandler(newIntermediateGame);

            expertItem = new ToolStripMenuItem();
            expertItem.Text = "Expert";
            expertItem.Click += new EventHandler(newExpertGame);

            statisticsItem = new ToolStripMenuItem();
            statisticsItem.Text = "Statistics";
            //statisticsItem.Click += new EventHandler();

            exitItem = new ToolStripMenuItem();
            exitItem.Text = "Exit";
            exitItem.Click += new EventHandler(exitGame);

            gameMenu = new ToolStripMenuItem();
            gameMenu.Text = "Game";
            gameMenu.DropDownItems.Add(newGameItem);
            gameMenu.DropDownItems.Add(beginnerItem);
            gameMenu.DropDownItems.Add(intermediateItem);
            gameMenu.DropDownItems.Add(expertItem);
            gameMenu.DropDownItems.Add(statisticsItem);
            gameMenu.DropDownItems.Add(exitItem);

            menu = new MenuStrip();
            menu.Items.Add(gameMenu);
            this.Controls.Add(menu);
            this.MainMenuStrip = menu;
            
            /* Create all controls */
            mineCount = new List<PictureBox>();
            time = new List<PictureBox>();
            face = new PictureBox();
            border = new PictureBox();
            spots = new PictureBox();

            spots.MouseDown += new MouseEventHandler(onSpotPress);
            spots.MouseUp += new MouseEventHandler(onSpotRelease);
            spots.MouseMove += new MouseEventHandler(onMouseMove);
            this.Controls.Add(border);
            this.Controls.Add(spots);

            /* Initialize header */
            for(int i = 0; i < 3; i++)
            {
                PictureBox p = new PictureBox();
                p.Image = Resources.dig_0;
                mineCount.Add(p);
                this.Controls.Add(p);
            }

            for (int i = 0; i < 3; i++)
            {
                PictureBox p = new PictureBox();
                p.Image = Resources.dig_0;
                time.Add(p);
                this.Controls.Add(p);
            }

            face.Image = Resources.face_play;
            face.Click += new EventHandler(newGame);
            this.Controls.Add(face);

            /* Draw game */
            drawGame();
        }

        private void drawGame()
        {
            /* Initialize dimensions */
            dims = new MainGUIDimensions(data.width, data.height);

            /* Initialize bitmaps */
            borderImg = new Bitmap(dims.window.Size.Width, dims.window.Size.Height);
            spotsImg = new Bitmap(data.width * 16, data.height * 16);
            border.Size = new Size(dims.window.Size.Width, dims.window.Size.Height);
            border.Location = new Point(0, 0);
            spots.Size = new Size(data.width* 16, data.height * 16);
            spots.Location = new Point(dims.border_vl.Size.Width, dims.menuBar.Size.Height + 2 * dims.border_tl.Size.Height + dims.border_vl.Size.Height);

            /* Draw border */
            drawOnBitmap(dims.border_tl.Location.X, dims.border_tl.Location.Y, Resources.border_tl, borderImg);
            drawOnBitmap(dims.border_tr.Location.X, dims.border_tr.Location.Y, Resources.border_tr, borderImg);
            drawOnBitmap(dims.border_vl.Location.X, dims.border_vl.Location.Y, Resources.border_vt, borderImg);
            drawOnBitmap(dims.border_vr.Location.X, dims.border_vr.Location.Y, Resources.border_vt, borderImg);
            drawOnBitmap(dims.border_ml.Location.X, dims.border_ml.Location.Y, Resources.border_ml, borderImg);
            drawOnBitmap(dims.border_mr.Location.X, dims.border_mr.Location.Y, Resources.border_mr, borderImg);
            drawOnBitmap(dims.border_bl.Location.X, dims.border_bl.Location.Y, Resources.border_bl, borderImg);
            drawOnBitmap(dims.border_br.Location.X, dims.border_br.Location.Y, Resources.border_br, borderImg);

            for (int i = 0; i < data.width; i++)
            {
                drawOnBitmap(dims.border_tl.Size.Width + i * 16, dims.border_tl.Location.Y, Resources.border_h, borderImg);
                drawOnBitmap(dims.border_tl.Size.Width + i * 16, dims.border_ml.Location.Y, Resources.border_h, borderImg);
                drawOnBitmap(dims.border_tl.Size.Width + i * 16, dims.border_bl.Location.Y, Resources.border_h, borderImg);
            }

            for (int i = 0; i < data.height; i++)
            {
                drawOnBitmap(dims.border_ml.Location.X, dims.border_ml.Location.Y + dims.border_ml.Size.Height + i * 16, Resources.border_v, borderImg);
                drawOnBitmap(dims.border_mr.Location.X, dims.border_ml.Location.Y + dims.border_ml.Size.Height + i * 16, Resources.border_v, borderImg);
            }

            /* Draw spots */
            for(int i = 0; i < data.width; i++)
            {
                for (int j = 0; j < data.height; j++)
                {
                    drawSpot(i, j, Resources.unsel);
                }
            }

            /* After images are created, set all locations */
            this.ClientSize = dims.window.Size;
            menu.Location = dims.menuBar.Location;
            menu.Size = dims.menuBar.Size;
            
            border.Image = borderImg;
            border.SendToBack();
            spots.Image = spotsImg;
            spots.BringToFront();

            for (int i = 0; i < 3; i++)
            {
                mineCount[i].Size = dims.mineCount[i].Size;
                mineCount[i].Location = dims.mineCount[i].Location;
            }

            for (int i = 0; i < 3; i++)
            {
                time[i].Size = dims.time[i].Size;
                time[i].Location = dims.time[i].Location;
            }

            face.Size = dims.face.Size;
            face.Location = dims.face.Location;
        }

    }
}

