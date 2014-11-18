using Entities;
using Entities.impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulation
{
    public class Simulation
    {
        public static void simulate()
        {
            Model model = Model.Instance;
            Timer timer = Timer.Instance;

            List<Entity> entities = model.getEntities();
            List<Project> projects = model.getProject();


            Entity start = getSchemaStart(entities);
            start.setReadyProjectQueue(projects);

            while (projects.Count != 0 && checkForNotReadyProjects(projects))
            {
                foreach (Entity entity in entities)
                {
                    if ((entity.getReadyProjectQueue() != null && entity.getReadyProjectQueue().Count != 0) ||
                        (entity.getNotReadyProjectQueue() != null && entity.getNotReadyProjectQueue().Count != 0))
                    {
                        entity.proceed();
                    }
                }
                timer.increment();
            }
        }

        private static bool checkForNotReadyProjects(List<Project> projects)
        {
            foreach (Project prj in projects)
            {
                if (prj.state.Equals(Entities.State.IN_PROGRESS))
                    return true;
            }

            return false;
        }

        private static Entity getSchemaStart(List<Entity> entities)
        {
            foreach (Entity ent in entities)
            {
                if (ent is EntityStart)
                {
                    return ent;
                }
            }
            return null;
        }

        private static Entity getSchemaDestenation(List<Entity> entities)
        {
            foreach (Entity ent in entities)
            {
                if (ent is EntityDestenation)
                {
                    return ent;
                }
            }
            return null;
        }
    }
}
