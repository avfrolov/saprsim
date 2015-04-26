using Entities;
using EntityTransformator;
using Kernel;
using sapr_sim.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace sapr_sim
{

    public partial class RunSimulation : Window
    {

        private Canvas canvas;
        private Controller controller;

        public RunSimulation(ProjectItem pi, Controller controller)
        {
            InitializeComponent();
            this.canvas = pi.Canvas;
            this.controller = controller;
            processName.Content = pi.Name;            
        }

        private void btnRun_Click(object sender, RoutedEventArgs e)
        {
            Model.Instance.timeRestriction = TimeConverter.fromHumanToModel(Project.Instance.TimeRestiction);

            controller.simulate();
                
            TimeWithMeasure simulationTime = TimeConverter.fromModelToHuman(controller.SimulationTime);
            time.Content = Math.Round(simulationTime.doubleValue, 3) + " " + simulationTime.measure.Name;
            runStatus.Content = ProcessingStateMethods.GetDescription(controller.SimulationState);                
            totalRunCount.Content = totalRunCount.Content == null ? 1 : int.Parse(totalRunCount.Content.ToString()) + 1;
            if (controller.SimulationState == ProcessingState.FINE)
                image.Source = new BitmapImage(new Uri(@"pack://application:,,,/Resources/success.png", UriKind.Absolute)); 
            else
                image.Source = new BitmapImage(new Uri(@"pack://application:,,,/Resources/failure.png", UriKind.Absolute)); 
        }
    }
}
