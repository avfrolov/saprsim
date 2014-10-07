using System;
using System.Drawing.Drawing2D;
using System.Drawing;

namespace sapr_sim.Figures.Basic
{
    [Serializable]
    public partial class RhombFigure : SolidFigure
    {
        public RhombFigure()
        {
            Path.AddPolygon(new PointF[]{
                new PointF(-defaultSize, 0),
                new PointF(0, -defaultSize/2),
                new PointF(defaultSize, 0),
                new PointF(0, defaultSize/2)
            });
            textRect = new RectangleF(-defaultSize / 2, -defaultSize / 4, defaultSize, defaultSize / 2);
        }
    }
}
