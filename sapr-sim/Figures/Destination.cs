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
    public class Destination : Source
    {

        public Destination(Canvas canvas) : base(canvas)
        {
            Fill = Brushes.LightGreen;
            textParam.Value = "Конец";
        }

        public override void createAndDraw(double x, double y)
        {
            port = new Port(this, canvas, x - 4, y + 26.5);
            canvas.Children.Add(port);
            ports.Add(port);

            label = new Label(this, canvas, x + 14, y + 23, textParam.Value);
            canvas.Children.Add(label);
        }

        public override List<UIParam> getParams()
        {
            List<UIParam> param = base.getParams();
            // removing inheireted property 'projectsCount'
            param.RemoveAt(1);
            // removing inheireted property 'complexity'
            param.RemoveAt(1);
            return param;
        }

    }
}
