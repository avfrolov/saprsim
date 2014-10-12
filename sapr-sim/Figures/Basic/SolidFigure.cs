using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Reflection;

using sapr_sim.Figures.Custom;
using sapr_sim.Figures.Basic.Util;
using sapr_sim.Figures.Basic.Enums;
using sapr_sim.Figures.Basic.Markers;

namespace sapr_sim.Figures.Basic
{
    //многоугольник с текстом внутри
    [Serializable]
    public class SolidFigure : Figure, ISolidFigure
    {
        //размер новой фигуры, по умолчанию
        public int defaultSize = 40;

        //заливка фигуры
        public Brush FigureBrush
        {
            get { return serializablePath.brush; }
            set { serializablePath.brush = value; }
        }

        //местоположение центра фигуры
        protected PointF location;

        //прямоугольник, в котором расположен текст
        protected RectangleF textRect;

        //текст
        private string text = null;

        //шрифт, которым написан текст
        private Font textFont = SystemFonts.DefaultFont;

        //цвет шрифта, которым написан текст
        private Brush textColor = Brushes.Black;

        //ссылка на сложную фигуру, частью которой является эта
        protected Figure ownerFigure;
       
        public Figure OwnerFigure
        {
            get { return ownerFigure; }
            set { ownerFigure = value; }
        }

        //направления, в которых может быть изменена фигура
        protected ResizeDirection resizeDirections = ResizeDirection.BothByCornerMarkers;
        public ResizeDirection ResizeDirection
        {
            get { return resizeDirections; }
            set { resizeDirections = value; }
        }
        
        //разрешение изменять текст на фигуре
        protected bool textChangeEnable = true;     
        public bool TextChangeEnable
        {
            get { return textChangeEnable; }
            set { textChangeEnable = value; }
        }

        //разрешение автоматически подгонять размер под текст
        protected bool autoSizeEnable = false;
        public bool AutoSizeEnable
        {
            get { return autoSizeEnable; }
            set { autoSizeEnable = value; }
        }

        //настройки вывода текста
        public virtual StringFormat StringFormat
        {
            get
            {
                StringFormat stringFormat = new StringFormat();
                stringFormat.Alignment = StringAlignment.Center;
                stringFormat.LineAlignment = StringAlignment.Center;
                return stringFormat;
            }
        }

        //находится ли точка внутри контура?
        public override bool IsInsidePoint(Point p)
        {
            return Path.IsVisible(p.X - location.X, p.Y - location.Y);
        }

        //прямоугольник вокруг фигуры (в абсолютных координатах)
        public override RectangleF Bounds
        {
            get
            {
                RectangleF bounds = Path.GetBounds();
                return new RectangleF(bounds.Left + location.X, bounds.Top + location.Y, bounds.Width, bounds.Height);
            }
        }

        //прямоугольник текста (в абсолютных координатах)
        public System.Drawing.Rectangle TextBounds
        {
            get
            {
                return new System.Drawing.Rectangle((int)textRect.Left + (int)location.X, (int)textRect.Top + (int)location.Y, (int)textRect.Width, (int)textRect.Height);
            }
        }

        //размер прямоугольника вокруг фигуры
        public SizeF Size
        {
            get { return Path.GetBounds().Size; }
            set
            {
                SizeF oldSize = Path.GetBounds().Size;
                SizeF newSize = new SizeF(Math.Max(1, value.Width), Math.Max(1, value.Height));
                //коэффициент шкалировани по x
                float kx = newSize.Width / oldSize.Width;
                //коэффициент шкалировани по y
                float ky = newSize.Height / oldSize.Height;
                Scale(kx, ky);
                //переопределение заливки
                if (FigureBrush is LinearGradientBrush)
                {
                    FigureBrush = new LinearGradientBrush(Path.GetBounds(),
                        (FigureBrush as LinearGradientBrush).LinearColors[0],
                        (FigureBrush as LinearGradientBrush).LinearColors[1], (float)0);
                }
            }
        }

        //изменение масштаба фигуры
        public void Scale(float scaleX, float scaleY)
        {
            //масштабируем линии
            Matrix m = new Matrix();
            m.Scale(scaleX, scaleY);
            Path.Transform(m);
            //масштабируем прямоугльник текста
            textRect = new RectangleF(textRect.Left * scaleX, textRect.Top * scaleY, textRect.Width * scaleX, textRect.Height * scaleY);
        }

        //сдвиг местоположения фигуры
        public override void Offset(float dx, float dy)
        {
            location.X += dx;
            location.Y += dy;
        }

        /// <summary>
        /// Автоматически подгоняет размер фигуры под текст
        /// </summary>
        public void AutoSize()
        {
            //Size = System.Windows.Forms.TextRenderer.MeasureText(text + "   ", textFont);
        }

        //отрисовка фигуры
        public override void Draw(Graphics gr)
        {
            gr.TranslateTransform(location.X, location.Y);
            gr.FillPath(FigureBrush, Path);
            gr.DrawPath(FigurePen, Path);
            if (!string.IsNullOrEmpty(text))
                gr.DrawString(text, textFont, textColor, textRect, StringFormat);
            gr.ResetTransform();
        }

        //создание маркера для изменения размера
        public override List<Marker> CreateMarkers(Diagram diagram)
        {
            List<Marker> markers = new List<Marker>();
            Marker m0 = new BottomRightMarker();
            Marker m1 = new BottomLeftMarker();
            Marker m2 = new TopRightMarker();
            Marker m3 = new TopLeftMarker();
            Marker mrw = new RightWidthMarker();
            Marker mlw = new LeftWidthMarker();
            Marker mth = new TopHeightMarker();
            Marker mbh = new BottomHeightMarker();
            m0.targetFigure = m1.targetFigure = m2.targetFigure = m3.targetFigure =
            mrw.targetFigure = mlw.targetFigure = mth.targetFigure = mbh.targetFigure = this;
            //добавляем угловые маркеры, если можно изменить фигуру во всех направлениях
            if (resizeDirections == ResizeDirection.Both || resizeDirections == ResizeDirection.BothByCornerMarkers)
            {
                markers.Add(m0);
                markers.Add(m1);
                markers.Add(m2);
                markers.Add(m3);
                if (resizeDirections == ResizeDirection.BothByCornerMarkers)
                    return markers;
            }
            //добавляем маркеры изменения по горизонтальной оси
            if (resizeDirections != ResizeDirection.Vertical && resizeDirections != ResizeDirection.None)
            {
                markers.Add(mrw);
                markers.Add(mlw);
            }
            //добавляем маркеры изменения по вертикальной оси
            if (resizeDirections != ResizeDirection.Horizontal && resizeDirections != ResizeDirection.None)
            {
                markers.Add(mbh);
                markers.Add(mth);
            }
            return markers;
        }

        //свойство "положение"
        public PointF Location
        {
            get { return location; }
            set { location = value; }
        }

        //свойство "текст"
        public string Text
        {
            get { return text; }
            set
            {
                text = value;
                if (autoSizeEnable)
                    AutoSize();
            }
        }
        public override object Clone()
        {
            object figure = Activator.CreateInstance(this.GetType());
            FieldInfo[] fields = figure.GetType().GetFields();
            //неглубокое копирование полей, допускающих это
            int i = 0;
            foreach (FieldInfo fi in this.GetType().GetFields())
            {
                if (fi.Name != "serializablePath"
                    && fi.Name != "textRect")
                    fields[i].SetValue(figure, fi.GetValue(this));
                i++;
            }
            //установка ряда свойств, требующих клонирование
            (figure as Figure).serializablePath = new SerializableGraphicsPath();
            (figure as Figure).FigurePen = (Pen)(this as Figure).FigurePen.Clone();
            (figure as Figure).Path = (GraphicsPath)(this as Figure).Path.Clone();
            (figure as SolidFigure).FigureBrush = (Brush)(this as SolidFigure).FigureBrush.Clone();
            (figure as Figure).Name = this.Name;
            (figure as SolidFigure).textRect = new RectangleF(this.textRect.Location, this.textRect.Size);
            return figure;
        }
    }
}
