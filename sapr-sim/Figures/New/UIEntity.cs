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

namespace sapr_sim.Figures.New
{
    public class UIEntity : Shape
    {

        public static readonly DependencyProperty AnchorPointProperty =
                DependencyProperty.Register(
                    "AnchorPoint", typeof(Point), typeof(UIEntity),
                        new FrameworkPropertyMetadata(new Point(100, 100),
                        FrameworkPropertyMetadataOptions.AffectsMeasure));

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

        public Point AnchorPoint
        {
            get { return (Point)GetValue(AnchorPointProperty); }
            set { SetValue(AnchorPointProperty, value); }
        }

        protected Canvas canvas;

        public UIEntity()
        {
            StrokeThickness = 1;
            Stroke = Brushes.Black;
            Fill = Brushes.LemonChiffon;
            BitmapEffect = defaultBitmapEffect(this);
        }

        protected UIEntity(Canvas canvas) : this()
        {
            this.canvas = canvas;
            this.LayoutUpdated += UIEntity_LayoutUpdated;
        }

        protected void UIEntity_LayoutUpdated(object sender, EventArgs e)
        {
            if (canvas != null)
            {
                Size size = RenderSize;
                Point ofs = new Point(size.Width / 2, size.Height / 2);

                // TODO why ofs with X=0.0 & Y=0.0 doesn't work?
                if (ofs.X == 0.0 && ofs.Y == 0.0) return;

                AnchorPoint = TransformToVisual(this.canvas).Transform(ofs);
            }
        }

        protected override System.Windows.Media.Geometry DefiningGeometry
        {
            get { throw new NotImplementedException(); }
        }

    }
}
