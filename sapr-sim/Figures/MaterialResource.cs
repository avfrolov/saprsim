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
