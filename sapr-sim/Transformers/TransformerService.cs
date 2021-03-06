﻿using Entities;
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
                    processConnectionLine(e as ConnectionLine, true);
                else if (e is sapr_sim.Figures.Resource)
                    processResource(elements, e as sapr_sim.Figures.Resource);
            }

            processSubprocess(elements);

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

        private void processConnectionLine(ConnectionLine c, bool skipSubprocess)
        {
            UIEntity src = c.SourcePort.Owner;
            UIEntity dest = c.DestinationPort.Owner;

            if (skipSubprocess && (src is SubDiagram || dest is SubDiagram)) return;

            if (map.ContainsKey(src) && map.ContainsKey(dest))
                addLinkForRealEntities(c, map[src], map[dest]);
        }

        private void processResource(UIElementCollection elements, sapr_sim.Figures.Resource resource)
        {
            List<ConnectionLine> connectors = ConnectorFinder.find(elements, resource);

            Entities.Resource res = new Entities.Resource() { efficiency = resource.Efficiency, price = resource.Price, count = resource.Count, isShared = resource.IsShared };
            resources.Add(res);

            foreach(ConnectionLine con in connectors)
            {
                UIEntity procedure = null;
                if (con.SourcePort != null && con.SourcePort.PortType == PortType.RESOURCE)
                {
                    UIEntity src = con.SourcePort.Owner;
                    UIEntity dst = con.DestinationPort.Owner;
                    procedure = src is Procedure ? src : dst is Procedure ? dst : null;

                    if (procedure != null)
                    {
                        Entities.impl.Procedure realProcedure = map[procedure] as Entities.impl.Procedure;
                        if (!realProcedure.getResources().Contains(res))
                            addAdditionalRelations(realProcedure, res);
                    }
                }
            }
        }

        private void processSubprocess(UIElementCollection elements)
        {
            foreach (UIElement e in elements)
            {
                if (e is SubDiagram)
                {
                    SubDiagram sd = e as SubDiagram;
                    List<ConnectionLine> connectors = ConnectorFinder.find(elements, sd);
                    foreach (ConnectionLine cl in connectors)
                    {
                        processConnectionLine(cl, false);
                    }
                }
            }
        }

        private void addLinkForRealEntities(ConnectionLine c, Entity realSrc, Entity realDest)
        {
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
        }

        private void addAdditionalRelations(Entity src, Entities.Resource dst)
        {
            if (src is Entities.impl.Procedure)
            {
                (src as Entities.impl.Procedure).addResource(dst);
            }
        }

    }
}
