using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityValidator.rules;

namespace EntityValidator.validator
{
    public class SystemValidator : IValidator
    {
        List<IRule> rules = new List<IRule>();

        public SystemValidator() //add here new rules for include them into validation
        {
            rules.Add(new OneStartRule());
            rules.Add(new OneDestinationRule());
            rules.Add(new InputAndOutputNotEmptyRule());
            rules.Add(new EntityHasCorrectInputsAndOutputsRule());
            rules.Add(new ResourceConnectedToProcedureRule());
        }      

        public ValidationResult startValidation()
        {
            ValidationResult result = new ValidationResult();

            foreach (IRule rule in rules)
            {
                if (!rule.validate())
                {
                    result.addErrors(rule.explain());
                }
            }

            return result;
        }

    }
}
