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
    public class Procedure : UIEntity, ISerializable
    {
        // not used in current version...
        public static readonly DependencyProperty SizeProperty = DependencyProperty.Register("Size", typeof(Double), typeof(Procedure));

        private Rect bound;
        private Port inputPort, outputPort, resourcePort;
        
        private UIParam<Double> manHour = new UIParam<Double>(1, new PositiveDoubleParamValidator(), "Продолжительность");

        private static readonly string DEFAULT_NAME = "Процедура";

        public Procedure(Canvas canvas) : base(canvas)
        {
            init();
            textParam.Value = DEFAULT_NAME;
        }

        public Procedure(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            this.inputPort = info.GetValue("inputPort", typeof(Port)) as Port;
            this.outputPort = info.GetValue("outputPort", typeof(Port)) as Port;
            this.resourcePort = info.GetValue("resourcePort", typeof(Port)) as Port;
            ports.Add(inputPort);
            ports.Add(outputPort);
            ports.Add(resourcePort);

            manHour = info.GetValue("manHour", typeof(UIParam<Double>)) as UIParam<Double>;

            init();
        }

        public double Size
        {
            get { return (double)this.GetValue(SizeProperty); }
            set { this.SetValue(SizeProperty, value); }
        }

        public override void createAndDraw(double x, double y)
        {
            inputPort = new Port(this, canvas, PortType.INPUT, x - 4, y + 26.5);
            outputPort = new Port(this, canvas, PortType.OUTPUT, x + 86, y + 26.5);
            resourcePort = new Port(this, canvas, PortType.RESOURCE, x + 42.5, y + 55.5);
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

        public double ManHour
        {
            get { return manHour.Value; }
            set { manHour.Value = value; }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("inputPort", inputPort);
            info.AddValue("outputPort", outputPort);
            info.AddValue("resourcePort", resourcePort);
            info.AddValue("manHour", manHour);
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

        private void init()
        {
            Fill = Brushes.LemonChiffon;
            bound = new Rect(new Size(90, 60));
        }
    }
}
