using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace 多边形三角化 {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        Brush[] brushes = new Brush[] {
            Brushes.Yellow, Brushes.Violet, Brushes.SlateGray, Brushes.SpringGreen, Brushes.Tan, Brushes.Pink, Brushes.Teal, Brushes.Purple,
            Brushes.OldLace, Brushes.Salmon, Brushes.OliveDrab, Brushes.DeepSkyBlue, Brushes.BurlyWood, Brushes.Thistle, Brushes.Turquoise, Brushes.Orchid,
        };

        List<Vec> polygon = new List<Vec>();

        private void btnClear_Click(object sender, EventArgs e) {
            polygon.Clear();
            pictureBox1.Invalidate();
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e) {
            polygon.Add(new Vec(e.X, e.Y));
            pictureBox1.Invalidate();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e) {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // 内部三角形
            if (polygon.Count >= 3) {
                List<int> tris = PolygonHelper.Resolve(polygon);
                if (tris != null) {
                    PointF[] points = new PointF[3];

                    // 画出拆出的三角形
                    for (int i = 0, j = 0; i < tris.Count; i += 3, j++) {
                        if (j >= brushes.Length) j -= brushes.Length;
                        int a = tris[i];
                        int b = tris[i + 1];
                        int c = tris[i + 2];
                        points[0] = new PointF(polygon[a].x, polygon[a].y);
                        points[1] = new PointF(polygon[b].x, polygon[b].y);
                        points[2] = new PointF(polygon[c].x, polygon[c].y);
                        g.FillPolygon(brushes[j], points);
                    }
                }
            }
            // 多边形的线
            for (int i = 0, j = 1; i < polygon.Count; i++, j++) {
                if (j >= polygon.Count) j -= polygon.Count;
                g.DrawLine(Pens.Red, polygon[i].x, polygon[i].y, polygon[j].x, polygon[j].y);
            }

            // 多边形的顶点
            for (int i = 0; i < polygon.Count; i++) {
                Vec point = polygon[i];
                Brush brush;
                if (i == 0) brush = Brushes.Blue;
                else brush = Brushes.Black;
                g.FillEllipse(brush, point.x - 4, point.y - 4, 8, 8);
            }
        }
    }

}
