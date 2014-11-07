using Entities;
using Entities.impl;
using EntityValidator.exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kernel
{
    class Performer
    {

        public static void simulate()
        {
            Model model = Model.Instance;

            List<Entity> entities =  model.getEntities();
            List<Project> projects = model.getProject();


            Entity start =  getSchemaStart(entities);
            start.setQueue(projects);

            while (checkForNotReadyProjects(projects))
            {


                



                model.modelTime++;   
            }
            

        }

        private static bool checkForNotReadyProjects(List<Project> projects)
        {
            foreach (Project prj in projects){
                if (prj.state.Equals(Entities.State.IN_PROGRESS))
                    return false;
            }

            return true;
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

            throw new NoStartEntityException();
        }
        
    }
}
