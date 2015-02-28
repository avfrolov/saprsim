﻿using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Runtime.Serialization;
using sapr_sim.Figures;

namespace sapr_sim.WPFCustomElements
{
    [Serializable]
    public class ScrollableCanvas: Canvas, ISerializable
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
    }
}
