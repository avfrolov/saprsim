using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Drawing2D;
using System.Drawing;

using sapr_sim.Figures.Basic.Markers;
using sapr_sim.Figures.Custom;

namespace sapr_sim.Figures.Basic
{
    [Serializable]
    public abstract partial class ComplexFigure : Figure, ISolidFigure//, ISerializable
    {
        private int defaultLabelOffset = 5;
        private int defaultPortOffset = 6;
        private int defaultShadowOffset = 4;
        private PointF location;
        private int mainFigureIndex = 0;
        protected List<SolidFigure> primitives = new List<SolidFigure>();

        public List<SolidFigure> SolidFigures
        {
            get { return primitives; }
        }

        public override RectangleF Bounds
        {
            get
            {
                RectangleF bounds = new RectangleF(Location.X, Location.Y, 0, 0);
                foreach (SolidFigure fig in primitives)
                    bounds = RectangleF.Union(bounds, fig.Bounds);
                return bounds;
            }
        }

        public virtual RectangleF MainFigureBounds
        {
            get { return primitives[mainFigureIndex].Bounds; }
        }

        public string MainFigureText
        {
            get { return SolidFigures[mainFigureIndex].Text; }
            set { SolidFigures[mainFigureIndex].Text = value; }
        }
        
        public List<RectangleF> SolidFiguresBounds
        {
            get
            {
                List<RectangleF> rects = new List<RectangleF>();
                for (int i = 0; i < primitives.Count; i++)
                    rects.Add(primitives[i].Bounds);
                return rects;
            }
        }


        public System.Drawing.Rectangle GetTextBounds(int figureIndex)
        {
            return primitives[figureIndex].TextBounds;
        }

        public List<System.Drawing.Rectangle> TextBounds
        {
            get
            {
                List<System.Drawing.Rectangle> bounds = new List<System.Drawing.Rectangle>();
                for (int i = 0; i < primitives.Count; i++)
                {
                    if (primitives[i].TextChangeEnable)
                        bounds.Add(primitives[i].TextBounds);
                }
                return bounds;
            }
        }

        public SizeF Size
        {
            get { return primitives[mainFigureIndex].Size; }
            set
            {
                //offset
                primitives[mainFigureIndex].Size = value;
            }
        }
        
        public virtual void PlaceFreelyMovedLabels()
        {
            int i;
            Label lab;
            float d1, d2;
            for (i = 0; i < primitives.Count; i++)
            {
                if (i != mainFigureIndex && (primitives[i] is Label))
                {
                    lab = (primitives[i] as Label);
                    if (lab.FreeMoveEnable && lab.Bounds.IntersectsWith(MainFigureBounds))
                    {
                        d1 = Math.Abs(lab.Location.X - MainFigureBounds.Left);
                        d2 = Math.Abs(lab.Location.X - MainFigureBounds.Right);
                        lab.Location = new PointF((d1 < d2 ? MainFigureBounds.Left - lab.Size.Width / 2 - defaultLabelOffset
                            : MainFigureBounds.Right + lab.Size.Width / 2 + defaultLabelOffset), lab.Location.Y);
                        //lab.Location.X = (d1 < d2 ? MainFigureBounds.Left - lab.Size.Width / 2 - defaultLabelOffset
                        //    : MainFigureBounds.Right + lab.Size.Width / 2 + defaultLabelOffset);
                        
                        d1 = Math.Abs(lab.Location.Y - MainFigureBounds.Top);
                        d2 = Math.Abs(lab.Location.Y - MainFigureBounds.Bottom);
                        lab.Location = new PointF(lab.Location.X, (d1 < d2 ? MainFigureBounds.Top - lab.Size.Height / 2 - defaultLabelOffset
                            : MainFigureBounds.Bottom + lab.Size.Height / 2 + defaultLabelOffset));
                        //lab.Location.Y = (d1 < d2 ? MainFigureBounds.Top - lab.Size.Height / 2 - defaultLabelOffset
                        //    : MainFigureBounds.Bottom + lab.Size.Height / 2 + defaultLabelOffset);
                    }
                }
            }
        }

        public override void Offset(float dx, float dy)
        {
            foreach (SolidFigure f in primitives)
                f.Offset(dx, dy);
            location = primitives[mainFigureIndex].Location;
        }

        public PointF Location
        {
            get { return primitives[mainFigureIndex].Location; }
            set
            {
                location = primitives[mainFigureIndex].Location;
                this.Offset((int)(-location.X + value.X), (int)(-location.Y + value.Y));
            }
        }
        
        public void SetAllTextes(string[] inputTextes)
        {
            int j = 0;
            for (int i = 0; i < primitives.Count; i++)
            {
                if (j < inputTextes.Length)
                {
                    if (primitives[i].TextChangeEnable)
                        primitives[i].Text = inputTextes[j++];
                }
                else
                    break;
            }
        }

        public override List<Marker> CreateMarkers(Diagram diagram)
        {
            //ìàðêåðû èçìåíåíèÿ ðàçìåðà ñîçäàþòñÿ òîëüêî íà íåêîòîðûå ôèãóðû
            List<Marker> m = new List<Marker>();
            foreach (SolidFigure fig in primitives)
                m.AddRange(fig.CreateMarkers(diagram));
            return m;
        }

        public override bool IsInsidePoint(Point p)
        {            
            foreach (SolidFigure f in primitives)
                if (f.IsInsidePoint(p))
                    return true;
            return false;
        }

        public override void Draw(Graphics gr)
        {
            RecalcFigure();
            DrawFigures(gr);
        }

        public abstract void RecalcFigure();
        
        public virtual void DrawFigures(Graphics gr)
        {
            DrawShadow(gr);
            foreach (SolidFigure f in primitives)
                f.Draw(gr);
        }
        
        public virtual void DrawShadow(Graphics gr)
        {
            Object shadow = Activator.CreateInstance(primitives[mainFigureIndex].GetType());
            (shadow as SolidFigure).Size = primitives[mainFigureIndex].Size;
            (shadow as SolidFigure).Location = PointF.Add(primitives[mainFigureIndex].Location,
                new Size(defaultShadowOffset, defaultShadowOffset));
            Brush br = new SolidBrush(Color.FromArgb(50, 0, 0, 0));
            (shadow as SolidFigure).FigureBrush = br;
            (shadow as SolidFigure).FigurePen = Pens.Transparent;
            (shadow as SolidFigure).Draw(gr);
        }

        public Brush FigureBrush
        {
            get { return primitives[mainFigureIndex].FigureBrush; }
            set { primitives[mainFigureIndex].FigureBrush = value; }
        }

        public virtual Pen FigurePen
        {
            get { return primitives[mainFigureIndex].FigurePen; }
            set { primitives[mainFigureIndex].FigurePen = value; }
        }
        
        public virtual bool CheckConnection()
        {
            return true;
        }

        public override object Clone()
        {
            int i;
            object figure = Activator.CreateInstance(this.GetType());
            (figure as ComplexFigure).Location = this.Location;
            (figure as ComplexFigure).Name = this.name;
            (figure as ComplexFigure).mainFigureIndex = this.mainFigureIndex;

            for (i = 0; i < this.SolidFigures.Count; i++)
                (figure as ComplexFigure).SolidFigures[i] = this.SolidFigures[i].Clone() as SolidFigure;

            for (i = 0; i < this.SolidFigures.Count; i++)
            {
                (figure as ComplexFigure).SolidFigures[i].OwnerFigure = figure as ComplexFigure;
                if ((figure as ComplexFigure).SolidFigures[i] is IConnectable)
                {
                    ((figure as ComplexFigure).SolidFigures[i] as IConnectable).MainFigure = (figure as ComplexFigure).SolidFigures[this.mainFigureIndex];
                   // ((figure as ComplexFigure).SolidFigures[i] as IConnectable).CheckConnection = (figure as ComplexFigure).CheckConnection;
                }
            }
            return figure;
        }
    }    
}
