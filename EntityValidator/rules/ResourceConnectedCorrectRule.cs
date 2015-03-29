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

        public ResourceConnectedCorrectRule(List<Entity> entities) : base(entities)
        {
        }

        public override bool validate()
        {
            int procedureResources = 0;
            foreach (Entity e in entities)
            {
                if (e is Procedure)
                    procedureResources += (e as Procedure).getResources().Count;
            }

            int totalResources = 0;
            foreach (Resource res in resources)
                totalResources += res.count;

            return procedureResources == totalResources;
        }

        public override List<ValidationError> explain()
        {
            List<ValidationError> errors = new List<ValidationError>();
            errors.Add(new ValidationError("Ресурсы не корректно подключены"));            
            return errors;
        }
    }
}
