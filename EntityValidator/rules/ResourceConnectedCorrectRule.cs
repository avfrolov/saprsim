using Entities;
using Entities.impl;
using EntityValidator.exeptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityValidator.rules
{
    public class ResourceConnectedCorrectRule : Rule
    {

        private List<Resource> resources = Model.Instance.getResources();

        public ResourceConnectedCorrectRule(List<Entity> entities, List<Resource> resources) : base(entities)
        {
            this.resources = resources;
        }

        public override bool validate()
        {
            int procedureResources = 0;
            foreach (Entity e in entities)
            {
                if (e is Procedure)
                {
                    Procedure p = (e as Procedure);
                    foreach(Resource res in p.getResources())
                        procedureResources += res.totalCount;
                }
            }

            int totalResources = 0;
            foreach (Resource res in resources)
                totalResources += res.count;

            return procedureResources >= totalResources;
        }

        public override List<ValidationError> explain()
        {
            List<ValidationError> errors = new List<ValidationError>();
            errors.Add(new ValidationError("Ресурсы не корректно подключены"));            
            return errors;
        }
    }
}
