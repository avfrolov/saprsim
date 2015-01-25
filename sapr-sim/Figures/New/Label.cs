using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using System.Windows.Controls;

namespace sapr_sim.Figures.New
{
    public class Label : UIEntity
    {
        private FormattedText label;

        public Label() : base()
        {
            label = new FormattedText("Надпись",
                CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                new Typeface("Times New Roman"), 12, Brushes.Black);
        }

        public override void createAndDrawPorts(double x, double y)
        {
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
