using Entities;
using EntityValidator.exeptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityValidator.rules
{
    public class ResourceConnectedToProcedureRule : IRule
    {

        private List<Entity> allEntities = Model.Instance.getEntities();

        public bool validate()
        {
            foreach (Entity e in allEntities)
            {
                
            }

            return true;
        }

        public List<ValidationError> explain()
        {
            return new List<ValidationError>();
            //return new ValidationError("Сущность 'Ресурс' может быть подключена только к сущности 'Процедура'");
        }
    }
}
