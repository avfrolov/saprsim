using System;
using System.Drawing.Drawing2D;
using System.Drawing;

using sapr_sim.Figures.Basic.Enums;

namespace sapr_sim.Figures.Basic
{
    [Serializable]
    public partial class LabelFigure : SolidFigure
    {
        //возможность двигать метку независимо от главной фигуры
        protected bool freeMoveEnable = true;
        public bool FreeMoveEnable
        {
            get { return freeMoveEnable; }
            set { freeMoveEnable = value; }
        }
        public LabelFigure()
        {
            Path.AddRectangle(new RectangleF(-defaultSize, -defaultSize / 2, 2 * defaultSize, defaultSize));
            textRect = new RectangleF(-defaultSize - 3, -defaultSize / 2 - 3, 2 * defaultSize + 6, defaultSize + 15);
            //опции изменения размера
            resizeDirections = ResizeDirection.None;
            autoSizeEnable = true;
            //настройка внешнего вида
            FigurePen = Pens.Transparent;
            FigureBrush = Brushes.Transparent;
        }
    }
}
