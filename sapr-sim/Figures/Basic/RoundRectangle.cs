using System;
using System.Drawing.Drawing2D;
using System.Drawing;

namespace sapr_sim.Figures.Basic
{
    [Serializable]
    public partial class RoundRectFigure : SolidFigure
    {
        public RoundRectFigure()
        {
            float diameter = 16f;
            SizeF sizeF = new SizeF(diameter, diameter);
            RectangleF arc = new RectangleF(-defaultSize, -defaultSize / 2, sizeF.Width, sizeF.Height);
            Path.AddArc(arc, 180, 90);
            arc.X = defaultSize - diameter;
            Path.AddArc(arc, 270, 90);
            arc.Y = defaultSize / 2 - diameter;
            Path.AddArc(arc, 0, 90);
            arc.X = -defaultSize;
            Path.AddArc(arc, 90, 90);
            Path.CloseFigure();

            textRect = new RectangleF(-defaultSize + 3, -defaultSize / 2 + 2, 2 * defaultSize - 6, defaultSize - 4);
        }
    }
}
