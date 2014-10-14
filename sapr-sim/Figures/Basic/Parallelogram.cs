using System;
using System.Drawing.Drawing2D;
using System.Drawing;

namespace sapr_sim.Figures.Basic
{
    [Serializable]
    public partial class Parallelogramm : SolidFigure
    {
        public Parallelogramm()
        {
            float shift = 8f;
            Path.AddPolygon(new PointF[]{
                new PointF(-defaultSize + shift/2, -defaultSize/2),
                new PointF(defaultSize + shift/2, -defaultSize/2),
                new PointF(defaultSize - shift/2, defaultSize/2),
                new PointF(-defaultSize - shift/2, defaultSize/2),
            });
            textRect = new RectangleF(-defaultSize + shift / 2, -defaultSize / 2 + 2, 2 * defaultSize - shift, defaultSize - 4);
        }
    }
}
