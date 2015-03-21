﻿using sapr_sim.Figures;
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

        private void CreateNewProject_Click(object sender, RoutedEventArgs e)
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
                    createNewTab(null, item.Name);
                    item.Canvas = currentCanvas;

                    fs.saveProject();
                    fs.save(currentCanvas, prj.FullPath + "\\" + item.Name + FileService.PROJECT_ITEM_EXTENSION);

                    TreeViewItem newModel = new TreeViewItem() { Header = item.Name };
                    projectItem.Items.Add(newModel);
                    projectItem.IsExpanded = true;
                }
                else
                    fs.saveProject();

                ButtonsActivation(true);
            }
        }

        private void OpenProject_Click(object sender, RoutedEventArgs e)
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
                        createNewTab(item.Canvas, item.Name);
                        item.Canvas = currentCanvas;
                        projectItem.Items.Add(new TreeViewItem() { Header = item.Name });
                        printInformation("Открыта диаграмма : " + item.Name);
                    }
                    projectItem.IsExpanded = true;
                }
            }
            ButtonsActivation(true);
        }

        private void CreateNewTab_Click(object sender, RoutedEventArgs e)
        {
            createNewTab(null);
        }

        private void CloseProject_Click(object sender, RoutedEventArgs e)
        {
            bool needSave = false;
            foreach (ClosableTabItem i in tabs.Items)
            {
                needSave = IsModelChanged(i);
                if (needSave) break;
            }

            if (needSave)
            {
                MessageBoxResult result = MessageBox.Show("Имеются не сохраненные данные. Сохранить изменения перед закрытием?",
                    "Предупреждение", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);

                switch (result)
                {
                    case MessageBoxResult.Yes:
                        // saveAll
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

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments); // Default directory
            dlg.DefaultExt = FileService.PROJECT_ITEM_EXTENSION; // Default file extension
            dlg.Filter = "SAPR-SIM models (.ssm)|*.ssm"; // Filter files by extension

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result.Value)
            {
                createNewTab(fs.open(dlg.FileName));
                printInformation("Открыт файл " + dlg.FileName);
                changeTabName(Path.GetFileName(dlg.FileName));
            }            
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            DeleteShapeCommand(selected, null);
        }

    }
}
