using Entities;
using EntityTransformator;
using Kernel;
using sapr_sim.Figures;
using sapr_sim.WPFCustomElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace sapr_sim
{
    public partial class MainWindow : Window
    {
        private void SimulateButton_Click(object sender, RoutedEventArgs e)
        {
            resetUIShadows(currentCanvas.Children);
            TransformerService ts = new TransformerService();
            List<Entity> entities = ts.transform(currentCanvas.Children);
            Controller controller = new Controller(entities);
            errorsListBox.Items.Clear();
            try
            {
                controller.simulate();
                MessageBox.Show("Результат выполнения моделирования - " + controller.SimulationTime + " условных единиц времени");
            } 
            catch(ValidationException ex)
            {                
                foreach(var err in ex.Errors)
                {
                    errorsListBox.Items.Add(new ListBoxItemError(err.Key, ts.transform(err.Value)));
                }
            }
        }

        private void resetUIShadows(UIElementCollection col)
        {
            foreach(UIElement el in col)
            {
                (el as UIEntity).defaultBitmapEffect();
            }
        }
    }
}
