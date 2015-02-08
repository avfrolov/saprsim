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
using System.Windows.Shapes;

namespace sapr_sim.Figures
{
    public class Procedure : UIEntity
    {
        // not used in current version...
        public static readonly DependencyProperty SizeProperty = DependencyProperty.Register("Size", typeof(Double), typeof(Procedure));

        private Rect bound;

        private Port inputPort, outputPort, resourcePort;
        
        private UIParam<Double> manHour = new UIParam<Double>(0, new DoubleParamValidator(), "Человекочасы");        

        public Procedure(Canvas canvas) : base(canvas)
        {
            Fill = Brushes.LemonChiffon;
            bound = new Rect(new Size(90, 60));
            textParam.Value = "Процедура";
        }

        public double Size
        {
            get { return (double)this.GetValue(SizeProperty); }
            set { this.SetValue(SizeProperty, value); }
        }

        public override void createAndDraw(double x, double y)
        {
            inputPort = new Port(this, canvas, x - 4, y + 26.5);
            outputPort = new Port(this, canvas, x + 86, y + 26.5);
            resourcePort = new Port(this, canvas, x + 42.5, y + 55.5);
            canvas.Children.Add(inputPort);
            canvas.Children.Add(outputPort);
            canvas.Children.Add(resourcePort);
            ports.Add(inputPort);
            ports.Add(outputPort);
            ports.Add(resourcePort);

            label = new Label(this, canvas, x + 18, y + 22, textParam.Value);
            canvas.Children.Add(label);
        }

        public override List<UIParam> getParams()
        {
            List<UIParam> param = base.getParams();
            param.Add(manHour);
            return param;
        }

        protected override System.Windows.Media.Geometry DefiningGeometry
        {
            get 
            {
                GeometryGroup gg = new GeometryGroup();
                gg.FillRule = FillRule.EvenOdd;
                RectangleGeometry rg = new RectangleGeometry(bound, 10, 10);
                gg.Children.Add(rg);
                return gg;
            }
        }

    }
}
