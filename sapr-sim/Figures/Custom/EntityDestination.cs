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
    [Serializable]
    public class EntityDestination : ComplexFigure, SchemaElement
    {

        private Label label;
        private Ellipse bound;
        private Port port;
        private Ellipse externalBound; 

        public EntityDestination()
        {
            label = new Label("Сток сущностей");
            label.OwnerFigure = this;
            primitives.Add(label);

            bound = new Ellipse();
            bound.Size = new SizeF(24F, 24F);
            bound.FigurePen = new Pen(Brushes.Transparent);
            bound.Text = "";
            bound.TextChangeEnable = false;
            bound.ResizeDirection = ResizeDirection.None;
            bound.Location = new PointF(label.Location.X, label.Location.Y + (int)label.Size.Height / 2 +
                DefaultLabelOffset + (int)bound.Size.Height / 2);
            bound.FigureBrush = new LinearGradientBrush(
               new PointF(-bound.Size.Width / 2, 10),
               new PointF(bound.Size.Width / 2, 10),
               Color.DarkRed,
               Color.FromArgb(255, 255, 50, 50));
            primitives.Add(bound);

            MainFigureIndex = 1;

            port = new Port(bound, this, ConnectorDirection.Input, ConnectorType.Entity);
            primitives.Add(port);

            externalBound = new Ellipse();
            externalBound.Size = new SizeF(30F, 30F);
            externalBound.FigurePen = Pens.DarkRed;
            externalBound.FigureBrush = Brushes.Transparent;
            externalBound.ResizeDirection = ResizeDirection.None;
            primitives.Add(externalBound);

            RecalcFigure();
        }

        public override void RecalcFigure()
        {
            port.Location = new PointF(externalBound.Location.X - (int)externalBound.Size.Width / 2 - defaultPortOffset, externalBound.Location.Y);
            externalBound.Location = bound.Location;
        }

        public override RectangleF MainFigureBounds
        {
            get { return externalBound.Bounds; }
        }

        public override Pen FigurePen
        {
            get { return bound.FigurePen; }
            set 
            {
                bound.FigurePen = value;
                externalBound.FigurePen = value;
            }
        }
    }
}
