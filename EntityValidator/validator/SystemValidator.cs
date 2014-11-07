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
        public SystemValidator()
        {
            rules.Add(new OneStartRule());
            rules.Add(new OneDestRule());
        }      

        public Boolean startValidation()
        {
            foreach (IRule rule in rules)
            {
                if (!rule.validate())
                {
                    rule.throwException();
                    return false;
                }
            }

            return true;
        }
    }
}
