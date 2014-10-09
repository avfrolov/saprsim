using System;
using System.Drawing.Drawing2D;
using System.Drawing;

namespace sapr_sim.Figures.Basic
{
    [Serializable]
    public partial class Rectangle: SolidFigure
    {
        public Rectangle()
        {
            Path.AddRectangle(new RectangleF(-defaultSize, -defaultSize / 2, 2 * defaultSize, defaultSize));
            textRect = new RectangleF(-defaultSize + 3, -defaultSize / 2 + 2, 2 * defaultSize - 6, defaultSize - 4);
        }
    }
}
