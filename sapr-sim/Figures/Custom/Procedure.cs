using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using sapr_sim.Figures.Basic;
using sapr_sim.Figures.Custom.Enum;
using sapr_sim.Figures.Basic.Enums;
using System.Drawing.Drawing2D;

namespace sapr_sim.Figures.Custom
{
    [Serializable]
    public class Procedure : ComplexFigure, SchemaElement
    {

        private RoundRectangle bound;
        /// <summary>
        /// port[0] - top port
        /// port[1] - left port
        /// port[2] - right port
        /// port[3] - bottom
        /// </summary>
        private Port[] ports;

        public Procedure()
        {
            bound = new RoundRectangle();
            bound.OwnerFigure = this;
            bound.Text = "Процедура";
            bound.Size = new SizeF(90F, 40F);
            bound.FigureBrush = new LinearGradientBrush(
                new PointF(-bound.Size.Width / 2, 10),
                new PointF(bound.Size.Width / 2, 10),
                Color.LemonChiffon,
                Color.FromArgb(255, 255, 250, 250));

            primitives.Add(bound);

            //down port
            ports = new Port[4];
            ports[0] = new Port(bound, this, ConnectorDirection.Input, ConnectorType.Resource, ConnectorOrientation.BottomToTop);
            (ports[0] as Port).EnableMultipleConnection = true;

            ports[1] = new Port(bound, this, ConnectorDirection.Input, ConnectorType.Entity);
            ports[2] = new Port(bound, this, ConnectorDirection.Output, ConnectorType.Entity);
            ports[3] = new Port(bound, this, ConnectorDirection.Input, ConnectorType.Resource, ConnectorOrientation.TopToBottom);
            
            primitives.Add(ports[0]);
            primitives.Add(ports[1]);
            primitives.Add(ports[2]);
            primitives.Add(ports[3]);
        }

        public override void RecalcFigure()
        {
            ports[0].Location = new PointF(
                bound.Location.X,
                bound.Location.Y + (int)bound.Size.Height / 2 + defaultPortOffset + 2);
            ports[1].Location = new PointF(
                bound.Location.X - (int)bound.Size.Width / 2 - defaultPortOffset - 2,
                bound.Location.Y);
            ports[2].Location = new PointF(
                bound.Location.X + (int)bound.Size.Width / 2,
                bound.Location.Y);
            ports[3].Location = new PointF(
                bound.Location.X,
                bound.Location.Y - (int)bound.Size.Height / 2 - defaultPortOffset - 2);
        }

        public override void Draw(Graphics gr)
        {
            RecalcFigure();
            DrawFigures(gr);
        }

    }
}
