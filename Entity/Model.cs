using Entities.impl;
using Statistics.Beans;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Model
    {
        private static Model instance = new Model();

        private Model(){ }

        public static Model Instance
        {
            get
            {
                return instance;
            }
        }

        public long id { get; set; }

        public double simulatePeriod { get; set; }
        public double projectAppearenceProbability { get; set; }

        public double timeRestriction { get; set; }
        public ProcessingState state { get; set; }

        private ICollection<Entity> _entities;

        public virtual ICollection<Entity> entities
        {
            get { return _entities ?? (_entities = new Collection<Entity>()); }
            protected set { _entities = value; }
        }


        private ICollection<Project> _projects;

        public virtual ICollection<Project> projects
        {
            get { return _projects ?? (_projects = new Collection<Project>()); }
            protected set { _projects = value; }
        }


        private ICollection<Resource> _resources;

        public virtual ICollection<Resource> resources
        {
            get { return _resources ?? (_resources = new Collection<Resource>()); }
            protected set { _resources = value; }
        }

        public void addEntity(Entity entity)
        {
            entities.Add(entity);
        }

        public ICollection<Entity> getEntities()
        {
            return entities;
        }

        public void addProject(Project project)
        {
            projects.Add(project);
        }

        public ICollection<Project> getProjects()
        {
            return projects;
        }

        public void addResource(Resource resource)
        {
            resources.Add(resource);
        }

        public ICollection<Resource> getResources()
        {
            return resources;
        }

        public void setResources(ICollection<Resource> resources)
        {
            this.resources = resources;
        }
    }
}
