using Entities.impl;
using EntityTransformator;
using sapr_sim.Figures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace EntityTransformator.Transformers
{
    public class SubdiagramTransformer : Transformer
    {

        public Entities.Entity transform(UIEntity entity)
        {
            TransformerService ts = new TransformerService();
            SubDiagram sd = entity as SubDiagram;
            Submodel submodel = new Submodel() { name = entity.EntityName };
            submodel.setEntites(ts.transform(sd.ProjectItem.Canvas.Children));
            return submodel;
        }
    }
}
