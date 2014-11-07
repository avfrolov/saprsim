using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public abstract class Entity
    {
        String name {get; set;}
        long id {get; set;}

        private List<Entity> input = new List<Entity>();
        private List<Entity> output = new List<Entity>();

        private List<Project> inputQueue = new List<Project>();

        public List<Entity> getInputs()
        {
            return input;
        }

        public void setInputs(List<Entity> inputs)
        {
            input = inputs;
        }

        public List<Entity> getOutputs()
        {
            return output;
        }

        public void setOutputs(List<Entity> output)
        {
            this.output = output;
        }

        public List<Project> getQueue()
        {
            return inputQueue;
        }

        public void setQueue(List<Project> inputQueue)
        {
            this.inputQueue = inputQueue;
        }

        public abstract void execute();
    }
}
