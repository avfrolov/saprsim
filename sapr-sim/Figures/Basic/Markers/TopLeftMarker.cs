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
    public partial class TopLeftMarker : Marker
    {
        public override void UpdateLocation() { RectangleF bounds = (targetFigure as SolidFigure).Bounds; location = new Point((int)Math.Round(bounds.Left) + defaultSize / 2, (int)Math.Round(bounds.Top) + defaultSize / 2); }
        public override void Offset(float dx, float dy)
        {
            base.Offset(dx, dy); UpdateLabelLocation(dx, dy);
            (targetFigure as SolidFigure).Location = PointF.Add((targetFigure as SolidFigure).Location, new SizeF(dx / 2.0F, dy / 2.0F));
            (targetFigure as SolidFigure).Size = SizeF.Add((targetFigure as SolidFigure).Size, new SizeF(-dx, -dy));
        }
    }
}
