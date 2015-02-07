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
    class NullOutputsForEntityDestRule : IRule
    {
        List<Entity> allEntities = Model.Instance.getEntities();

        public Boolean validate()
        {
            if (allEntities == null || allEntities.Count() == 0)
                return false;

            foreach (Entity entity in allEntities)
            {
                if (entity is EntityDestination)
                    return (entity.getOutputs() == null
                        || entity.getOutputs().Count() == 0);
            }
            return false;
        }

        public void throwException()
        {
            throw new NotNullEntityDestOutputsException();
        }
    }
}
