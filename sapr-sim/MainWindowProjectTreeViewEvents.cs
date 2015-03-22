using sapr_sim.Utils;
using sapr_sim.WPFCustomElements;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace sapr_sim
{
    public partial class MainWindow : Window
    {

        private void attachProjectEvents(TreeViewItem root)
        {
            root.PreviewMouseRightButtonDown += OnPreviewMouseRightButtonDown;

            ContextMenu menu = new ContextMenu();

            MenuItem addNewDgm = new MenuItem() { Header = "Добавить новую диаграмму" };
            addNewDgm.Click += AddNewDiagram_RootMenuClick;
            menu.Items.Add(addNewDgm);

            MenuItem openDgm = new MenuItem() { Header = "Открыть диаграмму" };
            openDgm.Click += OpenDiagram_RootMenuClick;
            menu.Items.Add(openDgm);

            menu.Items.Add(new MenuItem() { Header = "Запустить моделирование", IsEnabled = false });

            MenuItem renameRoot = new MenuItem() { Header = "Переименовать" };
            renameRoot.Click += Rename_RootMenuClick;
            menu.Items.Add(renameRoot);

            menu.Items.Add(new MenuItem() { Header = "Свойства", IsEnabled = false });
            root.ContextMenu = menu;
        }

        private void attachProjectItemEvents(TreeViewItem item)
        {
            item.MouseDoubleClick += OnProjectItemMouseDoubleClick;
            item.PreviewMouseRightButtonDown += OnPreviewMouseRightButtonDown;

            ContextMenu menu = new ContextMenu();
            menu.Items.Add(new MenuItem() { Header = "Добавить на диаграмму", IsEnabled = false });

            MenuItem runSimulationItem = new MenuItem() { Header = "Запустить моделирование" };
            runSimulationItem.Click += RunSimulation_MenuClick;
            menu.Items.Add(runSimulationItem);

            MenuItem removeItem = new MenuItem() { Header = "Убрать из проекта" };
            removeItem.Click += RemoveItem_MenuClick;
            menu.Items.Add(removeItem);

            MenuItem renameItem = new MenuItem() { Header = "Переименовать" };
            renameItem.Click += Rename_RootMenuClick;
            menu.Items.Add(renameItem);

            menu.Items.Add(new MenuItem() { Header = "Свойства", IsEnabled = false });
            item.ContextMenu = menu;
        }

        private void OnProjectItemMouseDoubleClick(object sender, MouseButtonEventArgs args)
        {
            ProjectTreeViewItem tvi = sender as ProjectTreeViewItem;
            ClosableTabItem cti = findTabItem(tvi.ProjectItem);
            if (cti == null)
                createNewDiagram(tvi.ProjectItem.Canvas, tvi.ProjectItem.Name);
            else
                cti.IsSelected = true;
        }

        private void AddNewDiagram_RootMenuClick(object sender, RoutedEventArgs e)
        {
            CreateNewDiagram_Click(sender, e);
        }

        private void OpenDiagram_RootMenuClick(object sender, RoutedEventArgs e)
        {
            Open_Click(null, null);
        }

        private void RunSimulation_MenuClick(object sender, RoutedEventArgs e)
        {
            ProjectTreeViewItem item = projectStructure.SelectedItem as ProjectTreeViewItem;
            ClosableTabItem cti = findTabItem(item.ProjectItem);
            if (cti == null)
                createNewDiagram(item.ProjectItem.Canvas, item.ProjectItem.Name);
            else
                cti.IsSelected = true;
            SimulateButton_Click(null, null);
        }

        private void RemoveItem_MenuClick(object sender, RoutedEventArgs e)
        {
            ProjectTreeViewItem item = projectStructure.SelectedItem as ProjectTreeViewItem;

            ClosableTabItem cti = findTabItem(item.ProjectItem);
            if (cti != null) cti.button_close_Click(null, null);

            (projectStructure.Items[0] as TreeViewItem).Items.Remove(item);
            Project.Instance.Items.Remove(item.ProjectItem);
            fs.saveProject();
        }
        
        private void Rename_RootMenuClick(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = projectStructure.SelectedItem as TreeViewItem;
            string name = item.Header as string;
            TextBox tb = new TextBox() { Text = name };
            tb.KeyDown += TextBox_KeyUp;
            tb.LostFocus += TextBox_LostFocus;

            tb.IsVisibleChanged += new DependencyPropertyChangedEventHandler(TextBox_IsVisibleChanged);

            tb.CaretIndex = tb.Text.Length;
            item.Header = tb;
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = projectStructure.SelectedItem as TreeViewItem;
            if (item.Header is TextBox)
                renameTreeViewItem(sender);
        }

        private void TextBox_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key != System.Windows.Input.Key.Enter) return;
            e.Handled = true;
            renameTreeViewItem(sender);
        }

        private void OnPreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            TreeViewItem treeViewItem = VisualUpwardSearch(e.OriginalSource as DependencyObject);
            e.Handled = true;
            if (treeViewItem != null)
            {
                treeViewItem.Focus();
                e.Handled = true;
            }
        }

        private TreeViewItem VisualUpwardSearch(DependencyObject source)
        {
            while (source != null && !(source is TreeViewItem))
                source = VisualTreeHelper.GetParent(source);

            return source as TreeViewItem;
        }

        private void renameTreeViewItem(object sender)
        {
            string newName = (sender as TextBox).Text;
            if (String.IsNullOrWhiteSpace(newName))
            {
                MessageBox.Show("Введено некорректное имя");
                return;
            }
            TreeViewItem selected = projectStructure.SelectedItem as TreeViewItem;
            
            ProjectItem pi = Project.Instance.Items.Find(x => x.Name == newName);
            if (pi != null)
            {
                // true = avoiding NOT Renaming (when user clicked "Rename" and name not changed)
                if (selected is ProjectTreeViewItem && (selected as ProjectTreeViewItem).ProjectItem.Equals(pi))
                    selected.Header = newName;
                else
                    MessageBox.Show("Диаграмма с таким именем уже подключена");
                return;
            }


            if (File.Exists(Project.Instance.FullPath + "\\" + newName + FileService.PROJECT_ITEM_EXTENSION))
            {
                MessageBoxResult result = MessageBox.Show("Файл с таким именем уже есть в проектной директории. Перезаписать файл?",
                    "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.No) return;
            }

            if (selected is ProjectTreeViewItem)
            {
                ProjectTreeViewItem item = selected as ProjectTreeViewItem;
                item.Header = newName;
                fs.renameProjectItem(item.ProjectItem, newName);
                changeTabName(findTabItem(item.ProjectItem), newName);
            }
            else 
            {
                TreeViewItem root = projectStructure.Items[0] as TreeViewItem;
                root.Header = newName;
                fs.renameProject(newName);
            }            
        }

        // fucking magic for set focus...
        // http://www.codeproject.com/Tips/478376/Setting-focus-to-a-control-inside-a-usercontrol-in
        private void TextBox_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue == true)
            {
                Dispatcher.BeginInvoke(
                DispatcherPriority.ContextIdle,
                new Action(delegate()
                {
                    (sender as TextBox).Focus();
                }));
            }
        }  
    }
}
