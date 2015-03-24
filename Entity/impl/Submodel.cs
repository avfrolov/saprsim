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

        public void setEntites(List<Entity> entities)
        {
            this.entities = entities;
        }

        public override void execute() 
        {
            Project prj = getProjectFromReadyQueue();

            foreach (Entity entity in entities)
            {
                if (!(entity is EntityStart) && !(entity is EntityDestination))
                {
                    entity.setReadyProjectQueue(getReadyProjectQueue());
                    entity.proceed();
                }
            }

            getReadyProjectQueue().Remove(prj);
        }

        public void addEntity(Entity entity)
        {
            entities.Add(entity);
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
            return input.Count == 1;
        }

        public override bool correctOutputCount()
        {
            return output.Count == 1;
        }

        public void setInputs(List<Entity> inputs)
        {
            foreach (Entity entity in entities)
            {
                if (entity is EntityStart)
                {
                    entity.getOutputs()[0].setInputs(inputs);
                    return;
                }
            }
        }

        public void setOutputs(List<Entity> outputs)
        {
            foreach (Entity entity in entities)
            {
                if (entity is EntityDestination)
                {
                    entity.getInputs()[0].setOutputs(outputs);
                    return;
                }
            }
        }
    }
}
