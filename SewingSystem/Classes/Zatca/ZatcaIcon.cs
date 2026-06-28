using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace SewingSystem.Classes.Zatca
{
    /// <summary>
    /// Generates a ZATCA / e-invoicing icon at runtime: a white invoice page with
    /// a teal header, text lines, and a small QR block (the Phase-2 hallmark).
    /// No external image resource needed.
    /// </summary>
    public static class ZatcaIcon
    {
        private static readonly Dictionary<int, Bitmap> _cache = new Dictionary<int, Bitmap>();
        private static readonly Color Teal = Color.FromArgb(14, 124, 123);
        private static readonly Color PageBorder = Color.FromArgb(120, 120, 120);
        private static readonly Color Line = Color.FromArgb(170, 170, 170);

        // 5x5 QR-like pattern (corners filled to mimic finder patterns)
        private static readonly int[,] Pat =
        {
            {1,1,0,1,1},
            {1,0,0,0,1},
            {0,0,1,0,0},
            {1,0,0,0,1},
            {1,1,0,1,1},
        };

        public static Bitmap Get(int size)
        {
            if (_cache.TryGetValue(size, out var cached)) return cached;
            var bmp = new Bitmap(size, size);
            using (var g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.Clear(Color.Transparent);

                float pw = size * 0.72f, ph = size * 0.86f;
                float px = (size - pw) / 2f, py = (size - ph) / 2f;
                float r = size * 0.10f;

                using (var page = Rounded(px, py, pw, ph, r))
                using (var white = new SolidBrush(Color.White))
                using (var pen = new Pen(PageBorder, System.Math.Max(1f, size * 0.04f)))
                {
                    g.FillPath(white, page);
                    g.DrawPath(pen, page);
                }
                // teal header
                using (var th = new SolidBrush(Teal))
                    g.FillRectangle(th, px + pw * 0.10f, py + ph * 0.10f, pw * 0.80f, ph * 0.14f);
                // a couple of text lines
                using (var lp = new Pen(Line, System.Math.Max(1f, size * 0.035f)))
                {
                    float lx = px + pw * 0.12f, lw = pw * 0.5f;
                    g.DrawLine(lp, lx, py + ph * 0.34f, lx + lw, py + ph * 0.34f);
                    g.DrawLine(lp, lx, py + ph * 0.45f, lx + lw * 0.8f, py + ph * 0.45f);
                }
                // mini QR (bottom area)
                float q = pw * 0.46f;
                float qx = px + pw - q - pw * 0.12f;
                float qy = py + ph - q - ph * 0.10f;
                DrawQr(g, qx, qy, q);
            }
            _cache[size] = bmp;
            return bmp;
        }

        private static void DrawQr(Graphics g, float x, float y, float s)
        {
            float cell = s / 5f;
            using (var black = new SolidBrush(Color.Black))
                for (int row = 0; row < 5; row++)
                    for (int col = 0; col < 5; col++)
                        if (Pat[row, col] == 1)
                            g.FillRectangle(black, x + col * cell, y + row * cell, cell + 0.5f, cell + 0.5f);
        }

        private static GraphicsPath Rounded(float x, float y, float w, float h, float r)
        {
            var p = new GraphicsPath();
            p.AddArc(x, y, r, r, 180, 90);
            p.AddArc(x + w - r, y, r, r, 270, 90);
            p.AddArc(x + w - r, y + h - r, r, r, 0, 90);
            p.AddArc(x, y + h - r, r, r, 90, 90);
            p.CloseFigure();
            return p;
        }
    }
}
