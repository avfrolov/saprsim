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
    public partial class RightWidthMarker : WidthMarker
    {
        public override void Offset(float dx, float dy)
        {
            base.Offset(dx, 0);
            (targetFigure as SolidFigure).Location = PointF.Add(
                (targetFigure as SolidFigure).Location, new SizeF(dx / 2.0F, 0));
            (targetFigure as SolidFigure).Size =
                SizeF.Add((targetFigure as SolidFigure).Size, new SizeF(dx, 0));
            UpdateLabelLocation(dx, 0);
        }
        public override void UpdateLocation()
        {
            RectangleF bounds = (targetFigure as SolidFigure).Bounds;
            location = new PointF((float)Math.Round(bounds.Right) - defaultSize / 2, (float)(targetFigure as SolidFigure).Location.Y);
        }
    }
}
