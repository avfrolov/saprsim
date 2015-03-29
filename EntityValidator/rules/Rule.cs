using Entities;
using EntityValidator.exeptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityValidator.rules
{
    public abstract class Rule
    {

        protected List<Entity> entities;

        public Rule(List<Entity> entities)
        {
            this.entities = entities;
        }

        public abstract bool validate();
        public abstract List<ValidationError> explain();
    }
}
