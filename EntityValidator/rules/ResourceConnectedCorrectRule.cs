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
    public class ResourceConnectedCorrectRule : IRule
    {

        private List<Entity> allEntities = Model.Instance.getEntities();
        private List<Resource> resources = Model.Instance.getResources();

        public bool validate()
        {
            int countResources = 0;
            foreach (Entity e in allEntities)
            {
                if (e is Procedure)
                    countResources += (e as Procedure).getResources().Count;
            }

            return countResources == resources.Count;
        }

        public List<ValidationError> explain()
        {
            List<ValidationError> errors = new List<ValidationError>();
            errors.Add(new ValidationError("Ресурсы не корректно подключены"));            
            return errors;
        }
    }
}
