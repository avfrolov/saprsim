using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace sapr_sim.Figures.New
{
    public class Collector : UIEntity
    {
        private Rect bound;

        public Collector(Canvas canvas) : base(canvas)
        {
            bound = new Rect(new Size(45, 45));
            RenderTransform = new RotateTransform(45, bound.Width / 2, bound.Height / 2);
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
