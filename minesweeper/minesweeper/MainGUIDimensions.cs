using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace minesweeper
{
    class MainGUIDimensions
    {
        public ControlDimension window { get; set; }
        public ControlDimension menuBar { get; set; }
        public ControlDimension border_tl { get; set; }
        public ControlDimension border_tr { get; set; }
        public ControlDimension border_vl { get; set; }
        public ControlDimension border_vr { get; set; }
        public ControlDimension border_ml { get; set; }
        public ControlDimension border_mr { get; set; }
        public ControlDimension border_bl { get; set; }
        public ControlDimension border_br { get; set; }
        public List<ControlDimension> mineCount { get; set; }
        public List<ControlDimension> time { get; set; }
        public ControlDimension face { get; set; }

        public MainGUIDimensions(int width, int height)
        {
            window = new ControlDimension();
            menuBar = new ControlDimension();
            border_tl = new ControlDimension();
            border_tr = new ControlDimension();
            border_vl = new ControlDimension();
            border_vr = new ControlDimension();
            border_ml = new ControlDimension();
            border_mr = new ControlDimension();
            border_bl = new ControlDimension();
            border_br = new ControlDimension();
            mineCount = new List<ControlDimension>();
            time = new List<ControlDimension>();
            face = new ControlDimension();

            menuBar.Size = new Size(20 + 16 * width, 24);
            menuBar.Location = new Point(0, 0);
            window.Size = new Size(20 + 16 * width, menuBar.Size.Height + 63 + 16 * height);

            border_tl.Size = new Size(10, 10);
            border_tl.Location = new Point(0, menuBar.Size.Height);
            border_tr.Size = new Size(10, 10);
            border_tr.Location = new Point(window.Size.Width - border_tr.Size.Width, menuBar.Size.Height);

            border_vl.Size = new Size(10, 33);
            border_vl.Location = new Point(0, border_tl.Location.Y + border_tl.Size.Height);
            border_vr.Size = new Size(10, 33);
            border_vr.Location = new Point(border_tr.Location.X, border_vl.Location.Y);

            border_ml.Size = new Size(10, 10);
            border_ml.Location = new Point(0, border_vl.Location.Y + border_vl.Size.Height);
            border_mr.Size = new Size(10, 10);
            border_mr.Location = new Point(border_vr.Location.X, border_ml.Location.Y);

            border_bl.Size = new Size(10, 10);
            border_bl.Location = new Point(0, window.Size.Height - border_bl.Size.Height);
            border_br.Size = new Size(10, 10);
            border_br.Location = new Point(border_mr.Location.X, border_bl.Location.Y);

            for(int i = 0; i < 3; i++)
            {
                ControlDimension d = new ControlDimension();
                d.Size = new Size(13, 23);
                d.Location = new Point(16 + i * d.Size.Width, 15 + menuBar.Size.Height);
                mineCount.Add(d);
            }

            for (int i = 0; i < 3; i++)
            {
                ControlDimension d = new ControlDimension();
                d.Size = new Size(13, 23);
                d.Location = new Point(window.Size.Width - 55 + i * d.Size.Width, 15 + menuBar.Size.Height);
                time.Add(d);
            }

            face.Size = new Size(26, 26);
            face.Location = new Point((window.Size.Width - face.Size.Width) / 2, 13 + menuBar.Size.Height);

        }
    }
}
