using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace sapr_sim.Figures
{
    public class Label : UIEntity
    {

        private UIEntity owner;
        private string text;
        private FormattedText ft;

        public Label(Canvas canvas) : base(canvas)
        {
            textParam.Value = "Надпись";
            ft = new FormattedText(textParam.Value,
                CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                new Typeface("Times New Roman"), 12, Brushes.Black);
            label = this;
        }

        public Label(UIEntity owner, Canvas canvas, double xPos, double yPos, string text) : base(canvas)
        {
            ft = new FormattedText(text,
                CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                new Typeface("Times New Roman"), 12, Brushes.Black);
            label = this;
                 
            this.canvas = canvas;
            this.owner = owner;
            this.text = text;

            Canvas.SetLeft(this, xPos);
            Canvas.SetTop(this, yPos);
        }

        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        public override void createAndDraw(double x, double y)
        {            
        }

        protected override System.Windows.Media.Geometry DefiningGeometry
        {
            get
            {
                GeometryGroup gg = new GeometryGroup();
                gg.FillRule = FillRule.EvenOdd;
                gg.Children.Add(ft.BuildGeometry(new Point()));
                return gg;
            }
        }
    }
}
