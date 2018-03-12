using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

        private PictureBox border_tl;
        private PictureBox border_tr;
        private PictureBox border_vl;
        private PictureBox border_vr;
        private PictureBox border_ml;
        private PictureBox border_mr;
        private PictureBox border_bl;
        private PictureBox border_br;
        private List<PictureBox> mineCount;
        private List<PictureBox> time;
        private PictureBox face;
        private List<PictureBox> borderRowt;
        private List<PictureBox> borderRowm;
        private List<PictureBox> borderRowb;
        private List<PictureBox> borderColl;
        private List<PictureBox> borderColr;
        private List<PictureBox> spots;


        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }


        private void InitializeComponent(bool first)
        {
            /* Create main window */
            dims = new MainGUIDimensions(data.width, data.height);

            if (first)
            {
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
                menu.Location = dims.menuBar.Location;
                menu.Size = dims.menuBar.Size;
                menu.Items.Add(gameMenu);
                this.Controls.Add(menu);
                this.MainMenuStrip = menu;
            }
            else
            {
                removeControls();
            }
            this.ClientSize = dims.window.Size;

            /* Create all controls */
            border_tl = new PictureBox();
            border_tr = new PictureBox();
            border_vl = new PictureBox();
            border_vr = new PictureBox();
            border_ml = new PictureBox();
            border_mr = new PictureBox();
            border_bl = new PictureBox();
            border_br = new PictureBox();
            mineCount = new List<PictureBox>();
            time = new List<PictureBox>();
            face = new PictureBox();
            borderColl = new List<PictureBox>();
            borderColr = new List<PictureBox>();
            borderRowb = new List<PictureBox>();
            borderRowm = new List<PictureBox>();
            borderRowt = new List<PictureBox>();
            spots = new List<PictureBox>();

            /* Initialize controls */
            border_tl.Size = dims.border_tl.Size;
            border_tl.Location = dims.border_tl.Location;
            border_tl.ImageLocation = "res/border_tl.png";
            this.Controls.Add(border_tl);

            border_tr.Size = dims.border_tr.Size;
            border_tr.Location = dims.border_tr.Location;
            border_tr.ImageLocation = "res/border_tr.png";
            this.Controls.Add(border_tr);

            border_vl.Size = dims.border_vl.Size;
            border_vl.Location = dims.border_vl.Location;
            border_vl.ImageLocation = "res/border_vt.png";
            this.Controls.Add(border_vl);

            border_vr.Size = dims.border_vr.Size;
            border_vr.Location = dims.border_vr.Location;
            border_vr.ImageLocation = "res/border_vt.png";
            this.Controls.Add(border_vr);

            border_ml.Size = dims.border_ml.Size;
            border_ml.Location = dims.border_ml.Location;
            border_ml.ImageLocation = "res/border_ml.png";
            this.Controls.Add(border_ml);

            border_mr.Size = dims.border_mr.Size;
            border_mr.Location = dims.border_mr.Location;
            border_mr.ImageLocation = "res/border_mr.png";
            this.Controls.Add(border_mr);

            border_bl.Size = dims.border_bl.Size;
            border_bl.Location = dims.border_bl.Location;
            border_bl.ImageLocation = "res/border_bl.png";
            this.Controls.Add(border_bl);

            border_br.Size = dims.border_br.Size;
            border_br.Location = dims.border_br.Location;
            border_br.ImageLocation = "res/border_br.png";
            this.Controls.Add(border_br);

            for(int i = 0; i < 3; i++)
            {
                PictureBox p = new PictureBox();
                p.Size = dims.mineCount[i].Size;
                p.Location = dims.mineCount[i].Location;
                p.ImageLocation = "res/dig_0.png";
                mineCount.Add(p);
                this.Controls.Add(p);
            }

            for (int i = 0; i < 3; i++)
            {
                PictureBox p = new PictureBox();
                p.Size = dims.time[i].Size;
                p.Location = dims.time[i].Location;
                p.ImageLocation = "res/dig_0.png";
                time.Add(p);
                this.Controls.Add(p);
            }

            face.Size = dims.face.Size;
            face.Location = dims.face.Location;
            face.ImageLocation = "res/face_play.png";
            face.Click += new EventHandler(newGame);
            this.Controls.Add(face);

            for(int i = 0; i < data.width; i++)
            {
                PictureBox p1 = new PictureBox();
                PictureBox p2 = new PictureBox();
                PictureBox p3 = new PictureBox();
                p1.Size = p2.Size = p3.Size = new Size(16, 10);
                p1.Location = new Point(dims.border_tl.Size.Width + i * p1.Size.Width, dims.border_tl.Location.Y);
                p2.Location = new Point(p1.Location.X, dims.border_ml.Location.Y);
                p3.Location = new Point(p1.Location.X, dims.border_bl.Location.Y);
                p1.ImageLocation = p2.ImageLocation = p3.ImageLocation = "res/border_h.png";
                borderRowt.Add(p1);
                borderRowm.Add(p2);
                borderRowb.Add(p3);
                this.Controls.Add(p1);
                this.Controls.Add(p2);
                this.Controls.Add(p3);
            }

            for (int i = 0; i < data.height; i++)
            {
                PictureBox p1 = new PictureBox();
                PictureBox p2 = new PictureBox();
                p1.Size = p2.Size = new Size(10, 16);
                p1.Location = new Point(dims.border_ml.Location.X, dims.border_ml.Location.Y + dims.border_ml.Size.Height + i * p1.Size.Height);
                p2.Location = new Point(dims.border_mr.Location.X, dims.border_ml.Location.Y + dims.border_ml.Size.Height + i * p1.Size.Height);
                p1.ImageLocation = p2.ImageLocation = "res/border_v.png";
                borderColl.Add(p1);
                borderColr.Add(p2);
                this.Controls.Add(p1);
                this.Controls.Add(p2);
            }

            for(int j = 0; j < data.height; j++)
            {
                for (int i = 0; i < data.width; i++)
                {
                    PictureBox p = new PictureBox();
                    p.Size = new Size(16, 16);
                    p.Location = new Point(border_ml.Location.X + border_ml.Size.Width + i * p.Size.Width, border_ml.Location.Y + border_ml.Size.Height + j * p.Size.Height);
                    p.ImageLocation = "res/unsel.png";
                    p.MouseUp += new MouseEventHandler(onSpotRelease);
                    p.MouseDown += new MouseEventHandler(onSpotPress);
                    p.MouseEnter += new EventHandler(onSpotEnter);
                    p.MouseLeave += new EventHandler(onSpotLeave);
                    spots.Add(p);
                }
            }
            this.Controls.AddRange(spots.ToArray());

        }


        private void removeControls()
        {
            this.Controls.Remove(border_tl);
            this.Controls.Remove(border_tr);
            this.Controls.Remove(border_vl);
            this.Controls.Remove(border_vr);
            this.Controls.Remove(border_ml);
            this.Controls.Remove(border_mr);
            this.Controls.Remove(border_bl);
            this.Controls.Remove(border_br);
            this.Controls.Remove(face);
            foreach (PictureBox p in mineCount)
                this.Controls.Remove(p);
            foreach (PictureBox p in time)
                this.Controls.Remove(p);
            foreach (PictureBox p in borderColl)
                this.Controls.Remove(p);
            foreach (PictureBox p in borderColr)
                this.Controls.Remove(p);
            foreach (PictureBox p in borderRowb)
                this.Controls.Remove(p);
            foreach (PictureBox p in borderRowm)
                this.Controls.Remove(p);
            foreach (PictureBox p in borderRowt)
                this.Controls.Remove(p);
            foreach (PictureBox p in spots)
                this.Controls.Remove(p);
        }

    }
}

