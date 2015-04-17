using Entities;
using EntityTransformator;
using Kernel;
using sapr_sim.Figures;
using sapr_sim.WPFCustomElements;
using sapr_sim.Utils;
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
            if (Project.Instance.MainProjectItem != null)
            {
                ClosableTabItem cti = findTabItem(Project.Instance.MainProjectItem);
                if (cti == null)
                    createNewDiagram(Project.Instance.MainProjectItem.Canvas, Project.Instance.MainProjectItem.Name);
                else
                    cti.IsSelected = true;
                simulate(Project.Instance.MainProjectItem.Canvas);
            }
            else
            {
                MessageBox.Show("Не указан главный процесс. Укажите главный процесс в Проект -> Настройки проекта");
            }
        }

        private void SimulateLocalButton_Click(object sender, RoutedEventArgs e)
        {
            simulate(currentCanvas);
        }

        private void simulate(Canvas canvas)
        {
            resetUIShadows(canvas.Children);
            SaveAll_Click(null, null);
            TransformerService ts = new TransformerService();
            List<Entity> entities = ts.transform(canvas.Children);
            Model.Instance.timeRestriction = TimeConverter.fromHumanToModel(Project.Instance.TimeRestiction);
            Controller controller = new Controller(entities, ts.getResources());
            errorsListBox.Items.Clear();
            try
            {
                controller.simulate();
                TimeWithMeasure simulationTime = TimeConverter.fromModelToHuman(controller.SimulationTime);

                MessageBox.Show(ProcessingStateMethods.GetDescription(controller.SimulationState) + " Время выполнения моделирования - " + Math.Round(simulationTime.doubleValue, 3) + " " + simulationTime.measure.Name);
            }
            catch (ValidationException ex)
            {
                errorsTab.IsSelected = true;
                foreach (var err in ex.Errors)
                {
                    errorsListBox.Items.Add(new ListBoxItemError(err.Message, ts.transform(err.Entities)));
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
