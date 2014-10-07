using System;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Collections.Generic;

using sapr_sim.Figures.Basic;
using sapr_sim.Figures.Basic.Util;
using sapr_sim.Figures.Basic.Markers;

namespace sapr_sim.Figures.Basic.Lines
{
    [Serializable]
    public partial class Line : Figure
    {
        public IConnectable From;
        public IConnectable To;
        static Pen clickPen = new Pen(Color.Transparent, 6);

        public Line()
        {

        }

        public Line(IConnectable from, IConnectable to)
        {
            From = from; To = to;
        }

        public override void Draw(Graphics gr)
        {
            if (From == null && To == null)
            {
                if (Path.PointCount >= 2)
                    gr.DrawPath(FigurePen, Path);
                return;
            }
            else
                RecalcPath();
            gr.DrawPath(FigurePen, Path);
        }

        //смещение линии
        public override void Offset(float dx, float dy)
        {
            if (Path.PointCount > 0)
            {
                for (int i = 0; i < Path.PathPoints.Length; i++)
                {
                    Path.PathPoints[i].X += dx;
                    Path.PathPoints[i].Y += dy;
                }
                RecalcPath();
                OffsetEndMarkers(dx, dy);
            }
        }

        //смещение концевых маркеров
        protected virtual void OffsetEndMarkers(float dx, float dy)
        {
            //если какой-то из концов линии не подключен к фигуре,
            //то смещаем маркеры тоже
            if (From != null && From is Marker)
            {
                From.X += dx;
                From.Y += dy;
            }
            if (To != null && To is Marker)
            {
                To.X += dx;
                To.Y += dy;
            }
        }

        public override bool IsInsidePoint(Point p)
        {
            if (From == null || To == null)
            {
                if (Path.PointCount < 2)
                    return false;
            }
            else
                RecalcPath();
            return Path.IsOutlineVisible(p, clickPen);
        }

        //проверяет, находится ли линия в заданном прямоугольнике
        public bool IsLineInRect(RectangleF rect)
        {
            if (Path.PointCount == 0)
                return false;
            foreach (PointF p in Path.PathPoints)
            {
                if (!rect.Contains(p))//точка пути не содержится - линия не содержится
                    return false;
            }
            return true;
        }

        protected virtual void RecalcPath()
        {
            PointF[] points = null;
            if (Path.PointCount > 0)
                points = Path.PathPoints;

            if (From == null || To == null ||
                Path.PointCount != 2 || points[0] != From.Location || points[1] != To.Location)
            {
                Path.Reset();
                if ((From == null) && (To != null)) //проверяем концы линии
                {
                    Path.AddLine(points[0], To.Location);
                }
                else if ((To == null) && (From != null))
                {
                    Path.AddLine(From.Location, points[1]);
                }
                else if ((To != null) && (From != null))
                    Path.AddLine(From.Location, To.Location);
                else
                    Path.AddLine(points[0], points[1]);
            }
        }

        public override List<Marker> CreateMarkers(Diagram diagram)
        {
            List<Marker> markers = new List<Marker>();
            EndLineMarker m1 = new EndLineMarker(diagram, 0);
            m1.targetFigure = this;
            EndLineMarker m2 = new EndLineMarker(diagram, 1);
            m2.targetFigure = this;

            markers.Add(m1);
            markers.Add(m2);

            return markers;
        }
        public override RectangleF Bounds
        {
            get { return Path.GetBounds(); }
        }
        public override object Clone()
        {
            Line line = new Line();
            line.serializablePath = new SerializableGraphicsPath();
            line.Path = (GraphicsPath)this.Path.Clone();
            line.FigurePen = (Pen)this.FigurePen.Clone();
            line.Name = name;
            //если концы линии подключены к маркерам, то клонируем их. Коннекторы не копируем
            if (!(line.From is EndLineMarker))
                (line as Line).From = From;
            else
                (line as Line).From = (IConnectable)(From as SolidFigure).Clone();
            if (!(line.To is EndLineMarker))
                (line as Line).To = To;
            else
                (line as Line).To = (IConnectable)(To as SolidFigure).Clone();
            return line;
        }
    }
}
