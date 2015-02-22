using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using Entities.impl;
using EntityValidator.exeptions;

namespace EntityValidator.rules
{
    class OneDestinationRule : IRule
    {
        private List<Entity> destinations = new List<Entity>();
        private List<Entity> allEntities = Model.Instance.getEntities();

        public Boolean validate()
        {
            if (allEntities == null || allEntities.Count == 0)
                return false;

            foreach (Entity entity in allEntities)
            {
                if (entity is EntityDestination)
                    destinations.Add(entity);
            }

            return destinations.Count == 1;                
        }

        public List<ValidationError> explain()
        {
            List<ValidationError> errors = new List<ValidationError>();
            if (destinations.Count == 0)
            {
                errors.Add(new ValidationError("Сущность 'Конец' не найдена"));
            }
            else if (destinations.Count > 1)
            {
                foreach(Entity dest in destinations)
                {
                    errors.Add(new ValidationError("Лишняя сущность 'Конец'", dest));
                }
            }
            return errors;
        }
    }
}
