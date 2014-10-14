using sapr_sim.Figures.Basic;
using sapr_sim.Figures.Basic.Enums;
using sapr_sim.Figures.Custom.Enum;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sapr_sim.Figures.Custom
{
    class Collector : ComplexFigure, SchemaElement
    {

        private Rhomb bound;
        private Port[] ports;

        public Collector()
        {
            bound = new Rhomb();
            bound.Size = new SizeF(36F, 36F);
            bound.Text = "";
            bound.TextChangeEnable = false;
            bound.FigureBrush = new LinearGradientBrush(
               new PointF(-bound.Size.Width / 2, 10),
               new PointF(bound.Size.Width / 2, 10),
               Color.LemonChiffon,
               Color.FromArgb(255, 255, 250, 250));
            primitives.Add(bound);

            MainFigureIndex = 0;

            ports = new Port[4];
            ports[0] = new Port(primitives[MainFigureIndex], this, ConnectorDirection.Input, ConnectorType.Entity);
            ports[1] = new Port(primitives[MainFigureIndex], this, ConnectorDirection.Input, ConnectorType.Entity, ConnectorOrientation.TopToBottom);
            ports[2] = new Port(primitives[MainFigureIndex], this, ConnectorDirection.Input, ConnectorType.Entity, ConnectorOrientation.BottomToTop);
            ports[3] = new Port(primitives[MainFigureIndex], this, ConnectorDirection.Output, ConnectorType.Entity);
            primitives.Add(ports[0]);
            primitives.Add(ports[1]);
            primitives.Add(ports[2]);
            primitives.Add(ports[3]);

            RecalcFigure();
        }

        public override void RecalcFigure()
        {
            ports[0].Location = new PointF(bound.Location.X - (int)bound.Size.Width / 2 - defaultPortOffset, bound.Location.Y);
            ports[1].Location = new PointF(ports[1].Location.X, bound.Location.Y - (int)bound.Size.Height / 2 - defaultPortOffset);
            ports[2].Location = new PointF(ports[1].Location.X, bound.Location.Y + (int)bound.Size.Height / 2 + defaultPortOffset);
            ports[3].Location = new PointF(bound.Location.X + (int)bound.Size.Width / 2, bound.Location.Y);
        }
    }
}
