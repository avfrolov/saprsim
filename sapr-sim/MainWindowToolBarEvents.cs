using sapr_sim.Figures;
using sapr_sim.Utils;
using sapr_sim.WPFCustomElements;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Xml;

namespace sapr_sim
{
    public partial class MainWindow : Window
    {

        private FileService fs = new FileService();

        // TODO refactor CreateNewProject_Click && OpenProject_Click
        private void CreateNewProject_Click(object sender, RoutedEventArgs e)
        {
            if (checkOnceProject())
            {
                CreateProject cp = new CreateProject();
                Nullable<bool> result = cp.ShowDialog();
                if (result.Value)
                {
                    Project prj = Project.Instance;

                    TreeViewItem projectItem = new TreeViewItem() { Header = prj.ProjectName };
                    projectStructure.Items.Add(projectItem);

                    if (prj.Items.Count > 0)
                    {
                        ProjectItem item = prj.Items[0];
                        createNewDiagram(null, item.Name);
                        item.Canvas = currentCanvas;

                        fs.saveProject();
                        fs.save(currentCanvas, prj.FullPath + "\\" + item.Name + FileService.PROJECT_ITEM_EXTENSION);

                        ProjectTreeViewItem newModel = new ProjectTreeViewItem() { Header = item.Name };
                        newModel.ProjectItem = item;
                        projectItem.Items.Add(newModel);
                        projectItem.IsExpanded = true;
                        attachProjectItemEvents(newModel);
                    }
                    else
                        fs.saveProject();

                    ButtonsActivation(true);
                    attachProjectEvents(projectItem);
                }
            }
        }

        private void OpenProject_Click(object sender, RoutedEventArgs e)
        {
            if (checkOnceProject())
            {
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments); // Default directory
                dlg.DefaultExt = FileService.PROJECT_EXTENSION; // Default file extension
                dlg.Filter = "SAPR-SIM project (.ssp)|*.ssp"; // Filter files by extension

                // Show open file dialog box
                Nullable<bool> result = dlg.ShowDialog();

                // Process open file dialog box results
                if (result.Value)
                {
                    printInformation("Открыт проект " + dlg.FileName);
                    fs.openProject(dlg.FileName);

                    Project prj = Project.Instance;

                    TreeViewItem projectItem = new TreeViewItem() { Header = prj.ProjectName };
                    projectStructure.Items.Add(projectItem);

                    if (prj.Items.Count > 0)
                    {
                        printInformation("Количество диаграмм в проекте: " + prj.Items.Count);
                        foreach (ProjectItem item in prj.Items)
                        {
                            createNewDiagram(item.Canvas, item.Name);
                            item.Canvas = currentCanvas;
                            ProjectTreeViewItem tvi = new ProjectTreeViewItem() { Header = item.Name };
                            tvi.ProjectItem = item;
                            attachProjectItemEvents(tvi);
                            projectItem.Items.Add(tvi);
                            printInformation("Открыта диаграмма : " + item.Name);
                        }
                        projectItem.IsExpanded = true;
                        attachProjectEvents(projectItem);
                    }
                    ButtonsActivation(true);
                }
            }
        }

        private void CreateNewDiagram_Click(object sender, RoutedEventArgs e)
        {
            createNewDiagram(null);
            ProjectItem newItem = new ProjectItem(currentCanvas, (tabs.SelectedItem as ClosableTabItem).Title);
            Project.Instance.addProjectItem(newItem);
            TreeViewItem root = projectStructure.Items[0] as TreeViewItem;
            ProjectTreeViewItem ptvi = new ProjectTreeViewItem() { Header = newItem.Name, ProjectItem = newItem };
            attachProjectItemEvents(ptvi);
            root.Items.Add(ptvi);
            fs.save(newItem.Canvas, newItem.FullPath);
            fs.saveProject();
        }

        private void CloseProject_Click(object sender, RoutedEventArgs e)
        {
            if (needSave())
            {
                MessageBoxResult result = MessageBox.Show("Имеются не сохраненные данные. Сохранить изменения перед закрытием?",
                    "Предупреждение", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);

                switch (result)
                {
                    case MessageBoxResult.Yes:
                        SaveAll_Click(null, null);
                        break;
                    case MessageBoxResult.No:
                        break;
                    case MessageBoxResult.Cancel:
                        return;
                }
            }

            Project.Instance.Close();
            tabs.Items.Clear();
            propertiesPanel.Children.Clear();
            selected = null;
            projectStructure.Items.Clear();
            ButtonsActivation(false);
        }

        private void SaveAs_Click(object sender, RoutedEventArgs e)
        {
            // Configure save file dialog box
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.InitialDirectory = Project.Instance.FullPath; // Default directory
            dlg.DefaultExt = FileService.PROJECT_ITEM_EXTENSION; // Default file extension
            dlg.Filter = "SAPR-SIM models (.ssm)|*.ssm"; // Filter files by extension

            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result.Value)
            {
                fs.save(currentCanvas, dlg.FileName);
                printInformation("Сохранение прошло успешно");
                printInformation("Сохранен файл " + dlg.FileName);
                changeTabName(Path.GetFileName(dlg.FileName));
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            ProjectItem item = Project.Instance.byCanvas(currentCanvas);
            fs.save(currentCanvas, item.FullPath);
            printInformation("Сохранение прошло успешно");
            printInformation("Сохранен файл " + item.FullPath);
            changeTabName(item.Name);
        }

        private void SaveAll_Click(object sender, RoutedEventArgs e)
        {
            foreach(ProjectItem item in Project.Instance.Items)
            {
                fs.save(item.Canvas, item.FullPath);                
                changeTabName(findTabItem(item), item.Name);
            }
            printInformation("Все измененные файлы сохранены");
        }

        private ClosableTabItem findTabItem(ProjectItem item)
        {
            foreach(ClosableTabItem ti in tabs.Items)
            {
                ScrollableCanvas sc = (ti.Content as ScrollViewer).Content as ScrollableCanvas;
                ProjectItem pi = Project.Instance.byCanvas(sc);
                if (pi.Equals(item))
                    return ti;
            }
            return null;
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.InitialDirectory = Project.Instance.FullPath; // Default directory
            dlg.DefaultExt = FileService.PROJECT_ITEM_EXTENSION; // Default file extension
            dlg.Filter = "SAPR-SIM models (.ssm)|*.ssm"; // Filter files by extension

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result.Value)
            {
                string filename = Path.GetFileNameWithoutExtension(dlg.FileName);
                bool piExist = Project.Instance.Items.Find(x => x.Name == filename) != null;
                if (!piExist)
                {
                    createNewDiagram(fs.open(dlg.FileName));
                    printInformation("Открыт файл " + dlg.FileName);
                    changeTabName(filename);

                    ProjectItem newItem = new ProjectItem(currentCanvas, filename);
                    Project.Instance.addProjectItem(newItem);
                    TreeViewItem root = projectStructure.Items[0] as TreeViewItem;

                    ProjectTreeViewItem ptvi = new ProjectTreeViewItem() { Header = newItem.Name, ProjectItem = newItem };
                    attachProjectItemEvents(ptvi);
                    root.Items.Add(ptvi);

                    fs.save(newItem.Canvas, newItem.FullPath);
                    fs.saveProject();
                }
                else
                    MessageBox.Show("Файл уже есть в проекте");
            }            
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            DeleteShapeCommand(selected, null);
        }

        private bool checkOnceProject()
        {
            if (projectStructure.Items.Count > 0)
            {
                MessageBoxResult result = MessageBox.Show("Закрыть проект?",
                    "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes) CloseProject_Click(null, null);
                if (result == MessageBoxResult.No) return false;
            }
            return true;
        }

    }
}
