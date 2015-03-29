using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.impl
{
    public class Submodel : Entity
    {

        private List<Entity> entities = new List<Entity>();
        private List<Resource> resources = new List<Resource>();
        private List<Entity> pseudoInput = new List<Entity>();
        private List<Entity> pseudoOutput = new List<Entity>();
        private bool isFirstCall = true;

        public void setEntites(List<Entity> entities)
        {
            this.entities = entities;
        }

        public void setResources(List<Resource> resources)
        {
            this.resources = resources;
        }

        public List<Resource> getResources()
        {
            return resources;
        }

        public override void execute()
        {
            foreach (Entity entity in entities)
            {
                if (!(entity is EntityStart) && !(entity is EntityDestination))
                {
                    //when we call submodel first time we need to setup projec queue to
                    // first entity in submodel enities chain
                    if (isFirstCall)
                    {
                        entity.setReadyProjectQueue(getReadyProjectQueue());
                        isFirstCall = false;
                    }
                    entity.proceed();
                }
            }

            deleteProjectFromSubmodelReadyQueue();
        }

        private void deleteProjectFromSubmodelReadyQueue()
        {
            List<Project> prjToDelete = new List<Project>();

            foreach (Project prj in getReadyProjectQueue())
            {
                bool needDelete = true;
                foreach (Entity e in entities)
                {
                    if (e.getReadyProjectQueue().Contains(prj)
                             || e.getNotReadyProjectQueue().Contains(prj)) //if project is proceeded or whait to be proceeded in 
                        //any submodel entity - dont remove it                       
                        needDelete = false;
                }
                if (needDelete)
                    prjToDelete.Add(prj);
            }

            foreach (Project prj in prjToDelete)
            {
                getReadyProjectQueue().Remove(prj);
            }
        }

        public void addEntity(Entity entity)
        {
            entities.Add(entity);
        }

        public List<Entity> Entities
        {
            get { return entities; }
        }

        public override bool canUseAsInput(Entity entity)
        {
            return true;
        }

        public override bool canUseAsOutput(Entity entity)
        {
            return true;
        }

        public override bool correctInputCount()
        {
            return pseudoInput.Count == 1;
        }

        public override bool correctOutputCount()
        {
            return pseudoOutput.Count == 1;
        }

        public override void addInput(Entity input)
        {
            foreach (Entity entity in entities)
            {
                if (entity is EntityStart)
                {
                    if (entity.getOutputs().Count > 0)
                    {
                        entity.getOutputs()[0].getInputs().Clear();
                        entity.getOutputs()[0].addInput(input);
                        this.pseudoInput.Add(input);
                        return;
                    }
                }
            }
        }

        public override void setInputs(List<Entity> inputs)
        {
            foreach (Entity entity in entities)
            {
                if (entity is EntityStart)
                {
                    if (entity.getOutputs().Count > 0)
                    {
                        entity.getOutputs()[0].setInputs(inputs);
                        this.pseudoInput.AddRange(inputs);
                        return;
                    }
                }
            }
        }

        public override void addOutput(Entity output)
        {
            foreach (Entity entity in entities)
            {
                if (entity is EntityDestination)
                {
                    if (entity.getInputs().Count > 0)
                    {
                        entity.getInputs()[0].getOutputs().Clear();
                        entity.getInputs()[0].addOutput(output);
                        this.pseudoOutput.Add(output);
                        return;
                    }
                }
            }
        }

        public override void setOutputs(List<Entity> outputs)
        {
            foreach (Entity entity in entities)
            {
                if (entity is EntityDestination)
                {
                    if (entity.getInputs().Count > 0)
                    {
                        entity.getInputs()[0].setOutputs(outputs);
                        this.pseudoOutput.AddRange(outputs);
                        return;
                    }
                }
            }
        }
    }
}
