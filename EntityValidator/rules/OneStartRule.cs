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
    public class OneStartRule : IRule
    {
        int startCount = 0;
        List<Entity> allEntities = Model.Instance.getEntities();

        public Boolean validate()
        {
            if (allEntities == null || allEntities.Count == 0)
                return false;

            foreach (Entity entity in allEntities)
            {
                if (entity is EntityStart)
                    startCount++;
            }

            if (startCount == 1)
                return true;
            else return false;
        }

        public void throwException()
        {
            throw new NotOneStartException();
        }
    }
}
