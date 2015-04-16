using Entities.impl;
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

        private Model() { }

        public static Model Instance
        {
            get
            {
                return instance;
            }
        }

        public double simulatePeriod { get; set; }
        public double projectAppearenceProbability { get; set; }

        public double timeRestriction { get; set; }
        public ProcessingState state { get; set; }

        private List<Entity> entities = new List<Entity>();
        private List<Project> projects = new List<Project>();
        private List<Resource> resources = new List<Resource>();

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

        public List<Project> getProjects()
        {
            return projects;
        }

        public void addResource(Resource resource)
        {
            resources.Add(resource);
        }

        public List<Resource> getResources()
        {
            return resources;
        }

        public void setResources(List<Resource> resources)
        {
            this.resources = resources;
        }
    }
}
