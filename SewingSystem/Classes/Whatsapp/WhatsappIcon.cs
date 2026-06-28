using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace SewingSystem.Classes.Whatsapp
{
    /// <summary>
    /// Generates a simple WhatsApp-style icon at runtime (green circle + white
    /// phone glyph) so no external image resource is needed.
    /// </summary>
    public static class WhatsappIcon
    {
        private static readonly Dictionary<int, Bitmap> _cache = new Dictionary<int, Bitmap>();
        private static readonly Color Green = Color.FromArgb(37, 211, 102); // WhatsApp green

        public static Bitmap Get(int size)
        {
            if (_cache.TryGetValue(size, out var cached)) return cached;
            var bmp = new Bitmap(size, size);
            using (var g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                g.Clear(Color.Transparent);

                // WhatsApp-style chat bubble = circle + a small tail at the bottom-left.
                float pad = size * 0.05f;
                float d = size - pad * 2;          // bubble diameter
                float cx = pad + d / 2f, cy = pad + d / 2f;
                using (var path = new GraphicsPath())
                using (var br = new SolidBrush(Green))
                {
                    path.AddEllipse(pad, pad, d, d);
                    // tail: small triangle pointing down-left out of the circle
                    path.AddPolygon(new[]
                    {
                        new PointF(cx - d * 0.36f, cy + d * 0.18f),
                        new PointF(pad + d * 0.04f, size - pad * 0.5f),
                        new PointF(cx - d * 0.04f, cy + d * 0.40f)
                    });
                    g.FillPath(br, path);
                }

                // white telephone handset glyph (✆) centered inside the bubble
                using (var f = new Font("Segoe UI Symbol", d * 0.52f, FontStyle.Regular, GraphicsUnit.Pixel))
                using (var wb = new SolidBrush(Color.White))
                {
                    var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                    g.DrawString("✆", f, wb, new RectangleF(pad, pad, d, d), sf); // ✆ U+2706 handset
                }
            }
            _cache[size] = bmp;
            return bmp;
        }
    }
}
