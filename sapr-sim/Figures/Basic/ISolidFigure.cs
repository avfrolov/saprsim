using System;
using System.Drawing.Drawing2D;
using System.Drawing;

namespace sapr_sim.Figures.Basic
{
    //интерфейс, который определяет, содержит ли фигура сплошные элементы
    public interface ISolidFigure
    {
        SizeF Size
        {
            get;
            set;
        }
        PointF Location
        {
            get;
            set;
        }
        Brush FigureBrush
        {
            get;
            set;
        }
        Pen FigurePen
        {
            get;
            set;
        }
    }
}
