using Entities;
using sapr_sim.Figures;
using sapr_sim.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace EntityTransformator
{
    public class TransformerService
    {

        private Dictionary<UIEntity, Entity> map = new Dictionary<UIEntity, Entity>();
        
        public List<Entity> transform(UIElementCollection elements)
        {
            List<Entity> realEntities = new List<Entity>();
            
            foreach(UIElement e in elements)
            {
                // skip no logic ui entities
                if (e is sapr_sim.Figures.Label || e is Port || e is Connector || e is Resource) continue;
                
                Transformer transformer = TransformerFactory.getTransformer(e.GetType());
                Entity re = transformer.transform(e as UIEntity);
                if (re != null)
                {
                    realEntities.Add(re);
                    map.Add(e as UIEntity, re);
                }
            }

            foreach (UIElement e in elements)
            {
                if (e is Connector)
                {
                    Connector c = e as Connector;
                    UIEntity src = c.SourcePort.Owner;
                    UIEntity dest = c.DestinationPort.Owner;

                    if (map.ContainsKey(src) && map.ContainsKey(dest))
                    {
                        Entity realSrc = map[src];
                        Entity realDest = map[dest];
                        
                        // protection for random line connection
                        if (realSrc.canUseAsOutput(realDest) && realDest.canUseAsInput(realSrc))
                        {
                            realSrc.addOutput(realDest);
                            realDest.addInput(realSrc);
                        } 
                        else
                        {
                            realSrc.addInput(realDest);
                            realDest.addOutput(realSrc);
                        }
                    }
                }
                else if (e is Resource)
                {
                    Resource resource = e as Resource;
                    List<Connector> connectors = ConnectorFinder.find(elements, resource);

                    if (connectors.Count > 0)
                    {
                        UIEntity procedure = null;
                        if (connectors[0].SourcePort != null)
                        {
                            if (connectors[0].SourcePort.Owner is Procedure)
                                procedure = connectors[0].SourcePort.Owner;
                            else if (connectors[0].DestinationPort.Owner is Procedure)
                                procedure = connectors[0].DestinationPort.Owner;
                        }
                        
                        if (procedure != null)
                            addAdditionalRelations(map[procedure], new Entities.impl.Resource() { efficiency = resource.Efficiency });
                    }
                }
            }

            return realEntities;
        }

        // TODO create additional map service
        private void addAdditionalRelations(Entity src, Entities.impl.Resource dst)
        {
            if (src is Entities.impl.Procedure)
            {
                (src as Entities.impl.Procedure).addResource(dst);
            }
        }

    }
}
