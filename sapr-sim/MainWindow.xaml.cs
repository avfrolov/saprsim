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
                if (currentEntity is Connector && source != null && source is Port)
                {

                    // TODO need refactoring...
                    if (firstConnect)
                    {
                        currentEntity.SetBinding(Connector.SourceProperty, new Binding()
                        {
                            Source = source,
                            Path = new PropertyPath(Port.AnchorPointProperty)
                        });
                        firstConnect = false;
                    }
                    else if (canConnect())
                    {
                        currentEntity.SetBinding(Connector.DestinationProperty, new Binding()
                        {
                            Source = source,
                            Path = new PropertyPath(Port.AnchorPointProperty)
                        });

                        currentEntity.MouseLeftButtonDown += Shape_MouseLeftButtonDown;
                        currentEntity.MouseLeftButtonUp += Shape_MouseLeftButtonUp;

                        currentCanvas.Children.Add(currentEntity);
                        currentEntity = null;                        
                    }
                }
                else if (!(currentEntity is Connector))
                {
                    Point position = e.GetPosition(this);
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
                return !(source as Port).Owner.Equals((srcExp.DataItem as Port).Owner);
            }
            return !source.Equals(srcExp.DataItem) && !source.Equals(dstExp.DataItem);
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
            if (e.ClickCount == 1)
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

                foreach (Port p in source.getPorts())
                {
                    source.putMovingCoordinate(
                        p,
                        VisualTreeHelper.GetOffset(p).X,
                        VisualTreeHelper.GetOffset(p).Y,
                        xCanvas, yCanvas);
                }

                if (!(source is Connector && source is Port))
                    source.BitmapEffect = new DropShadowBitmapEffect() { ShadowDepth = 0, Color = Colors.Red };
            }
            else if (e.ClickCount == 2 && !(sender is Port || sender is Connector))
            {
                UIEntity ent = sender as UIEntity;
                new ParameterDialog(ent.getParams(), ent).ShowDialog();
            }
        }

        private void Shape_MouseMove(object sender, MouseEventArgs e)
        {
            if (captured)
            {
                double x = e.GetPosition(this).X;
                double y = e.GetPosition(this).Y;

                xShape += x - xCanvas;
                yShape += y - yCanvas;
                Canvas.SetLeft(source, xShape);
                Canvas.SetTop(source, yShape);
                
                foreach (Port p in source.getPorts())
                {
                    UIEntity.CoordinatesHandler ch = source.getMovingCoordinate(p);
                    ch.xShape += x - ch.xCanvas;
                    ch.yShape += y - ch.yCanvas;
                    Canvas.SetLeft(p, ch.xShape);
                    Canvas.SetTop(p, ch.yShape);
                    ch.xCanvas = x;
                    ch.yCanvas = y;
                }

                xCanvas = x;
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
