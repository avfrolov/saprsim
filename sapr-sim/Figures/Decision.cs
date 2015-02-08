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
    public class Decision : UIEntity
    {
        private Rect bound;

        private Port inputPort, yesPort, noPort;

        private UIParam<String> inputProbabilityParams = new UIParam<String>("", new StringParamValidator(), "Параметры входа");

        public Decision(Canvas canvas) : base(canvas)
        {
            Fill = Brushes.LemonChiffon;
            bound = new Rect(new Size(45, 45));
            textParam.Value = "?";
        }

        public override void createAndDraw(double x, double y)
        {
            inputPort = new Port(this, canvas, x - 12, y + 19);
            yesPort = new Port(this, canvas, x + 19, y - 12.5);
            noPort = new Port(this, canvas, x + 19, y + 47.5);
            canvas.Children.Add(inputPort);
            canvas.Children.Add(yesPort);
            canvas.Children.Add(noPort);
            ports.Add(inputPort);
            ports.Add(yesPort);
            ports.Add(noPort);

            label = new Label(this, canvas, x + 20, y + 16, textParam.Value);
            canvas.Children.Add(label);
        }

        public override List<UIParam> getParams()
        {
            List<UIParam> param = base.getParams();
            param.Add(inputProbabilityParams);
            return param;
        }

        protected override System.Windows.Media.Geometry DefiningGeometry
        {
            get 
            {
                GeometryGroup gg = new GeometryGroup();
                gg.FillRule = FillRule.Nonzero;

                RectangleGeometry rg = new RectangleGeometry(bound);
                rg.Transform = new RotateTransform(45, bound.Width / 2, bound.Height / 2);

                gg.Children.Add(rg);
                return gg;
            }
        }
    }
}
