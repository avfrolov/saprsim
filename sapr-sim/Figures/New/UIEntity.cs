using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace sapr_sim.Figures.New
{
    public class UIEntity : Shape
    {

        // We must guarantee that creation of Shape instances are impossible
        protected UIEntity() { }

        protected override System.Windows.Media.Geometry DefiningGeometry
        {
            get { throw new NotImplementedException(); }
        }
    }
}
