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
        private List<Entities.Resource> resources = new List<Entities.Resource>();
        
        public List<Entity> transform(UIElementCollection elements)
        {
            List<Entity> realEntities = new List<Entity>();
            
            foreach(UIElement e in elements)
            {
                // skip no logic ui entities
                if (e is sapr_sim.Figures.Label || e is Port || e is ConnectionLine || e is sapr_sim.Figures.Resource) continue;
                
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
                if (e is ConnectionLine)
                {
                    ConnectionLine c = e as ConnectionLine;
                    UIEntity src = c.SourcePort.Owner;
                    UIEntity dest = c.DestinationPort.Owner;

                    if (map.ContainsKey(src) && map.ContainsKey(dest))
                    {
                        Entity realSrc = map[src];
                        Entity realDest = map[dest];
                        
                        if (c.SourcePort.PortType == PortType.OUTPUT && c.DestinationPort.PortType == PortType.INPUT)
                        {
                            realSrc.addOutput(realDest);
                            realDest.addInput(realSrc);
                        }
                        else if (c.SourcePort.PortType == PortType.INPUT && c.DestinationPort.PortType == PortType.OUTPUT)
                        {
                            realSrc.addInput(realDest);
                            realDest.addOutput(realSrc);
                        }
                        // protection for random line connection
                        //if (realSrc.canUseAsOutput(realDest) && realDest.canUseAsInput(realSrc))
                        //{
                        //    realSrc.addOutput(realDest);
                        //    realDest.addInput(realSrc);
                        //} 
                        //else if (realSrc.canUseAsInput(realDest) && realDest.canUseAsOutput(realSrc))
                        //{
                        //    realSrc.addInput(realDest);
                        //    realDest.addOutput(realSrc);
                        //}
                    }
                }
                else if (e is sapr_sim.Figures.Resource)
                {
                    sapr_sim.Figures.Resource resource = e as sapr_sim.Figures.Resource;
                    List<ConnectionLine> connectors = ConnectorFinder.find(elements, resource);

                    Entities.Resource res = new Entities.Resource() { efficiency = resource.Efficiency };
                    resources.Add(res);

                    if (connectors.Count > 0)
                    {
                        UIEntity procedure = null;
                        if (connectors[0].SourcePort != null && connectors[0].SourcePort.PortType == PortType.RESOURCE)
                        {
                            UIEntity src = connectors[0].SourcePort.Owner;
                            UIEntity dst = connectors[0].DestinationPort.Owner;
                            procedure = src is Procedure ? src : dst is Procedure ? dst : null;
                            if (procedure != null)
                            {
                                addAdditionalRelations(map[procedure], res);
                            }                                
                        }                         
                    }
                }
            }

            return realEntities;
        }

        public List<UIEntity> transform(List<Entity> elements)
        {
            List<UIEntity> uiEntities = new List<UIEntity>(elements.Count);

            foreach(Entity e in elements)
            { 
                if (map.ContainsValue(e))
                {
                    UIEntity uie = map.FirstOrDefault(x => x.Value.Equals(e)).Key;
                    if (uie != null)
                        uiEntities.Add(uie);
                }
            }
            return uiEntities;
        }

        public List<Entities.Resource> getResources()
        {
            return resources;
        }

        // TODO create additional map service
        private void addAdditionalRelations(Entity src, Entities.Resource dst)
        {
            if (src is Entities.impl.Procedure)
            {
                (src as Entities.impl.Procedure).addResource(dst);
            }
        }

    }
}
