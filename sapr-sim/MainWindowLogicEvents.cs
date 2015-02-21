using Entities;
using EntityTransformator;
using Kernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace sapr_sim
{
    public partial class MainWindow : Window
    {
        private void SimulateButton_Click(object sender, RoutedEventArgs e)
        {            
            TransformerService ts = new TransformerService();
            List<Entity> entities = ts.transform(currentCanvas.Children);
            Performer performer = new Performer(entities);
            try
            {
                performer.simulate();
                MessageBox.Show("Результат выполнения моделирования - " + performer.SimulationTime + " условных единиц времени");
            } 
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка валидации");
            }
        }
    }
}
