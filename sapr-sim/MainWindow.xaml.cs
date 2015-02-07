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

using sapr_sim.Figures;
using sapr_sim.WPFCustomElements;
using System.Windows.Media.Effects;
using sapr_sim.Parameters;

namespace sapr_sim
{
    public partial class MainWindow : Window
    {

        private UIEntity currentEntity;
        private Canvas currentCanvas;
       
        private bool firstConnect;

        public MainWindow()
        {
            InitializeComponent();
            bindHotkeys();
            createNewTab();
        }

        private void OnMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (currentEntity != null && e.LeftButton == MouseButtonState.Pressed)
            {
                if (currentEntity is Connector && selected != null && selected is Port)
                {
                    connect();
                }
                else if (!(currentEntity is Connector))
                {
                    drawOnCanvas(e.GetPosition(this));
                }                
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
            canvas.MouseLeftButtonDown += Canvas_MouseLeftButtonDown;

            scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
            scrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Visible;
            scrollViewer.Content = canvas;

            theTabItem.Content = scrollViewer;
            theTabItem.Title = "Новая диаграмма " + (tabs.Items.Count + 1);            
            tabs.Items.Add(theTabItem);
            tabs.SelectionChanged += TabControl_SelectionChanged;

            currentCanvas = canvas;
        }

        private bool canConnect()
        {
            BindingExpression srcExp = currentEntity.GetBindingExpression(Connector.SourceProperty);
            BindingExpression dstExp = currentEntity.GetBindingExpression(Connector.DestinationProperty);
            if (dstExp == null)
            {
                return !(selected as Port).Owner.Equals((srcExp.DataItem as Port).Owner);
            }
            return !selected.Equals(srcExp.DataItem) && !selected.Equals(dstExp.DataItem);
        }

        private void connect()
        {
            if (firstConnect)
            {
                setBinding(Connector.SourceProperty);
                firstConnect = false;
            }
            else if (canConnect())
            {
                setBinding(Connector.DestinationProperty);

                currentEntity.MouseLeftButtonDown += Shape_MouseLeftButtonDown;
                currentEntity.MouseLeftButtonUp += Shape_MouseLeftButtonUp;

                currentCanvas.Children.Add(currentEntity);
                currentEntity = null;
            }
        }

        private void setBinding(DependencyProperty dp)
        {
            currentEntity.SetBinding(dp, new Binding()
            {
                Source = selected,
                Path = new PropertyPath(Port.AnchorPointProperty)
            });
        }

        private void drawOnCanvas(Point position)
        {
            attachMovingEvents(currentEntity);

            // 200 - width of tool panel (it's a constant in xaml)
            // 100 - random +- value :)
            Canvas.SetLeft(currentEntity, position.X - 200);
            Canvas.SetTop(currentEntity, position.Y - 100);

            currentCanvas.Children.Add(currentEntity);
            currentEntity.createAndDrawPorts(position.X - 200, position.Y - 100);

            foreach (Port p in currentEntity.getPorts())
            {
                p.MouseLeftButtonDown += Shape_MouseLeftButtonDown;
                p.MouseLeftButtonUp += Shape_MouseLeftButtonUp;
            }

            currentEntity = null;
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl && tabs.SelectedIndex >= 0)
            {
                currentCanvas = ((tabs.Items[tabs.SelectedIndex] as ClosableTabItem).Content as ScrollViewer).Content as ScrollableCanvas;
            }
        }

        private void ArrowButton_Click(object sender, RoutedEventArgs e)
        {
            currentEntity = null;
        }
        
        private void LabelButton_Click(object sender, RoutedEventArgs e)
        {
            currentEntity = new sapr_sim.Figures.Label();
        }

        private void ProcedureButton_Click(object sender, RoutedEventArgs e)
        {
            currentEntity = new Procedure(currentCanvas);
        }

        private void ResourceButton_Click(object sender, RoutedEventArgs e)
        {
            currentEntity = new Resource(currentCanvas);
        }

        private void SyncButton_Click(object sender, RoutedEventArgs e)
        {
            currentEntity = new Sync(currentCanvas);
        }

        private void ParallelButton_Click(object sender, RoutedEventArgs e)
        {
            currentEntity = new sapr_sim.Figures.Parallel(currentCanvas);
        }

        private void DecisionButton_Click(object sender, RoutedEventArgs e)
        {
            currentEntity = new Decision(currentCanvas);
        }

        private void CollectorButton_Click(object sender, RoutedEventArgs e)
        {
            currentEntity = new Collector(currentCanvas);
        }

        private void EntitySourceButton_Click(object sender, RoutedEventArgs e)
        {
            currentEntity = new Source(currentCanvas);
        }

        private void EntityDestinationButton_Click(object sender, RoutedEventArgs e)
        {
            currentEntity = new Destination(currentCanvas);
        }

        private void LineButton_Click(object sender, RoutedEventArgs e)
        {
            currentEntity = new Connector(currentCanvas);
            firstConnect = true;
        }

        private void attachMovingEvents(UIEntity entity)
        {
            entity.MouseLeftButtonDown += Shape_MouseLeftButtonDown;
            entity.MouseMove += Shape_MouseMove;
            entity.MouseLeftButtonUp += Shape_MouseLeftButtonUp;
        }

    }
}
