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
    public class Source : UIEntity
    {
        private Rect bound;

        protected Port port;

        private UIParam<Int32> projectsCount = new UIParam<Int32>(0, new IntegerParamValidator(), "Количество проектов");
        private UIParam<Int32> complexity = new UIParam<Int32>(1, new IntegerParamValidator(), "Сложность проектов");

        public Source(Canvas canvas) : base(canvas)
        {
            Fill = Brushes.Red;
            bound = new Rect(new Size(60, 60));
            textParam.Value = "Начало";
        }

        public override void createAndDraw(double x, double y)
        {
            port = new Port(this, canvas, x + 56, y + 26.5);
            canvas.Children.Add(port);
            ports.Add(port);

            label = new Label(this, canvas, x + 14, y + 23, textParam.Value);
            canvas.Children.Add(label);
        }

        public override List<UIParam> getParams()
        {
            List<UIParam> param = base.getParams();
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
    }
}
