using sapr_sim.Figures.New;
using sapr_sim.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace sapr_sim
{
    public partial class MainWindow : Window
    {

        private void bindHotkeys()
        {
            try
            {
                RoutedCommand deleteBinding = new RoutedCommand();
                deleteBinding.InputGestures.Add(new KeyGesture(Key.Delete));
                CommandBindings.Add(new CommandBinding(deleteBinding, DeleteShapeCommand));
            }
            catch (Exception err)
            {
                // #TODO
                // enable logger
                MessageBox.Show(err.Message);
            }
        }

        private void DeleteShapeCommand(object sender, ExecutedRoutedEventArgs e)
        {
            if (source != null)
            {
                List<Connector> connectors = ConnectorFinder.find(currentCanvas.Children, source);
                foreach (Connector c in connectors)
                {
                    BindingOperations.ClearBinding(c, Connector.SourceProperty);
                    BindingOperations.ClearBinding(c, Connector.DestinationProperty);
                    currentCanvas.Children.Remove(c);
                }

                source.removePorts();
                currentCanvas.Children.Remove(source);
            }
        }
    }
}
