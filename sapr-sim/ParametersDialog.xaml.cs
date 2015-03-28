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

using sapr_sim.Parameters;
using sapr_sim.WPFCustomElements;

namespace sapr_sim
{
    
    public partial class ParameterDialog : Window
    {

        private List<UIParam> parameters = new List<UIParam>();
        private sapr_sim.Figures.UIEntity owner;

        public ParameterDialog(List<UIParam> param, sapr_sim.Figures.UIEntity owner)
        {
            InitializeComponent();
            this.parameters = param;
            this.owner = owner;

            ParameterProccesor.drawParameters(parameters, sp, true);
            addButtons();
        }

        private void btnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            foreach(UIElement el in sp.Children)
            {
                if (el is DockPanel)
                {
                    DockPanel dp = el as DockPanel;
                    Label l = dp.Children[0] as Label;
                    ParameterInput input = dp.Children[1] as ParameterInput;
                    UIParam param = parameters.Find(p => p.DisplayedText == l.Content.ToString());
                    if (!param.Validator.validate(input.getValue().ToString()))
                    {
                        MessageBox.Show("Параметр '" + l.Content + "' задан не верно");
                        return;
                    }
                    if (sapr_sim.Figures.UIEntity.ENTITY_NAME_PARAM.Equals(param.DisplayedText) && !param.RawValue.Equals(input.getValue().ToString()))
                        owner.updateText(input.getValue().ToString());
                    param.RawValue = input.getValue();
                    //if (dp.Children[1] is TextBox)
                    //{
                    //    TextBox tb = dp.Children[1] as TextBox;
                    //    UIParam param = parameters.Find(p => p.DisplayedText == l.Content.ToString());
                    //    if (!param.Validator.validate(tb.Text)) 
                    //    {
                    //        MessageBox.Show("Параметр '" + l.Content + "' задан не верно");
                    //        return;
                    //    }                    
                    //    if (sapr_sim.Figures.UIEntity.ENTITY_NAME_PARAM.Equals(param.DisplayedText) && !param.RawValue.Equals(tb.Text))
                    //        owner.updateText(tb.Text);
                    //    param.RawValue = tb.Text;
                    //}

                    //if (dp.Children[1] is CheckBox)
                    //{
                    //    CheckBox cb = dp.Children[1] as CheckBox;
                    //    UIParam param = parameters.Find(p => p.DisplayedText == l.Content.ToString());
                    //    if (!param.Validator.validate(""))
                    //    {
                    //        MessageBox.Show("Параметр '" + l.Content + "' задан не верно");
                    //        return;
                    //    }
                    //    param.RawValue = cb.IsChecked;
                    //}
                }
            }

            this.Close();
            ((MainWindow)System.Windows.Application.Current.MainWindow).ModelChanged();
        }

        private void addButtons()
        {
            WrapPanel wp = new WrapPanel() { HorizontalAlignment = HorizontalAlignment.Right, Margin = new Thickness(0, 15, 0, 0) };
            Button ok = new Button() { IsDefault = true, Name = "btnDialogOk", MinWidth = 60, Margin = new Thickness(0, 0, 10, 0), Content = "Ок" };
            Button cancel = new Button() { IsCancel = true, MinWidth = 60, Content = "Отмена" };
            ok.Click += btnDialogOk_Click;
            wp.Children.Add(ok);
            wp.Children.Add(cancel);
            sp.Children.Add(wp);
        }
    }

    public static class ParameterProccesor
    {
        public static void drawParameters(List<UIParam> parameters, StackPanel drawPanel, bool paramsEnabled)
        {
            if (parameters != null)
            {
                drawPanel.Children.Clear();
                foreach (UIParam entry in parameters)
                {

                    DockPanel sprow = new DockPanel() { LastChildFill = true, Margin = new Thickness(2, 2, 2, 5) };
                    Label l = new Label() { Content = entry.DisplayedText };
                    ParameterInput input = entry.ContentControl;
                    UIElement uiControl = null;

                    if (input == null)
                    {
                        input = new ParameterTextBox(entry.RawValue, paramsEnabled);
                        uiControl = input as UIElement;
                    }
                    else
                    {
                        input.setValue(entry.RawValue);
                        input = input.Clone() as ParameterInput;
                        uiControl = input as UIElement;
                        uiControl.IsEnabled = paramsEnabled;
                    }
                    
                    sprow.Children.Add(l);
                    sprow.Children.Add(uiControl);
                    drawPanel.Children.Add(sprow);
                }
            }
        }
    }
}
