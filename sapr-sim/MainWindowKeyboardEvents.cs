using sapr_sim.Figures;
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

                RoutedCommand newTabBinding = new RoutedCommand();
                newTabBinding.InputGestures.Add(new KeyGesture(Key.N, ModifierKeys.Control));
                CommandBindings.Add(new CommandBinding(newTabBinding, CreateNewTabCommand));
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
            if (selected != null && !(selected is Port))
            {
                List<Connector> connectors = ConnectorFinder.find(currentCanvas.Children, selected);
                foreach (Connector c in connectors)
                {
                    BindingOperations.ClearBinding(c, Connector.SourceProperty);
                    BindingOperations.ClearBinding(c, Connector.DestinationProperty);
                    currentCanvas.Children.Remove(c);
                }

                selected.removeAll();
                currentCanvas.Children.Remove(selected);
            }
        }

        private void CreateNewTabCommand(object sender, ExecutedRoutedEventArgs e)
        {
            createNewTab(null);
        }
    }
}
