using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using System.Windows.Controls;
using sapr_sim.Parameters;

namespace sapr_sim.Figures
{
    public class Label : UIEntity
    {

        public Label() : base()
        {
            label = new FormattedText("Надпись",
                CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                new Typeface("Times New Roman"), 12, Brushes.Black);
        }

        public override void createAndDrawPorts(double x, double y)
        {
        }

        public override List<UIParam> getParams()
        {
            List<UIParam> param = new List<UIParam>();
            return param;
        }

        protected override Geometry DefiningGeometry
        {
            get
            {
                GeometryGroup gg = new GeometryGroup();
                gg.FillRule = FillRule.EvenOdd;
                gg.Children.Add(label.BuildGeometry(new Point()));
                return gg;
            }
        }
    }
}
