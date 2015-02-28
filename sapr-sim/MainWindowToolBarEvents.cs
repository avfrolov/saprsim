using sapr_sim.Figures;
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
        private void CreateNewTab_Click(object sender, RoutedEventArgs e)
        {
            createNewTab(null);
        }

        private void SaveToFile_Click(object sender, RoutedEventArgs e)
        {
            // Configure save file dialog box
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments); // Default directory
            dlg.FileName = "Model"; // Default file name
            dlg.DefaultExt = ".ssm"; // Default file extension
            dlg.Filter = "SAPR-SIM models (.ssm)|*.ssm"; // Filter files by extension

            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result.Value)
            {
                using (FileStream filestream = new FileStream(dlg.FileName, FileMode.OpenOrCreate))
                {
                    new BinaryFormatter().Serialize(filestream, currentCanvas);
                }
                printInformation("Сохранение прошло успешно");
                printInformation("Сохранен файл " + dlg.FileName);
            }

            
        }

        private void OpenFromFile_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments); // Default directory
            dlg.FileName = "Model"; // Default file name
            dlg.DefaultExt = ".ssm"; // Default file extension
            dlg.Filter = "SAPR-SIM models (.ssm)|*.ssm"; // Filter files by extension

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result.Value)
            {
                using (FileStream fs = new FileStream(dlg.FileName, FileMode.Open))
                {
                    createNewTab((ScrollableCanvas)new BinaryFormatter().Deserialize(fs));
                }
                printInformation("Открыт файл " + dlg.FileName);
            }
            
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            DeleteShapeCommand(selected, null);
        }

    }
}
