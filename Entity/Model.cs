using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public sealed class Model
    {
        private static Model instance = new Model();

        static Model() { }
        private Model() { }

        public static Model Instance
        {
            get
            {
                return instance;
            }
        }


        private List<Entity> entities = new List<Entity>();
        private List<Project> projects = new List<Project>();

        public void addEntity(Entity entity)
        {
            entities.Add(entity);
        }

        public List<Entity> getEntities()
        {
            return entities;
        }

        public void addProject(Project project)
        {
            projects.Add(project);
        }

        public List<Project> getProject()
        {
            return projects;
        }
    }
}
