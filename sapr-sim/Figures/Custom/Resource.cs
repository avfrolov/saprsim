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
    public class Resource : ComplexFigure, SchemaElement
    {
    
        private sapr_sim.Figures.Basic.Rectangle bound;

        private static float defaultRectH = 3;

        public Resource()
        {
            bound = new sapr_sim.Figures.Basic.Rectangle();
            bound.OwnerFigure = this;
            bound.Text = "Ресурс";
            bound.FigureBrush = new LinearGradientBrush(
               new PointF(-bound.Size.Width / 2, 10),
               new PointF(bound.Size.Width / 2, 10),
               Color.LemonChiffon,
               Color.FromArgb(255, 255, 250, 250));

            primitives.Add(bound);

            Port topPort = new Port(bound, this, ConnectorDirection.Output, ConnectorType.Resource, ConnectorOrientation.BottomToTop);
            topPort.EnableMultipleConnection = true;
            primitives.Add(topPort);

            for (int i = 0; i < 6; i++)
            {
                sapr_sim.Figures.Basic.Rectangle r = new sapr_sim.Figures.Basic.Rectangle();              
                r.FigureBrush = new SolidBrush(Color.FromArgb(255, 40, 40, 40));
                r.FigurePen = Pens.Transparent;
                r.ResizeDirection = ResizeDirection.None;
                primitives.Add(r);
            }

            primitives.Add(new Port(bound, this, ConnectorDirection.Output, ConnectorType.Resource, ConnectorOrientation.TopToBottom));
        }

        public override void RecalcFigure()
        {

            primitives[2].Size = primitives[3].Size = new SizeF(bound.Size.Width, defaultRectH);
            primitives[2].Location = new PointF(bound.Location.X, bound.Location.Y + (int)bound.Size.Height / 2 - defaultRectH / 2);
            primitives[3].Location = new PointF(bound.Location.X, bound.Location.Y - (int)bound.Size.Height / 2 + defaultRectH / 2);

            primitives[4].Size = primitives[5].Size = primitives[6].Size = primitives[7].Size = 
                new SizeF(defaultRectH, primitives[0].Size.Height / 4);
            primitives[4].Location = new PointF(bound.Location.X - bound.Size.Width / 2 + defaultRectH / 2,
                bound.Location.Y - bound.Size.Height / 2 + bound.Size.Height / 8);
            primitives[5].Location = new PointF(bound.Location.X + bound.Size.Width / 2 - defaultRectH / 2,
                bound.Location.Y - bound.Size.Height / 2 + bound.Size.Height / 8);
            primitives[6].Location = new PointF(bound.Location.X - bound.Size.Width / 2 + defaultRectH / 2,
                bound.Location.Y + bound.Size.Height / 2 - bound.Size.Height / 8);
            primitives[7].Location = new PointF(bound.Location.X + bound.Size.Width / 2 - defaultRectH / 2,
                bound.Location.Y + bound.Size.Height / 2 - bound.Size.Height / 8);
                       
            primitives[1].Location = new PointF(
                primitives[0].Location.X,
                primitives[0].Location.Y - (int)bound.Size.Height / 2);

            primitives[8].Location = new PointF(
                primitives[0].Location.X,
                primitives[0].Location.Y + (int)bound.Size.Height / 2);
        }

        public override void Draw(Graphics gr)
        {
            RecalcFigure();
            DrawFigures(gr);
        }
    }
}
