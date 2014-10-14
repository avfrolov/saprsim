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

using sapr_sim.Figures.Basic.Lines;
using sapr_sim.Figures.Basic;
using sapr_sim.Figures.Basic.Enums;
using sapr_sim.Figures.Basic.Util;
using sapr_sim.Figures.Basic.Markers;
using sapr_sim.Figures.Custom;

namespace sapr_sim.Figures.Basic.Lines
{
    [Serializable]
    public partial class LedgeLine : Line
    {
        /// <summary>
        /// Отступ от границ блоков при автопостроении линии
        /// </summary>
        private static int blockOffset = 10;
        //список точек перелома 
        public List<LedgePoint> ledgePositions = new List<LedgePoint>();

        protected bool IsBothFiguresOrientationHorizontal()
        {
            if (To.Orientation < ConnectorOrientation.TopToBottom
                && From.Orientation < ConnectorOrientation.TopToBottom)
                return true;
            return false;
        }
        protected bool IsBothFiguresOrientationVertical()
        {
            if (To.Orientation >= ConnectorOrientation.TopToBottom
                && From.Orientation >= ConnectorOrientation.TopToBottom)
                return true;
            return false;
        }
        protected void AddNewLedge(int index, float coord, LedgeDirection dir)
        {
            LedgePoint lp = new LedgePoint();
            lp.coodinate = coord;
            lp.dir = dir;
            if (index == ledgePositions.Count)
                ledgePositions.Add(lp);
            else if (index >= 0)
                ledgePositions.Insert(index, lp);
        }
        protected void AddLedgeForSameOrientation()
        {
            if (IsBothFiguresOrientationHorizontal())
                AddNewLedge(0, (From.Location.X + To.Location.X) / 2,
                    LedgeDirection.Vertical);
            else if (IsBothFiguresOrientationVertical())
                AddNewLedge(0, (From.Location.Y + To.Location.Y) / 2,
                    LedgeDirection.Horizontal);
        }
        protected override void RecalcPath()
        {
            PointF[] points = null;
            int lastLedgeIndex = ledgePositions.Count - 1;
            if (To != null && From != null)
            {
                //если число точек нулевое и при этом ориентация маркеров одинакова, добавляем маркер
                if (ledgePositions.Count == 0)
                    AddLedgeForSameOrientation();
                //удаляем лишние маркеры, которые лежат на 1й линии
                if (lastLedgeIndex > 1)
                {
                    if ((ledgePositions[lastLedgeIndex].dir == LedgeDirection.Horizontal &&
                        ledgePositions[lastLedgeIndex].coodinate == To.Location.Y)
                        || (ledgePositions[lastLedgeIndex].dir == LedgeDirection.Vertical &&
                        ledgePositions[lastLedgeIndex].coodinate == To.Location.X))
                        ledgePositions.RemoveAt(lastLedgeIndex);
                }
            }
            if (Path.PointCount > 0)
                points = Path.PathPoints;
            if (To == null || From == null ||
                Path.PointCount != (3 + ledgePositions.Count) ||
                points[0] != From.Location ||
                points[points.Length - 1] != To.Location ||
                !LedgePositionsOK(points))
            {

                //проверяем, чтобы направления изломов чередовались
                for (int i = 0; i < ledgePositions.Count - 1; i++)
                {
                    if (ledgePositions[i].dir == ledgePositions[i + 1].dir)
                    {
                        //удаляем следующую позицию
                        ledgePositions.RemoveAt(i + 1);
                    }
                }
                //создаем новую линию с изломом
                List<PointF> linePoints = new List<PointF>();
                //подготавливаем позиции точек
                PointF fromLocation, toLocation;
                if (From != null)
                    fromLocation = From.Location;
                else
                    fromLocation = points[0];
                if (To != null)
                    toLocation = To.Location;
                else
                    toLocation = points[points.Length - 1];
                //первая точка - стартовая
                linePoints.Add(fromLocation);
                //вторая точка
                if (ledgePositions.Count > 0)
                {
                    if (ledgePositions[0].dir == LedgeDirection.Vertical)
                        linePoints.Add(new PointF(ledgePositions[0].coodinate, fromLocation.Y));
                    else
                        linePoints.Add(new PointF(fromLocation.X, ledgePositions[0].coodinate));
                    //генерируем все промежуточные точки
                    for (int i = 0; i < ledgePositions.Count - 1; i++)
                    {
                        if (ledgePositions[i].dir == LedgeDirection.Vertical)
                            linePoints.Add(new PointF(ledgePositions[i].coodinate,
                                ledgePositions[i + 1].coodinate));
                        else
                            linePoints.Add(new PointF(ledgePositions[i + 1].coodinate,
                                 ledgePositions[i].coodinate));
                    }
                    //предпоследняя точка
                    if (ledgePositions[ledgePositions.Count - 1].dir == LedgeDirection.Vertical)
                        linePoints.Add(new PointF(ledgePositions[ledgePositions.Count - 1].coodinate, toLocation.Y));
                    else
                        linePoints.Add(new PointF(toLocation.X, ledgePositions[ledgePositions.Count - 1].coodinate));
                }
                else if (From != null)//точек перелома нет. Проверим, куда ставить угол
                {

                    if (From.Orientation <= ConnectorOrientation.RightToLeft)
                        //горизонтальная
                        linePoints.Add(new PointF(toLocation.X, fromLocation.Y));
                    else
                        linePoints.Add(new PointF(fromLocation.X, toLocation.Y));
                }
                //последняя точка
                linePoints.Add(toLocation);
                //соединяем точки линиями
                Path.Reset();
                for (int i = 0; i < linePoints.Count - 1; i++)
                    Path.AddLine(linePoints[i], linePoints[i + 1]);
            }
        }
        private bool RightConnectionOK(IConnectable compareFigure, float x0, float x1)
        {
            float left = compareFigure.MainFigure.Bounds.Left;
            float right = compareFigure.MainFigure.Bounds.Right;
            if (x0 + 4 >= right && x0 > left && x1 + 4 >= right && x1 > left)
                return true;
            return false;
        }
        private bool LeftConnectionOK(IConnectable compareFigure, float x0, float x1)
        {
            float left = compareFigure.MainFigure.Bounds.Left;
            float right = compareFigure.MainFigure.Bounds.Right;
            if (x0 - 4 <= left && x0 < right && x1 - 4 <= left && x1 < right)
                return true;
            return false;
        }
        private bool BottomConnectionOK(IConnectable compareFigure, float y0, float y1)
        {
            float top = compareFigure.MainFigure.Bounds.Top;
            float bottom = compareFigure.MainFigure.Bounds.Bottom;
            if (y0 + 4 >= bottom && y0 > top && y1 + 4 >= bottom && y1 > top)
                return true;
            return false;
        }
        private bool TopConnectionOK(IConnectable compareFigure, float y0, float y1)
        {
            float top = compareFigure.MainFigure.Bounds.Top;
            float bottom = compareFigure.MainFigure.Bounds.Bottom;
            if (y0 - 4 <= top && y0 < bottom && y1 - 4 <= top && y1 < bottom)
                return true;
            return false;
        }
        private bool CheckLineEndHorizontal(IConnectable compareFigure, int insert_index, int delta_index, int prev_point_id, int next_point_id, float delta_Y, bool AddLedges)
        {
            bool result = true;
            PointF[] points = Path.PathData.Points;
            float new_Y_coord;
            if (delta_Y > 0)
                new_Y_coord = compareFigure.MainFigure.Bounds.Bottom + delta_Y;
            else
                new_Y_coord = compareFigure.MainFigure.Bounds.Top + delta_Y;
            //проверяем правый порт
            if (compareFigure.Orientation <= ConnectorOrientation.RightToLeft &&
                compareFigure.Location.X + 4 >= compareFigure.MainFigure.Bounds.Right)
            {
                if (!RightConnectionOK(compareFigure, points[prev_point_id].X, points[next_point_id].X))
                {
                    if (AddLedges)
                    {
                        AddNewLedge(insert_index, new_Y_coord, LedgeDirection.Horizontal);
                        AddNewLedge(insert_index + delta_index, compareFigure.MainFigure.Bounds.Right + blockOffset, LedgeDirection.Vertical);
                    }
                    result = false;
                }
            }
            //проверяем левый порт
            if (compareFigure.Orientation <= ConnectorOrientation.RightToLeft &&
                compareFigure.Location.X - 4 <= compareFigure.MainFigure.Bounds.Left)
            {
                if (!LeftConnectionOK(compareFigure, points[prev_point_id].X, points[next_point_id].X))
                {
                    if (AddLedges)
                    {
                        AddNewLedge(insert_index, new_Y_coord, LedgeDirection.Horizontal);
                        AddNewLedge(insert_index + delta_index, compareFigure.MainFigure.Bounds.Left - blockOffset, LedgeDirection.Vertical);
                    }
                    result = false;
                }
            }
            return result;
        }
        private bool CheckLineEndVertical(IConnectable compareFigure, int insert_index, int delta_index, int prev_point_id, int next_point_id, float delta_X, bool AddLedges)
        {
            bool result = true;
            PointF[] points = Path.PathData.Points;
            float new_X_coord;
            if (delta_X > 0)
                new_X_coord = compareFigure.MainFigure.Bounds.Right + delta_X;
            else
                new_X_coord = compareFigure.MainFigure.Bounds.Left + delta_X;
            //проверяем нижний порт
            if (compareFigure.Orientation > ConnectorOrientation.RightToLeft &&
                compareFigure.Location.Y + 4 >= compareFigure.MainFigure.Bounds.Bottom)
            {
                if (!BottomConnectionOK(compareFigure, points[prev_point_id].Y, points[next_point_id].Y))
                {
                    if (AddLedges)
                    {
                        AddNewLedge(insert_index, new_X_coord, LedgeDirection.Vertical);
                        AddNewLedge(insert_index + delta_index, compareFigure.MainFigure.Bounds.Bottom + blockOffset, LedgeDirection.Horizontal);
                    }
                    result = false;
                }
            }
            //проверяем верхний порт
            if (compareFigure.Orientation > ConnectorOrientation.RightToLeft &&
                compareFigure.Location.Y - 4 <= compareFigure.MainFigure.Bounds.Top)
            {
                if (!TopConnectionOK(compareFigure, points[prev_point_id].Y, points[next_point_id].Y))
                {
                    if (AddLedges)
                    {
                        AddNewLedge(insert_index, new_X_coord, LedgeDirection.Vertical);
                        AddNewLedge(insert_index + delta_index, compareFigure.MainFigure.Bounds.Top - blockOffset, LedgeDirection.Horizontal);
                    }
                    result = false;
                }
            }
            return result;
        }
        //проверяет, не пересекает ли линия фигуры
        //а также достраивает изломы, если это нарушается и AddLedges == true
        protected bool CheckIntersections(bool AddLedges)
        {
            int last = Path.PathData.Points.Length - 1;
            if (last < 1)
                return false;
            bool result = true;
            //проверяем начало линии
            float delta_Y = (From.Y < To.Y ? blockOffset : -blockOffset);
            float delta_X = (From.X < To.X ? blockOffset : -blockOffset);
            if (From.MainFigure != null)
            {
                result = CheckLineEndHorizontal(From, 0, 0, 0, 1, delta_Y, AddLedges);
                if (!result && !AddLedges)
                    return false;
                result = CheckLineEndVertical(From, 0, 0, 0, 1, delta_X, AddLedges);
                if (AddLedges)
                    RecalcPath();
                else if (!result)
                    return false;
            }
            delta_X = -delta_X;
            delta_Y = -delta_Y;
            //проверяем конец линии
            if (To.MainFigure != null)
            {
                result = CheckLineEndHorizontal(To, ledgePositions.Count, 1, last - 1, last, delta_Y, AddLedges);
                if (!result && !AddLedges)
                    return false;
                result = CheckLineEndVertical(To, ledgePositions.Count, 1, last - 1, last, delta_X, AddLedges);
            }
            return result;
        }
        /// <summary>
        /// Перестраивает линию оптимальным образом.
        /// Вызывать при новом подключении линии к фигурам / при движении фигур
        /// </summary>
        public void RebuildLineOptimal()
        {
            //Если какой-либо конец не подключен, фукнция не имеет смысла
            if (From == null || To == null)
                return;
            //Перестраиваем линию, только если наблюдаются нарушения
            //***(закомментировано - это нетипичное, но возможное поведение)***
            /*if (!CheckIntersections(false))
            {*/
            ledgePositions.Clear();
            //добавляем излом, если у выходных портов одинаковая ориентация
            AddLedgeForSameOrientation();
            //проверка на тот случай, если первая/последняя линия пересекает фигуру
            RecalcPath();

            int Q = ledgePositions.Count;
            CheckIntersections(true);
            //если добавили с обоих концов по 2 маркера, удалим средний
            //но это в том случае, если маркер 2 не находится на уровне
            //второй фигуры
            bool isOK = false;
            if (IsBothFiguresOrientationHorizontal() || IsBothFiguresOrientationVertical())
                isOK = true;
            if (ledgePositions.Count > 1 && To.MainFigure != null)
            {
                if (ledgePositions[1].dir == LedgeDirection.Horizontal)
                {
                    if (ledgePositions[1].coodinate >= To.MainFigure.Bounds.Top &&
                        ledgePositions[1].coodinate <= To.MainFigure.Bounds.Bottom)
                        isOK = false;
                }
                else
                {
                    if (ledgePositions[1].coodinate >= To.MainFigure.Bounds.Left &&
                       ledgePositions[1].coodinate <= To.MainFigure.Bounds.Right)
                        isOK = false;
                }
            }
            //собственно, удаление маркера
            if (ledgePositions.Count - Q == 4 && isOK)
                ledgePositions.RemoveAt(Q + 1);
            RecalcPath();
            //провека, если линия выгнута дугой: в этом случае ее нужно отодвинуть от коннекторов
            BuildArc();
            // }
        }
        protected void BuildArc()
        {
            int delta = 2 * blockOffset;
            if (Path.PointCount == 4)
            {
                if (ledgePositions[0].dir == LedgeDirection.Horizontal
                    && Math.Abs(Path.PathPoints[0].Y - Path.PathPoints[1].Y) < delta
                    && Math.Abs(Path.PathPoints[2].Y - Path.PathPoints[3].Y) < delta)
                {
                    if (To.Orientation == ConnectorOrientation.TopToBottom && To.Direction == ConnectorDirection.Input ||
                        To.Orientation == ConnectorOrientation.BottomToTop && To.Direction == ConnectorDirection.Output)
                        delta *= -1;
                    ledgePositions[0].coodinate += delta;
                }
            }
        }
        //определяет, совпадают ли позиции точек перелома с 
        //координатами линии, заданными в GraphicPath'e
        protected bool LedgePositionsOK(PointF[] points)
        {
            LedgePoint p;
            for (int i = 0; i < ledgePositions.Count; i++)
            {
                p = ledgePositions[i];
                if (p.dir == LedgeDirection.Vertical)
                {
                    if (p.coodinate != points[i + 1].X)
                        return false;
                }
                else
                {
                    if (p.coodinate != points[i + 1].Y)
                        return false;
                }
            }
            return true;
        }
        public override List<Marker> CreateMarkers(Diagram diagram)
        {
            if (From != null && To != null)
                RecalcPath();
            List<Marker> markers = new List<Marker>();
            //маркеры концов линии
            EndLineMarker m1 = new EndLineMarker(diagram, 0);
            m1.targetFigure = this;
            EndLineMarker m2 = new EndLineMarker(diagram, 1);
            m2.targetFigure = this;
            markers.Add(m1);
            markers.Add(m2);
            LedgeMarker m3;
            //маркеры переломов
            for (int i = 0; i < ledgePositions.Count; i++)
            {
                m3 = new LedgeMarker(i);
                m3.targetFigure = this;
                m3.UpdateLocation();
                markers.Add(m3);
            }
            return markers;
        }
        bool isIConnectableVertical(IConnectable fig)
        {
            if (fig.Orientation == ConnectorOrientation.BottomToTop ||
                fig.Orientation == ConnectorOrientation.TopToBottom)
                return true;
            return false;
        }
        public override void Offset(float dx, float dy)
        {
            if (Path.PointCount > 0)
            {
                for (int i = 0; i < Path.PathPoints.Length; i++)
                {
                    Path.PathPoints[i].X += dx;
                    Path.PathPoints[i].Y += dy;
                }
                //сдвиг всех точек перелома
                LedgePoint p;
                for (int i = 0; i < ledgePositions.Count; i++)
                {
                    p = ledgePositions[i];
                    if (p.dir == LedgeDirection.Vertical)
                        p.coodinate += dx;
                    else
                        p.coodinate += dy;
                }
                OffsetEndMarkers(dx, dy);
                RecalcPath();
            }
        }
        public override object Clone()
        {
            LedgeLine line = new LedgeLine();
            line.Name = Name;
            line.serializablePath = new SerializableGraphicsPath();
            line.FigurePen = (Pen)(this as Figure).FigurePen.Clone();
            line.Path = (GraphicsPath)(this as Figure).Path.Clone();
            for (int i = 0; i < ledgePositions.Count; i++)
                line.ledgePositions.Add(ledgePositions[i].Clone() as LedgePoint);
            line.To = To;
            line.From = From;
            return line as Object;
        }
    }
}
