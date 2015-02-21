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
    public class InputAndOutputNotNullRule : IRule
    {
        List<Entity> allEntities = Model.Instance.getEntities();

        public Boolean validate()
        {
            if (allEntities == null || allEntities.Count() == 0)
                return false;

            foreach (Entity entity in allEntities)
            {
                if (!(entity is EntityStart || entity is EntityDestination))
                {
                    if (isInputsNull(entity) || isOutputsNull(entity))
                        return false;
                }                    
            }
            return true;
        }

        private Boolean isInputsNull(Entity entity)
        {
            return (entity.getInputs() == null || entity.getInputs().Count() == 0);
        }

        private Boolean isOutputsNull(Entity entity)
        {
            return (entity.getOutputs() == null || entity.getOutputs().Count() == 0);
        }

        public ValidationError explain()
        {
            return new ValidationError("что-то не так(");
        }
    }
}
