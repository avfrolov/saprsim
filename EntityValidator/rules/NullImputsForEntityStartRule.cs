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
    class NullInputsForEntityStartRule : IRule
    {
        List<Entity> allEntities = Model.Instance.getEntities();

        public Boolean validate()
        {
            if (allEntities == null || allEntities.Count() == 0)
                return false;

            foreach (Entity entity in allEntities)
            {
                if (entity is EntityStart)
                    return (entity.getInputs() == null
                        || entity.getInputs().Count() == 0);
            }
            return false;
        }

        public ValidationError explain()
        {
            return new ValidationError("Выходной порт сущности 'Старт' не задан");
        }
    }
}
