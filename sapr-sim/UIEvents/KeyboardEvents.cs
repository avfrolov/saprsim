using sapr_sim.Figures;
using sapr_sim.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Shapes;
using System.Xml;

namespace sapr_sim
{
    public partial class MainWindow : Window
    {

        private void bindHotkeys()
        {
            try
            {
                // ****************************************************************************************************
                // File Commands
                RoutedCommand newTabBinding = new RoutedCommand();
                newTabBinding.InputGestures.Add(new KeyGesture(Key.N, ModifierKeys.Control));
                CommandBindings.Add(new CommandBinding(newTabBinding, CreateNewTabCommand, Hotkeys_CanExecute));

                RoutedCommand openFromFileBinding = new RoutedCommand();
                openFromFileBinding.InputGestures.Add(new KeyGesture(Key.O, ModifierKeys.Control));
                CommandBindings.Add(new CommandBinding(openFromFileBinding, OpenFromFileCommand, Hotkeys_CanExecute));

                RoutedCommand saveBinding = new RoutedCommand();
                saveBinding.InputGestures.Add(new KeyGesture(Key.S, ModifierKeys.Control));
                CommandBindings.Add(new CommandBinding(saveBinding, SaveCommand, Hotkeys_CanExecute));

                RoutedCommand saveAllBinding = new RoutedCommand();
                saveAllBinding.InputGestures.Add(new KeyGesture(Key.S, ModifierKeys.Shift | ModifierKeys.Control));
                CommandBindings.Add(new CommandBinding(saveAllBinding, SaveAllCommand, Hotkeys_CanExecute));

                // ****************************************************************************************************
                // Project Commands

                RoutedCommand runSimulationBinding = new RoutedCommand();
                runSimulationBinding.InputGestures.Add(new KeyGesture(Key.F5));
                CommandBindings.Add(new CommandBinding(runSimulationBinding, RunSimulationCommand, Hotkeys_CanExecute));

                // ****************************************************************************************************
                // Other Commands
                RoutedCommand closeTabBinding = new RoutedCommand();
                closeTabBinding.InputGestures.Add(new KeyGesture(Key.W, ModifierKeys.Control));
                CommandBindings.Add(new CommandBinding(closeTabBinding, CloseTabCommand, Hotkeys_CanExecute));

                RoutedCommand deleteBinding = new RoutedCommand();
                deleteBinding.InputGestures.Add(new KeyGesture(Key.Delete));
                CommandBindings.Add(new CommandBinding(deleteBinding, DeleteShapeCommand, Hotkeys_CanExecute));


                RoutedCommand copyBinding = new RoutedCommand();
                copyBinding.InputGestures.Add(new KeyGesture(Key.C, ModifierKeys.Control));
                CommandBindings.Add(new CommandBinding(copyBinding, CopyComand));

                RoutedCommand cutBinding = new RoutedCommand();
                cutBinding.InputGestures.Add(new KeyGesture(Key.X, ModifierKeys.Control));
                CommandBindings.Add(new CommandBinding(cutBinding, CutComand));

                RoutedCommand pasteBinding = new RoutedCommand();
                pasteBinding.InputGestures.Add(new KeyGesture(Key.V, ModifierKeys.Control));
                CommandBindings.Add(new CommandBinding(pasteBinding, PasteComand));
       
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
            deleteEntity();
        }

        private void deleteEntity()
        {
            if (selected != null && !(selected is Port))
            {
                List<ConnectionLine> connectors = ConnectorFinder.find(currentCanvas.Children, selected);
                foreach (ConnectionLine c in connectors)
                {
                    BindingOperations.ClearBinding(c, ConnectionLine.SourceProperty);
                    BindingOperations.ClearBinding(c, ConnectionLine.DestinationProperty);
                    currentCanvas.Children.Remove(c);
                }

                selected.removeAll();
                currentCanvas.Children.Remove(selected);
                ModelChanged();
                propertiesPanel.Children.Clear();
            }
        }

        private void Hotkeys_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (Project.Instance.IsLoaded)
            {
                e.CanExecute = true;
                e.ContinueRouting = true;
                e.Handled = true;
            }
        }

        private void CreateNewTabCommand(object sender, ExecutedRoutedEventArgs e)
        {
            CreateNewDiagram_Click(null, null);
        }

        private void CloseTabCommand(object sender, ExecutedRoutedEventArgs e)
        {
            (tabs.SelectedItem as sapr_sim.WPFCustomElements.ClosableTabItem).button_close_Click(null, null);
        }

        private void OpenFromFileCommand(object sender, ExecutedRoutedEventArgs e)
        {
            Open_Click(null, null);
        }

        private void SaveCommand(object sender, ExecutedRoutedEventArgs e)
        {
            Save_Click(null, null);
        }


        private void SaveAllCommand(object sender, ExecutedRoutedEventArgs e)
        {
            SaveAll_Click(null, null);
        }

        private void RunSimulationCommand(object sender, ExecutedRoutedEventArgs e)
        {
            SimulateButton_Click(null, null);
        }


        private void CopyComand(object sender, ExecutedRoutedEventArgs e)
        {
            if (sender != null && selected != null)
            {
                IDataObject dataObj = new DataObject();
                dataObj.SetData(DataFormats.Serializable, selected, false);
                Clipboard.SetDataObject(dataObj, false);
            }
        }

        private void CutComand(object sender, ExecutedRoutedEventArgs e)
        {
            if (sender != null && selected != null)
            {
                IDataObject dataObj = new DataObject();
                dataObj.SetData(DataFormats.Serializable, selected, false);
                Clipboard.SetDataObject(dataObj, false);

                deleteEntity();
            }
        }

        private void PasteComand(object sender, ExecutedRoutedEventArgs e)
        {
            IDataObject dataObj = Clipboard.GetDataObject();
            string format = typeof(UIEntity).FullName;
            format = DataFormats.Serializable;

            if (dataObj.GetDataPresent(format))
            {
                try
                {
                    UIEntity obj = (UIEntity)dataObj.GetData(format);
                    obj.canvas.Children.Clear();
                    obj.canvas = currentCanvas;

                    foreach (Port p in obj.getPorts())
                    {
                        p.canvas = currentCanvas;
                    }

                    currentEntity = obj;
                    drawOnCanvas(new Point(320, 120));

                    ZIndexUtil.setCorrectZIndex(currentCanvas, obj);
                }
                catch (Exception ex)
                {
                    Console.Out.WriteLine(ex);
                }

            }
        }

    }
}
