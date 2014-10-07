using System;
using System.Drawing.Drawing2D;
using System.Drawing;

namespace sapr_sim.Figures.Basic
{
    [Serializable]
    public partial class EllipseFigure : SolidFigure
    {
        public EllipseFigure()
        {
            Path.AddEllipse(new RectangleF(-defaultSize, -defaultSize / 2, defaultSize * 2, defaultSize));
            textRect = new RectangleF(-defaultSize / 1.4f, -defaultSize / 2 / 1.4f, 2 * defaultSize / 1.4f, defaultSize / 1.4f);
        }
    }
}
