using sapr_sim.Parameters;
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
    public class Collector : UIEntity
    {
        private Rect bound;

        private Port inputPort, outputPort, backPort;

        public Collector(Canvas canvas) : base(canvas)
        {
            bound = new Rect(new Size(45, 45));
            RenderTransform = new RotateTransform(45, bound.Width / 2, bound.Height / 2);
        }

        public override void createAndDraw(double x, double y)
        {
            inputPort = new Port(this, canvas, x - 12, y + 19);
            outputPort = new Port(this, canvas, x + 47.5, y + 19);
            backPort = new Port(this, canvas, x + 19, y - 12.5);
            canvas.Children.Add(inputPort);
            canvas.Children.Add(outputPort);
            canvas.Children.Add(backPort);
            ports.Add(inputPort);
            ports.Add(outputPort);
            ports.Add(backPort);
        }

        public override List<UIParam> getParams()
        {
            return new List<UIParam>();
        }

        protected override System.Windows.Media.Geometry DefiningGeometry
        {
            get 
            {
                GeometryGroup gg = new GeometryGroup();
                gg.FillRule = FillRule.Nonzero;
                gg.Children.Add(new RectangleGeometry(bound));
                return gg;
            }
        }
    }
}
