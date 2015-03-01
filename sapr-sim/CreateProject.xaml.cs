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
    /// <summary>
    /// Interaction logic for CreateProject.xaml
    /// </summary>
    public partial class CreateProject : Window
    {

        public CreateProject()
        {
            InitializeComponent();
        }

        private void Browse_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = dialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
                directory.Text = dialog.SelectedPath;
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(name.Text) && !String.IsNullOrWhiteSpace(directory.Text))
            {
                Project project = Project.Instance;
                project.ProjectName = name.Text;
                project.ProjectPath = directory.Text;

                if (createNewModel.IsChecked.Value)
                {
                    project.addProjectItem(new ProjectItem(modelName.Text));
                }

                DialogResult = true;
                this.Close();
            }
            else
                MessageBox.Show("Заполните все параметры", "Ошибка");
        }
    }
}
