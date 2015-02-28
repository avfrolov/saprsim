using sapr_sim.Parameters;
using sapr_sim.Parameters.Validators;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace sapr_sim.Figures
{
    [Serializable]
    public class Resource : UIEntity, ISerializable
    {

        private Rect bound;
        private Rect topExternalBound;
        private Rect bottomExternalBound;

        private Port port;

        private UIParam<Double> efficiency = new UIParam<Double>(0, new BetweenDoubleParamValidator(0.0, 1.0), "Эффективность");

        private static readonly string DEFAULT_NAME = "Ресурс";

        public Resource(Canvas canvas) : base(canvas)
        {
            init();
            textParam.Value = DEFAULT_NAME;
        }

        public Resource(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            this.port = info.GetValue("port", typeof(Port)) as Port;
            ports.Add(port);

            efficiency = info.GetValue("efficiency", typeof(UIParam<Double>)) as UIParam<Double>;

            init();
        }

        public override void createAndDraw(double x, double y)
        {
            port = new Port(this, canvas, x + 42.5, y - 3.5);
            canvas.Children.Add(port);
            ports.Add(port);

            label = new Label(this, canvas, x + 28, y + 22, textParam.Value);
            canvas.Children.Add(label);
        }

        public override List<UIParam> getParams()
        {
            List<UIParam> param = base.getParams();
            param.Add(efficiency);
            return param;
        }

        public double Efficiency
        {
            get { return efficiency.Value; }
            set { efficiency.Value = value; }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("port", port);
            info.AddValue("efficiency", efficiency);
        }

        protected override System.Windows.Media.Geometry DefiningGeometry
        {
            get 
            {
                GeometryGroup gg = new GeometryGroup();
                gg.FillRule = FillRule.Nonzero;
                gg.Children.Add(new RectangleGeometry(bound));
                gg.Children.Add(new RectangleGeometry(topExternalBound));
                gg.Children.Add(new RectangleGeometry(bottomExternalBound));
                return gg;
            }
        }

        private void init()
        {
            bound = new Rect(new Size(90, 60));
            topExternalBound = new Rect(new Point(0, 0), new Point(90, 10));
            bottomExternalBound = new Rect(new Point(0, 50), new Point(90, 60));
        }
    }
}
