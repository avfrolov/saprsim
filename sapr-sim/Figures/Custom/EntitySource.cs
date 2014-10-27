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
    public class EntitySource : ComplexFigure, SchemaElement
    {

        private Label label;
        private Ellipse bound;
        private Port port;

        public EntitySource()
        {
            label = new Label("Источник сущностей");           
            label.OwnerFigure = this;
            primitives.Add(label);

            bound = new Ellipse();
            bound.Size = new SizeF(27F, 27F);
            bound.FigurePen = Pens.Transparent;
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
            port = new Port(bound, this, ConnectorDirection.Output, ConnectorType.Entity);
            RecalcFigure();
        }

        public override void RecalcFigure()
        {
            port.Location = new PointF(bound.Location.X + (int)bound.Size.Width / 2, bound.Location.Y);
        }

        public override Pen FigurePen
        {
            get { return bound.FigurePen.Color != Color.Transparent ? bound.FigurePen : Pens.Black; }
            set { bound.FigurePen = value; }
        }
    }
}
