using sapr_sim.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Shapes;

namespace sapr_sim.Figures
{
    public abstract class UIEntity : Shape
    {

        public static readonly string ENTITY_NAME_PARAM = "Имя";

        protected Canvas canvas;
        protected Dictionary<UIEntity, CoordinatesHandler> coordinates = new Dictionary<UIEntity, CoordinatesHandler>();
        protected List<Port> ports = new List<Port>();

        protected Label label;

        protected UIParam<String> textParam = new UIParam<String>("Сущность", new StringParamValidator(), ENTITY_NAME_PARAM);

        public static BitmapEffect defaultBitmapEffect(UIElement element)
        {
            // http://stackoverflow.com/questions/4022746/wpf-add-a-dropshadow-effect-to-an-element-from-code-behind
            DropShadowBitmapEffect myDropShadowEffect = new DropShadowBitmapEffect();
            // Set the color of the shadow to Black.
            Color myShadowColor = new Color();
            myShadowColor.ScA = 1;
            myShadowColor.ScB = 0;
            myShadowColor.ScG = 0;
            myShadowColor.ScR = 0;
            myDropShadowEffect.Color = myShadowColor;

            // Set the direction of where the shadow is cast to 320 degrees.
            myDropShadowEffect.Direction = 320;

            // Set the depth of the shadow being cast.
            myDropShadowEffect.ShadowDepth = 25;

            // Set the shadow softness to the maximum (range of 0-1).
            myDropShadowEffect.Softness = 1;
            // Set the shadow opacity to half opaque or in other words - half transparent.
            // The range is 0-1.
            myDropShadowEffect.Opacity = 0.5;

            return myDropShadowEffect;
        }

        public void putMovingCoordinate(UIEntity entity, double xShape, double yShape, double xCanvas, double yCanvas)
        {
            // avoiding 'System.ArgumentException' for existing key... double facepalm!!!
            coordinates.Remove(entity);
            coordinates.Add(entity, new CoordinatesHandler(xShape, yShape, xCanvas, yCanvas));
        }

        public CoordinatesHandler getMovingCoordinate(UIEntity entity)
        {
            return coordinates[entity];
        }

        public void updateText(string newText)
        {
            if (label != null)
            {
                canvas.Children.Remove(label);
                CoordinatesHandler ch = getMovingCoordinate(label);
                label = new Label(this, canvas, ch.xShape, ch.yShape, newText);
                canvas.Children.Add(label);
            }             
        }

        public Label Label
        {
            get { return label; }
        }

        public UIEntity()
        {
            StrokeThickness = 1;
            Stroke = Brushes.Black;
            Fill = Brushes.LemonChiffon;
            BitmapEffect = defaultBitmapEffect(this);
        }

        public List<Port> getPorts()
        {
            return ports;
        }

        public void removeAll()
        {
            foreach (Port p in ports)
            {
                if (canvas.Children.Contains(p))
                    canvas.Children.Remove(p);
            }
            ports.Clear();

            canvas.Children.Remove(label);
        }

        public abstract void createAndDraw(double x, double y);

        public virtual List<UIParam> getParams()
        {
            List<UIParam> param = new List<UIParam>();
            param.Add(textParam);
            return param;
        }

        protected UIEntity(Canvas canvas) : this()
        {
            this.canvas = canvas;
        }

        public struct CoordinatesHandler
        {
            public double xShape, yShape, xCanvas, yCanvas;

            public CoordinatesHandler(double xShape, double yShape, double xCanvas, double yCanvas)
            {
                this.xShape = xShape;
                this.yShape = yShape;
                this.xCanvas = xCanvas;
                this.yCanvas = yCanvas;
            }
        }

    }
}
