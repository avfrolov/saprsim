﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using Entities.impl;
using EntityValidator.exeptions;

namespace EntityValidator.rules
{
    class OneDestRule : IRule
    {
        int destCount = 0;
        List<Entity> allEntities = Model.Instance.getEntities();

        public Boolean validate()
        {
            if (allEntities == null || allEntities.Count == 0)
                return false;

            foreach (Entity entity in allEntities)
            {
                if (entity is EntityDestenation)
                    destCount++;
            }

            if (destCount == 1)
                return true;
            else return false;
        }

        public void throwException()
        {
            throw new NotOneDestException();
        }
    }
}