using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;

using sapr_sim.Figures.Basic;
using sapr_sim.Figures.Custom.Enum;
using sapr_sim.Figures.Basic.Enums;

namespace sapr_sim.Figures.Custom
{
    [Serializable]
    public class Sync : ComplexFigure, SchemaElement
    {

        private Label label;
        private sapr_sim.Figures.Basic.Rectangle bound;
        private Port[] ports;

        public Sync()
        {
            label = new Label("Синхронизация");
            label.OwnerFigure = this;
            primitives.Add(label);

            bound = new sapr_sim.Figures.Basic.Rectangle();
            bound.Size = new SizeF(8F, 60F);
            bound.FigurePen = new Pen(Brushes.Transparent);
            bound.Text = "";
            bound.TextChangeEnable = false;
            bound.ResizeDirection = ResizeDirection.Vertical;
            bound.OwnerFigure = this;
            bound.FigureBrush = new LinearGradientBrush(
               new PointF(10, -bound.Size.Height / 2),
               new PointF(10,  bound.Size.Height / 2),
               Color.Black,
               Color.FromArgb(255, 50, 50, 50));
            primitives.Add(bound);

            MainFigureIndex = 1;

            ports = new Port[3];
            ports[0] = new Port(bound, this, ConnectorDirection.Output, ConnectorType.Entity);
            ports[1] = new Port(bound, this, ConnectorDirection.Input, ConnectorType.Entity);
            ports[2] = new Port(bound, this, ConnectorDirection.Input, ConnectorType.Entity);
            primitives.Add(ports[0]);
            primitives.Add(ports[1]);
            primitives.Add(ports[2]);

            bound.Location = new PointF(label.Location.X, label.Location.Y + (int)label.Size.Height / 2 +
                defaultLabelOffset + (int)bound.Size.Height / 2);

            RecalcFigure();
        }

        public override void RecalcFigure()
        {
            ports[0].Location = new PointF(bound.Location.X + (int)bound.Size.Width / 2, bound.Location.Y);
            ports[1].Location = new PointF(bound.Location.X - (int)bound.Size.Width / 2 - defaultPortOffset, bound.Location.Y - (int)bound.Size.Height / 4);
            ports[2].Location = new PointF(bound.Location.X - (int)bound.Size.Width / 2 - defaultPortOffset, bound.Location.Y + (int)bound.Size.Height / 4);
        }

        public override Pen FigurePen
        {
            get { return Pens.Black; }
            set { }
        }

    }
}
