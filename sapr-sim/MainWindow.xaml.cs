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

        private readonly string defaultTabName = "Новая диаграмма ";
        private readonly string modelChangedPostfix = "* "; 

        public MainWindow()
        {
            InitializeComponent();
            bindHotkeys();
        }

        private void Application_ClosingEvent(object sender, System.ComponentModel.CancelEventArgs e)
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
                        e.Cancel = true;
                        break;
                }
            }
        }

        private void OnMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (currentEntity != null && e.LeftButton == MouseButtonState.Pressed)
            {
                if (currentEntity is ConnectionLine && selected != null && selected is Port)
                {
                    connect();
                }
                else if (currentEntity is SubDiagram)
                {
                    SubDiagram sd = currentEntity as SubDiagram;
                    ScrollableCanvas sc = currentCanvas as ScrollableCanvas;
                    if (sc.Equals(sc, sd.ProjectItem.Canvas as ScrollableCanvas))
                    {
                        currentEntity = null;
                        MessageBox.Show("Нельзя добавить процесс к самому себе");
                    }
                    else
                    {
                        sd.canvas = currentCanvas;
                        drawOnCanvas(e.GetPosition(this));
                    }
                }
                else if (!(currentEntity is ConnectionLine))
                {
                    drawOnCanvas(e.GetPosition(this));
                }                
            }
        }

        private void createNewDiagram(Canvas canvas, string tabName)
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
            theTabItem.Title = tabName;
            tabs.Items.Add(theTabItem);
            tabs.SelectionChanged += TabControl_SelectionChanged;

            attachContextMenu(canvas);

            currentCanvas = canvas;
        }

        private void createNewDiagram(Canvas canvas)
        {
            createNewDiagram(canvas, defaultTabName + (tabs.Items.Count + 1));
        }

        private bool canConnect()
        {
            BindingExpression srcExp = currentEntity.GetBindingExpression(ConnectionLine.SourceProperty);
            BindingExpression dstExp = currentEntity.GetBindingExpression(ConnectionLine.DestinationProperty);
            if (dstExp == null)
            {
                //return !(selected as Port).Owner.Equals((srcExp.DataItem as Port).Owner);
                return !selected.Equals((selected as Port).Owner, (srcExp.DataItem as Port).Owner);
            }
            return !selected.Equals(selected, srcExp.DataItem as UIEntity) && !selected.Equals(selected, dstExp.DataItem as UIEntity);
            //return !selected.Equals(srcExp.DataItem) && !selected.Equals(dstExp.DataItem);
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
                ModelChanged();
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
            ModelChanged();
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
            // Garantee that we have only one event http://stackoverflow.com/questions/136975/has-an-event-handler-already-been-added
            entity.MouseLeftButtonDown -= Shape_MouseLeftButtonDown;
            entity.MouseLeftButtonDown += Shape_MouseLeftButtonDown;
            entity.MouseMove -= Shape_MouseMove;
            entity.MouseMove += Shape_MouseMove;
            entity.MouseLeftButtonUp -= Shape_MouseLeftButtonUp;
            entity.MouseLeftButtonUp += Shape_MouseLeftButtonUp;
            entity.MouseRightButtonDown -= Shape_MouseRightButtonDown;
            entity.MouseRightButtonDown += Shape_MouseRightButtonDown;

            attachContextMenu(entity);
        }        

        private void printInformation(string info)
        {
            infoTextBlock.Text += info + Environment.NewLine;
        }

        private void changeTabName(ClosableTabItem ti, string newName)
        {
            if (ti != null)           
                ti.Title = newName;
        }

        private void changeTabName(string newName)
        {
            changeTabName(tabs.SelectedItem as ClosableTabItem, newName);
        }

        private void findAndOpenRelatedTab(ProjectItem pi)
        {
            ClosableTabItem cti = findTabItem(pi);
            if (cti == null)
                createNewDiagram(fs.open(pi.FullPath), pi.Name);
            else
                cti.IsSelected = true;
        }

        public void ModelChanged()
        {
            ClosableTabItem ti = tabs.SelectedItem as ClosableTabItem;
            if (!ti.Title.Contains(modelChangedPostfix))
                changeTabName(modelChangedPostfix + ti.Title);
        }

        public bool IsModelChanged()
        {
            return IsModelChanged(tabs.SelectedItem as ClosableTabItem);
        }

        public bool IsModelChanged(ClosableTabItem ti)
        {
            return ti.Title.Contains(modelChangedPostfix);
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

        // TODO think about button activition design
        private void ButtonsActivation(bool activate)
        {
            // Файл
            newDiagramButton.IsEnabled = activate;
            openDiagramButton.IsEnabled = activate;
            saveButton.IsEnabled = activate;
            saveAsButton.IsEnabled = activate;
            saveAllButton.IsEnabled = activate;                   
            closeProjectButton.IsEnabled = activate;

            // Правка
            cutButton.IsEnabled = activate;
            copyButton.IsEnabled = activate;
            pasteButton.IsEnabled = activate;
            deleteButton.IsEnabled = activate;

            // Модель
            runSimulationButton.IsEnabled = activate;
            projectSettingsButton.IsEnabled = activate;

            // tool buttons
            NewDiagram.IsEnabled = activate;
            OpenDiagram.IsEnabled = activate;
            SaveDiagram.IsEnabled = activate;
            Cut.IsEnabled = activate;
            Copy.IsEnabled = activate;
            Paste.IsEnabled = activate;
            runSimulationToolButton.IsEnabled = activate;
        }

        private bool needSave()
        {
            foreach (ClosableTabItem i in tabs.Items)
                if (IsModelChanged(i)) return true;
            return false;
        }

    }
}
