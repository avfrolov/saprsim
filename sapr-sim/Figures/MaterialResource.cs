using sapr_sim.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace sapr_sim.Figures
{
    [Serializable]
    public class MaterialResource : Resource, ISerializable
    {

        public MaterialResource(Canvas canvas) : base(canvas)
        {
            init();
            textParam.Value = ResourceType.MATERIAL.Name;
            type.Value = ResourceType.MATERIAL;
        }

        public MaterialResource(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            init();
        }

        public override void createAndDraw(double x, double y)
        {
            base.createAndDraw(x, y);
            label = new Label(this, canvas, x + 20, y + 22, textParam.Value);
            canvas.Children.Add(label);
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

        protected override void init()
        {
            base.init();
            Fill = Brushes.LightSeaGreen;
        }
    }
}
