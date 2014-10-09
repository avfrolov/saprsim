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

using sapr_sim.Figures.Basic;

namespace sapr_sim.Figures.Basic.Markers
{
    [Serializable]
    public abstract class Marker : SolidFigure
    {
        protected static new int defaultSize = 2;
        public Figure targetFigure;

        public override bool IsInsidePoint(Point p)
        {
            if (p.X < location.X - defaultSize || p.X > location.X + defaultSize)
                return false;
            if (p.Y < location.Y - defaultSize || p.Y > location.Y + defaultSize)
                return false;

            return true;
        }

        public override void Draw(Graphics gr)
        {
            gr.DrawRectangle(Pens.Black, location.X - defaultSize, location.Y - defaultSize, defaultSize * 2, defaultSize * 2);
            gr.FillRectangle(Brushes.BlueViolet, location.X - defaultSize, location.Y - defaultSize, defaultSize * 2, defaultSize * 2);
        }
        //функция обновляет положение метки, если изменяемая фигура является
        //частью сложной фигуры с надписью
        public void UpdateLabelLocation(float dx, float dy)
        {
            if (targetFigure is SolidFigure)
            {
                if (!((targetFigure as SolidFigure).OwnerFigure is ComplexFigure))
                    return;
                ComplexFigure CF = ((targetFigure as SolidFigure).OwnerFigure as ComplexFigure);
                PointF labelP;
                foreach (SolidFigure fig in CF.SolidFigures)
                {
                    if (fig is Label)
                    {
                        Label label = (fig as Label);
                        //пересчитываем положение рамки, если маркер на нее наехал
                        labelP = label.Location;
                        RectangleF testBounds = (targetFigure as SolidFigure).Bounds;
                        testBounds.Width += Math.Abs(4 * dx);
                        testBounds.Height += Math.Abs(4 * dy);
                        testBounds.X -= Math.Abs(2 * dx);
                        testBounds.Y -= Math.Abs(2 * dy);
                        if (label.Bounds.IntersectsWith(testBounds) && label.IsInsidePoint(new Point((int)labelP.X, (int)(location.Y + 2 * defaultSize * dy / Math.Abs(dy)))))//наехал по оси Y
                            label.Location = new PointF(label.Location.X, label.Location.Y + dy);
                            //label.Location.Y += dy;
                        if (label.Bounds.IntersectsWith(testBounds) && label.IsInsidePoint(new Point((int)(location.X + 2 * defaultSize * dx / Math.Abs(dx)), (int)labelP.Y)))//наехал по оси X
                            label.Location = new PointF(label.Location.X + dx, label.Location.Y);
                            //label.Location.X += dx;
                    }
                }
            }
        }
        public abstract void UpdateLocation();
    }
}
