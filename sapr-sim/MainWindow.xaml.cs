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

using sapr_sim.Figures.New;
using sapr_sim.WPFCustomElements;
using sapr_sim.Figures.Basic;

namespace sapr_sim
{
    public partial class MainWindow : Window
    {

        //private Diagram diagram;
        private UIEntity currentEntity;

        // used in moving figure on canvas
        private bool captured = false;
        private double xShape, xCanvas, yShape, yCanvas;
        private UIElement source = null;

        public MainWindow()
        {
            InitializeComponent();
            createNewTab();
            //diagram = new Diagram();
        }

        private void OnMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (currentEntity != null && e.LeftButton == MouseButtonState.Pressed)
            {
                Point position = e.GetPosition(this);
                attachMovingEvents(currentEntity);

                // dirty... but it's 146%
                // see MainWindow.createNewTab()
                Canvas currentCanvas = ((tabs.Items[tabs.SelectedIndex] as ClosableTabItem).Content as ScrollViewer).Content as ScrollableCanvas;
                             
                // 200 - width of tool panel (it's a constant in xaml)
                // 100 - random +- value :)
                Canvas.SetLeft(currentEntity, position.X - 200);
                Canvas.SetTop(currentEntity, position.Y - 100);

                currentCanvas.Children.Add(currentEntity);

                currentEntity = null;
            }            
        }

        private void CreateNewTab_Click(object sender, RoutedEventArgs e)
        {
            createNewTab();            
        }

        private void createNewTab()
        {
            ClosableTabItem theTabItem = new ClosableTabItem();
            ScrollViewer scrollViewer = new ScrollViewer();
            Canvas canvas = new ScrollableCanvas();

            canvas.Background = Brushes.Transparent;
            canvas.MouseDown += OnMouseDown;

            scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
            scrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Visible;
            scrollViewer.Content = canvas;

            theTabItem.Content = scrollViewer;
            theTabItem.Title = "Новая диаграмма " + (tabs.Items.Count + 1);
            tabs.Items.Add(theTabItem);
        }

        private void Shape_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            source = (UIElement)sender;
            Mouse.Capture(source);
            captured = true;
            xShape = VisualTreeHelper.GetOffset(source).X;
            xCanvas = e.GetPosition(this).X;
            yShape = VisualTreeHelper.GetOffset(source).Y;
            yCanvas = e.GetPosition(this).Y;
        }

        private void Shape_MouseMove(object sender, MouseEventArgs e)
        {
            if (captured)
            {
                double x = e.GetPosition(this).X;
                double y = e.GetPosition(this).Y;
                xShape += x - xCanvas;
                Canvas.SetLeft(source, xShape);
                xCanvas = x;
                yShape += y - yCanvas;
                Canvas.SetTop(source, yShape);
                yCanvas = y;
            }
        }

        private void Shape_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Mouse.Capture(null);
            captured = false;
        }

        private void ArrowButton_Click(object sender, RoutedEventArgs e)
        {
            currentEntity = null;
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
            currentEntity = new Procedure();
        }

        private void ResourceButton_Click(object sender, RoutedEventArgs e)
        {
            currentEntity = null;
        }

        private void SyncButton_Click(object sender, RoutedEventArgs e)
        {
            currentEntity = null;
        }

        private void MultithreadButton_Click(object sender, RoutedEventArgs e)
        {
            currentEntity = null;
        }

        private void DecisionButton_Click(object sender, RoutedEventArgs e)
        {
            currentEntity = null;
        }

        private void CollectorButton_Click(object sender, RoutedEventArgs e)
        {
            currentEntity = null;
        }

        private void EntitySourceButton_Click(object sender, RoutedEventArgs e)
        {
            currentEntity = null;
        }

        private void EntityDestinationButton_Click(object sender, RoutedEventArgs e)
        {
            currentEntity = null;
        }

        private void attachMovingEvents(UIEntity entity)
        {
            entity.MouseLeftButtonDown += Shape_MouseLeftButtonDown;
            entity.MouseMove += Shape_MouseMove;
            entity.MouseLeftButtonUp += Shape_MouseLeftButtonUp;
        }

    }
}
