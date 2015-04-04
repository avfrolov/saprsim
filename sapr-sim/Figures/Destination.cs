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
using System.Runtime.Serialization;

namespace sapr_sim.Figures
{
    [Serializable]
    public class Destination : Source, ISerializable
    {

        public Destination(Canvas canvas) : base(canvas)
        {
            Fill = Brushes.Red;
        }

        public Destination(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            this.port = info.GetValue("port", typeof(Port)) as Port;
            ports.Add(port);
            Fill = Brushes.Red;
        }

        public override void createAndDraw(double x, double y)
        {
            port = new Port(this, canvas, PortType.INPUT, x - 4, y + 12);
            canvas.Children.Add(port);
            ports.Add(port);
        }

        public override List<UIParam> getParams()
        {
            return null;
        }

    }
}
