using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace sapr_sim.WPFCustomElements
{
    class ScrollableCanvas: Canvas
    {
        static ScrollableCanvas()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ScrollableCanvas), new FrameworkPropertyMetadata(typeof(ScrollableCanvas)));
        }

        protected override Size MeasureOverride(Size constraint)
        {
            double bottomMost = 0d;
            double rightMost = 0d;

            foreach(object obj in Children)
            {
                FrameworkElement child = obj as FrameworkElement;

                if(child != null)
                {
                    child.Measure(constraint);
                    bottomMost = Math.Max(bottomMost, VisualTreeHelper.GetOffset(child).Y + child.DesiredSize.Height);
                    rightMost = Math.Max(rightMost, VisualTreeHelper.GetOffset(child).X + child.DesiredSize.Width);
                }
            }
            return new Size(rightMost, bottomMost);
        }
    }
}
