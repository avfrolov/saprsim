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

        public static Connector find(UIElementCollection entities, UIEntity entity)
        {
            foreach (UIEntity e in entities)
            {
                if (e is Connector)
                {
                    Connector conenctor = e as Connector;
                    BindingExpression srcExp = conenctor.GetBindingExpression(Connector.SourceProperty);
                    BindingExpression dstExp = conenctor.GetBindingExpression(Connector.DestinationProperty);

                    if (srcExp != null && dstExp != null)
                    {
                        UIEntity src = srcExp.DataItem as UIEntity;
                        UIEntity dst = dstExp.DataItem as UIEntity;

                        if (entity.Equals(src) || entity.Equals(dst)) 
                            return conenctor;
                    }
                }
            }

            return null;
        }

    }
}
