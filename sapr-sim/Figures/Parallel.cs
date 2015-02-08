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
    public class Parallel : UIEntity
    {
        private Rect bound;

        private Port inputPort, outputPort1, outputPort2;

        public Parallel(Canvas canvas) : base(canvas)
        {
            Fill = Brushes.Black;
            StrokeThickness = .5;
            bound = new Rect(new Size(10, 90));
            textParam.Value = "Распараллеливание";
        }

        public override void createAndDraw(double x, double y)
        {
            inputPort = new Port(this, canvas, x - 4, y + 45);
            outputPort1 = new Port(this, canvas, x + 7, y + 22.5);
            outputPort2 = new Port(this, canvas, x + 7, y + 67.5);
            canvas.Children.Add(inputPort);
            canvas.Children.Add(outputPort1);
            canvas.Children.Add(outputPort2);
            ports.Add(inputPort);
            ports.Add(outputPort1);
            ports.Add(outputPort2);

            label = new Label(this, canvas, x - 45, y - 20, textParam.Value);
            canvas.Children.Add(label);
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
