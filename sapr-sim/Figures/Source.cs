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

        private const int innerLabelOffsetX = 14;
        private const int innerLabelOffsetY = 23;

        public Source(Canvas canvas) : base(canvas)
        {
            Fill = Brushes.Red;

            bound = new Rect(new Size(60, 60));
            label = new FormattedText("Начало",
                CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                new Typeface("Times New Roman"), 12, Brushes.Black);
        }

        public override void createAndDrawPorts(double x, double y)
        {
            port = new Port(this, canvas, x + 56, y + 26.5);
            canvas.Children.Add(port);
            ports.Add(port);
        }

        public override List<UIParam> getParams()
        {
            List<UIParam> param = new List<UIParam>();
            param.Add(projectsCount);
            return param;
        }

        protected override Geometry DefiningGeometry
        {
            get
            {
                GeometryGroup gg = new GeometryGroup();
                gg.FillRule = FillRule.EvenOdd;
                EllipseGeometry eg = new EllipseGeometry(bound);
                Geometry geometry = label.BuildGeometry(new Point(innerLabelOffsetX, innerLabelOffsetY));

                gg.Children.Add(eg);
                gg.Children.Add(geometry);

                return gg;
            }
        }
    }
}
