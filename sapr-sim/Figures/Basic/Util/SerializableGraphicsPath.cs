using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Runtime.Serialization;
using System.Reflection;
using System.ComponentModel;

namespace sapr_sim.Figures.Basic.Util
{
    [Serializable]
    public class SerializableGraphicsPath : ISerializable
    {
        public GraphicsPath path = new GraphicsPath();
        public Pen pen = new Pen(Brushes.Black);
        public Brush brush = new SolidBrush(Color.White);

        public SerializableGraphicsPath()
        {

        }

        private SerializableGraphicsPath(SerializationInfo info, StreamingContext context)
        {
            //Path
            if (!info.GetBoolean("path_empty"))
            {
                PointF[] points = (PointF[])info.GetValue("p", typeof(PointF[]));
                byte[] types = (byte[])info.GetValue("t", typeof(byte[]));
                path = new GraphicsPath(points, types);
            }
            else
                path = new GraphicsPath();
            //Pen
            pen.Color = (Color)info.GetValue("p_color", typeof(Color));
            pen.Width = (float)info.GetValue("p_width", typeof(float));
            //Brush
            if (info.GetBoolean("b_is_gradient"))
            {
                brush = new LinearGradientBrush(
                    (RectangleF)info.GetValue("b_rect", typeof(RectangleF)),
                    (Color)info.GetValue("b_color1", typeof(Color)),
                    (Color)info.GetValue("b_color2", typeof(Color)), (float)0);
            }
            else
                brush = new SolidBrush((Color)info.GetValue("b_color", typeof(Color)));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (path.PointCount > 0)
            {
                //path
                info.AddValue("path_empty", false);
                info.AddValue("p", path.PathPoints);
                info.AddValue("t", path.PathTypes);
            }
            else
                info.AddValue("path_empty", true);
            //pen
            info.AddValue("p_color", pen.Color);
            info.AddValue("p_width", pen.Width);
            //brush
            info.AddValue("b_is_gradient", (brush is LinearGradientBrush));
            if (brush is LinearGradientBrush)
            {
                info.AddValue("b_color1", (brush as LinearGradientBrush).LinearColors[0]);
                info.AddValue("b_color2", (brush as LinearGradientBrush).LinearColors[1]);
                info.AddValue("b_rect", (brush as LinearGradientBrush).Rectangle);
            }
            else
            {
                info.AddValue("b_color", (brush as SolidBrush).Color);
            }
        }
    }
}
