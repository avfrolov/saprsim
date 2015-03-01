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
using sapr_sim.Utils;

namespace sapr_sim
{
    public partial class MainWindow : Window
    {

        private UIEntity currentEntity;
        private Canvas currentCanvas;
       
        private bool firstConnect;

        // 300 - width of left panel (it's a constant in xaml)
        // 100 - random +- value :)
        private const int X_OFFSET = -300;
        private const int Y_OFFSET = -100;

        public MainWindow()
        {
            InitializeComponent();
            bindHotkeys();
        }

        private void OnMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (currentEntity != null && e.LeftButton == MouseButtonState.Pressed)
            {
                if (currentEntity is ConnectionLine && selected != null && selected is Port)
                {
                    connect();
                }
                else if (!(currentEntity is ConnectionLine))
                {
                    drawOnCanvas(e.GetPosition(this));
                }                
            }
        }

        private void createNewTab(Canvas canvas)
        {
            if (canvas == null) canvas = new ScrollableCanvas();

            ClosableTabItem theTabItem = new ClosableTabItem();
            ScrollViewer scrollViewer = new ScrollViewer();            

            canvas.Background = Brushes.Transparent;
            canvas.MouseDown += OnMouseDown;
            canvas.MouseLeftButtonDown += Canvas_MouseLeftButtonDown;

            scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
            scrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Visible;
            scrollViewer.Content = canvas;

            theTabItem.IsSelected = true;
            theTabItem.Content = scrollViewer;
            theTabItem.Title = "Новая диаграмма " + (tabs.Items.Count + 1);            
            tabs.Items.Add(theTabItem);
            tabs.SelectionChanged += TabControl_SelectionChanged;

            currentCanvas = canvas;
        }

        private bool canConnect()
        {
            BindingExpression srcExp = currentEntity.GetBindingExpression(ConnectionLine.SourceProperty);
            BindingExpression dstExp = currentEntity.GetBindingExpression(ConnectionLine.DestinationProperty);
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
                setBinding(ConnectionLine.SourceProperty);
                firstConnect = false;
            }
            else if (canConnect())
            {
                setBinding(ConnectionLine.DestinationProperty);

                currentEntity.MouseLeftButtonDown += Shape_MouseLeftButtonDown;
                currentEntity.MouseLeftButtonUp += Shape_MouseLeftButtonUp;

                ZIndexUtil.setCorrectZIndex(currentCanvas, currentEntity);

                currentCanvas.Children.Add(currentEntity);
                currentEntity = null;
                canvasChanged();
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

            Canvas.SetLeft(currentEntity, position.X + X_OFFSET);
            Canvas.SetTop(currentEntity, position.Y + Y_OFFSET);

            currentCanvas.Children.Add(currentEntity);
            currentEntity.createAndDraw(position.X + X_OFFSET, position.Y + Y_OFFSET);

            ZIndexUtil.setCorrectZIndex(currentCanvas, currentEntity);

            foreach (Port p in currentEntity.getPorts())
            {
                p.MouseLeftButtonDown += Shape_MouseLeftButtonDown;
                p.MouseLeftButtonUp += Shape_MouseLeftButtonUp;
            }

            currentEntity = null;
            canvasChanged();
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
            currentEntity = new sapr_sim.Figures.Label(currentCanvas);
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
            currentEntity = new ConnectionLine(currentCanvas);
            firstConnect = true;
        }

        public void attachMovingEvents(UIEntity entity)
        {
            entity.MouseLeftButtonDown += Shape_MouseLeftButtonDown;
            entity.MouseMove += Shape_MouseMove;
            entity.MouseLeftButtonUp += Shape_MouseLeftButtonUp;
        }        

        private void printInformation(string info)
        {
            infoTextBlock.Text += info + Environment.NewLine;
        }

        private void changeTabName(string newName)
        {
            ClosableTabItem ti = tabs.SelectedItem as ClosableTabItem;
            ti.Title = newName;
        }

        private void canvasChanged()
        {
            ClosableTabItem ti = tabs.SelectedItem as ClosableTabItem;
            if (!ti.Title.Contains("* "))
                changeTabName("* " + ti.Title);
        }

        private void errorsListBox_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            object selected = (sender as ListBox).SelectedItem;

            // WTF? avoiding click for empty listbox
            if (selected != null)
            {
                resetUIShadows(currentCanvas.Children);
                ListBoxItemError error = selected as ListBoxItemError;
                if (error.FailedEntities != null)
                {
                    foreach(UIEntity ent in error.FailedEntities)
                    {
                        ent.errorBitmapEffect();
                    }
                }
            }
        }

    }
}
