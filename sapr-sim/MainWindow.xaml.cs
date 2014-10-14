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
using System.Windows.Navigation;
using System.Windows.Shapes;

using sapr_sim.WPFCustomElements;
using sapr_sim.Figures.Custom;
using sapr_sim.Figures.Basic;

namespace sapr_sim
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private Diagram diagram;
        private ComplexFigure currentFigure;
        private System.Drawing.Graphics gr;

        public MainWindow()
        {
            InitializeComponent();
            gr = System.Drawing.Graphics.FromHwnd(new System.Windows.Interop.WindowInteropHelper(this).Handle);
            createNewTab();
            diagram = new Diagram();
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            if (currentFigure != null && e.ButtonState == e.LeftButton)
            {              
                addComplexFigure(e.GetPosition(this));
                currentFigure.Draw(gr);
            }
            else if (e.ButtonState == e.RightButton)
            {
                currentFigure = null;
            }
            
        }

        private void CreateNewTab_Click(object sender, RoutedEventArgs e)
        {
            createNewTab();            
        }

        private void createNewTab()
        {
            ClosableTabItem theTabItem = new ClosableTabItem();
            theTabItem.Title = "Новая диаграмма " + (tabs.Items.Count + 1);
            tabs.Items.Add(theTabItem);
            theTabItem.Focus();
        }

        private void ArrowButton_Click(object sender, RoutedEventArgs e)
        {
            currentFigure = null;
        }

        private void BorderButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Вооообщееее не понятно зачем этот инструмент тут...");
        }
        
        private void LabelButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Скорее всего придется переписать этот компонент, т.к. он не ComplexFigure");
        }

        private void ProcedureButton_Click(object sender, RoutedEventArgs e)
        {
            currentFigure = new Procedure();
        }

        private void ResourceButton_Click(object sender, RoutedEventArgs e)
        {
            currentFigure = new Resource();
        }

        private void SyncButton_Click(object sender, RoutedEventArgs e)
        {
            currentFigure = new Sync();
        }

        private void addComplexFigure(Point location)
        {
            // backward Karimov's capability...
            System.Drawing.Point convertedPoint = new System.Drawing.Point((int)location.X, (int)location.Y);
            currentFigure.Location = convertedPoint;
            if (!(currentFigure is IBackgroundFigure))
            {
                diagram.Figures.Add(currentFigure);
            }
            else
            {
                //поиск более раниих фоновых фигур
                int i;
                for (i = 0; i < diagram.Figures.Count; i++)
                    if (!(diagram.Figures[i] is IBackgroundFigure))
                        break;
                diagram.Figures.Insert(i, currentFigure);
            }
        }

    }
}
