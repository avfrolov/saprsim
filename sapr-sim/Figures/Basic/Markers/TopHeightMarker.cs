using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Runtime.Serialization;
using System.Reflection;
using System.ComponentModel;

namespace sapr_sim.Figures.Basic.Markers
{
    public partial class TopHeightMarker : HeightMarker
    {
        public override void Offset(float dx, float dy)
        {
            base.Offset(0, dy);
            (targetFigure as SolidFigure).Location = PointF.Add(
                (targetFigure as SolidFigure).Location, new SizeF(0, dy / 2.0F));
            (targetFigure as SolidFigure).Size =
                SizeF.Add((targetFigure as SolidFigure).Size, new SizeF(0, -dy));
            UpdateLabelLocation(0, dy);
        }
        public override void UpdateLocation()
        {
            RectangleF bounds = (targetFigure as SolidFigure).Bounds;
            location = new PointF(
                (float)(targetFigure as SolidFigure).Location.X,
                (float)Math.Round(bounds.Top) + defaultSize / 2);
        }
    }
}
