using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Runtime.Serialization;
using sapr_sim.Figures;
using sapr_sim.Utils;

namespace sapr_sim.WPFCustomElements
{
    [Serializable]
    public class ScrollableCanvas : Canvas, ISerializable, IEqualityComparer<ScrollableCanvas>
    {
        static ScrollableCanvas()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ScrollableCanvas), new FrameworkPropertyMetadata(typeof(ScrollableCanvas)));
        }

        public ScrollableCanvas(SerializationInfo info, StreamingContext context)
        {
            for (int i = 0; i < info.MemberCount; i++)
            {
                UIEntity ent = info.GetValue("Child" + i, typeof(UIEntity)) as UIEntity;                
                Children.Add(ent);
                ZIndexUtil.setCorrectZIndex(this, ent);
            }
        }

        public ScrollableCanvas()
        {

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

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            for (int i = 0; i < Children.Count; i++)
            {
                UIEntity ent = Children[i] as UIEntity;
                info.AddValue("Child" + i, ent);
            }            
        }

        public bool Equals(ScrollableCanvas x, ScrollableCanvas y)
        {
            if (x == null || y == null) return false;
            if (x.Children.Count == 0 && y.Children.Count == 0) return true;
            
            bool result = false;
            for (int i = 0; i < x.Children.Count; i++)
            {
                UIEntity ent1 = x.Children[i] as UIEntity;
                bool localResult = false;
                for (int j = 0; j < y.Children.Count; j++)
                {
                    UIEntity ent2 = y.Children[j] as UIEntity;
                    if (ent1.Equals(ent1, ent2))
                        localResult = true;
                }
                result = localResult;
            }   
            return result;
        }

        public int GetHashCode(ScrollableCanvas obj)
        {
            return obj.GetHashCode();
        }
    }
}
