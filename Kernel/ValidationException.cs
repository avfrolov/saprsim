using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kernel
{
    public class ValidationException : Exception
    {
        private Dictionary<string, List<Entity>> errors = new Dictionary<string, List<Entity>>();

        public void addError(string message, List<Entity> entities)
        {
            if (errors.ContainsKey(message))
            {
                List<Entity> failed = errors[message];
                failed.AddRange(entities);
                errors.Remove(message);
                errors.Add(message, failed);
            }
            else
            {
                errors.Add(message, entities);
            }
        }

        public void addError(string message, Entity entity)
        {
            List<Entity> entities = new List<Entity>();
            entities.Add(entity);
            addError(message, entities);
        }

        public Dictionary<string, List<Entity>> Errors
        {
            get { return errors; }
        }

    }
}
