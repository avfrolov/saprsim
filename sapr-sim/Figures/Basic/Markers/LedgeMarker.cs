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

namespace sapr_sim.Figures.Basic.Markers
{
    [Serializable]
    public partial class LedgeMarker : Marker
    {
        public int ledgePointIndex;//индекс точки, с которой маркер связан

        public LedgeMarker(int ledge_point_id)
        {
            ledgePointIndex = ledge_point_id;
        }
        public override void UpdateLocation()
        {
            float x, y;//новые координаты
            LedgeLine line = (targetFigure as LedgeLine);
            if (line.From == null || line.To == null)
                return;//не обновляем маркеры оторванных концов
            if (ledgePointIndex >= line.ledgePositions.Count)
                return;
            //фигура, с которой связана линия
            if (line.ledgePositions[ledgePointIndex].dir == LedgeDirection.Vertical)
            {
                x = line.ledgePositions[ledgePointIndex].coodinate;
                //первая "половина" координаты Y - предыдущая точка на линии
                if (ledgePointIndex < 1)
                    y = line.From.Location.Y;
                else
                    y = line.ledgePositions[ledgePointIndex - 1].coodinate;
                //вторая "половина" Y - следующая точка на линии
                if (ledgePointIndex == line.ledgePositions.Count - 1)
                    y = (y + line.To.Location.Y) / 2;
                else
                    y = (y + line.ledgePositions[ledgePointIndex + 1].coodinate) / 2;
            }
            else
            {
                y = line.ledgePositions[ledgePointIndex].coodinate;
                //первая "половина" координаты X - предыдущая точка на линии
                if (ledgePointIndex < 1)
                    x = line.From.Location.X;
                else
                    x = line.ledgePositions[ledgePointIndex - 1].coodinate;
                //вторая "половина" X - следующая точка на линии
                if (ledgePointIndex == line.ledgePositions.Count - 1)
                    x = (x + line.To.Location.X) / 2;
                else
                    x = (x + line.ledgePositions[ledgePointIndex + 1].coodinate) / 2;
            }
            location = new Point((int)x, (int)y);
        }

        public override void Offset(float dx, float dy)
        {
            base.Offset(dx, dy);
            //сдвиг соответствующей точки перелома
            if (ledgePointIndex < (targetFigure as LedgeLine).ledgePositions.Count)
            {
                LedgePoint p = (targetFigure as LedgeLine).ledgePositions[ledgePointIndex];
                if (p.dir == LedgeDirection.Vertical)
                    p.coodinate += dx;
                else
                    p.coodinate += dy;
            }
        }
        public override void Draw(Graphics gr)
        {
            gr.DrawRectangle(Pens.Black, location.X - defaultSize, location.Y - defaultSize, defaultSize * 2, defaultSize * 2);
            gr.FillRectangle(Brushes.Red, location.X - defaultSize, location.Y - defaultSize, defaultSize * 2, defaultSize * 2);
        }
    }
}
