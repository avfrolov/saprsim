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
using System.Windows.Media.Effects;

namespace sapr_sim
{
    public partial class MainWindow : Window
    {

        private UIEntity currentEntity;
        private Canvas currentCanvas;

        // used in moving figure on canvas
        private bool captured = false;
        private double xShape, xCanvas, yShape, yCanvas;
        private UIEntity source = null;

        public MainWindow()
        {
            InitializeComponent();
            bindHotkeys();
            createNewTab();
        }

        private bool firstConnect;

        private void OnMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (currentEntity != null && e.LeftButton == MouseButtonState.Pressed)
            {
                if (currentEntity is Connector)
                {
                    // TODO need refactoring...
                    if (firstConnect)
                    {
                        currentEntity.SetBinding(Connector.SourceProperty, new Binding()
                        {
                            Source = source,
                            Path = new PropertyPath(UIEntity.AnchorPointProperty)
                        });
                        firstConnect = false;
                    }
                    else
                    {
                        currentEntity.SetBinding(Connector.DestinationProperty, new Binding()
                            {
                                Source = source,
                                Path = new PropertyPath(UIEntity.AnchorPointProperty)
                            });

                        Canvas currentCanvas = ((tabs.Items[tabs.SelectedIndex] as ClosableTabItem).Content as ScrollViewer).Content as ScrollableCanvas;                       
                        currentCanvas.Children.Add(currentEntity);
                        currentEntity = null;
                    }
                }
                else
                {
                    Point position = e.GetPosition(this);
                    attachMovingEvents(currentEntity);
                 
                    // 200 - width of tool panel (it's a constant in xaml)
                    // 100 - random +- value :)
                    Canvas.SetLeft(currentEntity, position.X - 200);
                    Canvas.SetTop(currentEntity, position.Y - 100);

                    currentCanvas.Children.Add(currentEntity);

                    currentEntity = null;
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
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl && tabs.SelectedIndex >= 0)
            {
                currentCanvas = ((tabs.Items[tabs.SelectedIndex] as ClosableTabItem).Content as ScrollViewer).Content as ScrollableCanvas;
            }
        }

        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (source != null && !captured)
            {
                source.BitmapEffect = UIEntity.defaultBitmapEffect(source);
                source = null;
            }
        }

        private void Shape_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // clear bitmap effect for previous selected entity
            if (source != null)
                source.BitmapEffect = UIEntity.defaultBitmapEffect(source);

            source = (UIEntity)sender;
            Mouse.Capture(source);
            captured = true;
            xShape = VisualTreeHelper.GetOffset(source).X;
            xCanvas = e.GetPosition(this).X;
            yShape = VisualTreeHelper.GetOffset(source).Y;
            yCanvas = e.GetPosition(this).Y;

            DropShadowBitmapEffect eff = new DropShadowBitmapEffect();
            eff.ShadowDepth = 0;
            eff.Color = Colors.Red;

            source.BitmapEffect = eff;
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
        
        private void LabelButton_Click(object sender, RoutedEventArgs e)
        {
            currentEntity = new sapr_sim.Figures.New.Label();
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
            currentEntity = new sapr_sim.Figures.New.Parallel(currentCanvas);
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
