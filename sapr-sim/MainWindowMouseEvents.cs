using sapr_sim.Figures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace sapr_sim
{
    public partial class MainWindow : Window
    {

        // used in moving figure on canvas
        private bool captured = false;
        private UIEntity selected = null;

        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (selected != null && !captured)
            {
                selected.BitmapEffect = UIEntity.defaultBitmapEffect(selected);
                selected = null;
            }
        }

        private void Shape_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 1)
            {
                // clear bitmap effect for previous selected entity
                if (selected != null)
                    selected.BitmapEffect = UIEntity.defaultBitmapEffect(selected);

                selected = (UIEntity)sender;
                Mouse.Capture(selected);
                captured = true;

                double xCanvas = e.GetPosition(this).X;
                double yCanvas = e.GetPosition(this).Y;

                selected.putMovingCoordinate(selected,
                    VisualTreeHelper.GetOffset(selected).X,
                    VisualTreeHelper.GetOffset(selected).Y,
                    xCanvas, yCanvas);

                foreach (Port p in selected.getPorts())
                {
                    selected.putMovingCoordinate(
                        p,
                        VisualTreeHelper.GetOffset(p).X,
                        VisualTreeHelper.GetOffset(p).Y,
                        xCanvas, yCanvas);
                }

                if (!(selected is Connector && selected is Port))
                    selected.BitmapEffect = new DropShadowBitmapEffect() { ShadowDepth = 0, Color = Colors.Red };
            }
            else if (e.ClickCount == 2 && !(sender is Port || sender is Connector))
            {
                UIEntity ent = sender as UIEntity;
                new ParameterDialog(ent.getParams(), ent).ShowDialog();
            }
        }

        private void Shape_MouseMove(object sender, MouseEventArgs e)
        {
            if (captured)
            {
                double x = e.GetPosition(this).X;
                double y = e.GetPosition(this).Y;

                UIEntity.CoordinatesHandler sch = selected.getMovingCoordinate(selected);
                sch.xShape += x - sch.xCanvas;
                sch.yShape += y - sch.yCanvas;
                Canvas.SetLeft(selected, sch.xShape);
                Canvas.SetTop(selected, sch.yShape);                

                foreach (Port p in selected.getPorts())
                {
                    UIEntity.CoordinatesHandler ch = selected.getMovingCoordinate(p);
                    ch.xShape += x - ch.xCanvas;
                    ch.yShape += y - ch.yCanvas;
                    Canvas.SetLeft(p, ch.xShape);
                    Canvas.SetTop(p, ch.yShape);
                    ch.xCanvas = x;
                    ch.yCanvas = y;
                }

                sch.xCanvas = x;
                sch.yCanvas = y;
            }
        }

        private void Shape_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Mouse.Capture(null);
            captured = false;
        }

    }
}
