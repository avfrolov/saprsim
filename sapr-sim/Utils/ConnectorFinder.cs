using sapr_sim.Figures.New;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace sapr_sim.Utils
{
    public class ConnectorFinder
    {

        public static List<Connector> find(UIElementCollection entities, UIEntity entity)
        {
            List<Connector> result = new List<Connector>();
            foreach (UIEntity e in entities)
            {
                if (e is Connector)
                {
                    Connector conenctor = e as Connector;
                    BindingExpression srcExp = conenctor.GetBindingExpression(Connector.SourceProperty);
                    BindingExpression dstExp = conenctor.GetBindingExpression(Connector.DestinationProperty);

                    if (srcExp != null && dstExp != null)
                    {
                        Port src = srcExp.DataItem as Port;
                        Port dst = dstExp.DataItem as Port;

                        if (entity.Equals(src.Owner) || entity.Equals(dst.Owner))
                            result.Add(conenctor);
                    }
                }
            }

            return result;
        }

    }
}
