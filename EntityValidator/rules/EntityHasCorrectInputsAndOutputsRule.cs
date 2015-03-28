using Entities;
using EntityValidator.exeptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityValidator.rules
{
    public class EntityHasCorrectInputsAndOutputsRule : Rule
    {

        private Dictionary<Entity, Entity> failedInputs = new Dictionary<Entity, Entity>();
        private Dictionary<Entity, Entity> failedOutputs = new Dictionary<Entity, Entity>();

        public EntityHasCorrectInputsAndOutputsRule(List<Entity> entities) : base(entities)
        {
        }

        public override bool validate()
        {
            // doesn't work because random line protection in TransformesService#transform()
            // TODO
            foreach (Entity e in entities)
            {
                foreach (Entity input in e.getInputs())
                {
                    if (!e.canUseAsInput(input))
                        failedInputs.Add(e, input);
                }

                foreach (Entity output in e.getOutputs())
                {
                    if (!e.canUseAsOutput(output))
                        failedOutputs.Add(e, output);
                }
            }

            return failedInputs.Count == 0 && failedOutputs.Count == 0;
        }

        public override List<ValidationError> explain()
        {
            List<ValidationError> errors = new List<ValidationError>();
            
            foreach (var input in failedInputs)
            {
                errors.Add(new ValidationError("Сущность '" + input.Key.ToString() + "' не может иметь на входе сущность '" + input.Value.ToString() + "'"));
            }

            foreach (var output in failedOutputs)
            {
                errors.Add(new ValidationError("Сущность '" + output.Key.ToString() + "' не может иметь на выходе сущность '" + output.Value.ToString() + "'"));
            }
            
            return errors;
        }
    }
}
