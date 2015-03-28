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
    public class SubmodelRule : IRule
    {

        private List<Entity> allEntities = Model.Instance.getEntities();
        private List<Entity> failed = new List<Entity>();

        public bool validate()
        {            
            foreach (Entity e in allEntities)
            {
                if (e is Submodel)
                {
                    Submodel sm = e as Submodel;
                    bool containsStart = false, containsDestination = false;
                    foreach (Entity se in sm.Entities)
                    {
                        if (se is EntityStart)
                            containsStart = true;
                        if (se is EntityDestination)
                            containsDestination = true;
                    }
                    if (!containsStart || !containsDestination)
                        failed.Add(sm);
                }
            }
            return failed.Count == 0;
        }

        public List<ValidationError> explain()
        {
            List<ValidationError> errors = new List<ValidationError>();

            foreach (var fail in failed)
            {
                errors.Add(new ValidationError("Подключаемый подпроцесс '" + fail.ToString() + "' имеет неправильную структуру", fail));
            }

            return errors;
        }
    }
}
