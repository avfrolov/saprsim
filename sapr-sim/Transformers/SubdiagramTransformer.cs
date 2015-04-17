using Entities.impl;
using EntityTransformator;
using sapr_sim.Figures;
using sapr_sim.Utils;
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
            Submodel submodel = new Submodel() { name = entity.EntityName, id = entity.Id };
            submodel.setEntites(ts.transform(new FileService().open(sd.ProjectItem.FullPath).Children));
            submodel.setResources(ts.getResources());
            return submodel;
        }
    }
}
