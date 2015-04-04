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

namespace sapr_sim.Figures
{
    [Serializable]
    public class Source : UIEntity, ISerializable
    {

        protected Port port;
        private Rect bound;

        private UIParam<Int32> projectsCount = new UIParam<Int32>(1, new PositiveIntegerParamValidator(), "Количество проектов");
        private UIParam<Int32> complexity = new UIParam<Int32>(1, new PositiveIntegerParamValidator(), "Сложность проектов");

        public Source(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            this.port = info.GetValue("port", typeof(Port)) as Port;
            ports.Add(port);

            projectsCount = info.GetValue("projectsCount", typeof(UIParam<Int32>)) as UIParam<Int32>;
            complexity = info.GetValue("complexity", typeof(UIParam<Int32>)) as UIParam<Int32>;

            init();
        }

        public Source(Canvas canvas) : base(canvas)
        {
            init();
        }

        public override void createAndDraw(double x, double y)
        {
            port = new Port(this, canvas, PortType.OUTPUT, x + 26, y + 12);
            canvas.Children.Add(port);
            ports.Add(port);
        }

        public override List<UIParam> getParams()
        {
            List<UIParam> param = new List<UIParam>();
            param.Add(projectsCount);
            param.Add(complexity);
            return param;
        }

        public int ProjectsCount
        {
            get { return projectsCount.Value; }
            set { projectsCount.Value = value; }
        }

        public int Complexity
        {
            get { return complexity.Value; }
            set { complexity.Value = value; }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("port", port);
            info.AddValue("projectsCount", projectsCount);
            info.AddValue("complexity", complexity);
        }

        protected override Geometry DefiningGeometry
        {
            get
            {
                GeometryGroup gg = new GeometryGroup();
                gg.FillRule = FillRule.EvenOdd;
                EllipseGeometry eg = new EllipseGeometry(bound);
                gg.Children.Add(eg);
                return gg;
            }
        }

        private void init()
        {
            Fill = Brushes.LightGreen;
            bound = new Rect(new Size(30, 30));
        }
    }
}
