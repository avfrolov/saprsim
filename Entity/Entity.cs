using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public abstract class Entity
    {
        public String name {get; set;}
        public long id {get; set;}

        protected List<Entity> input = new List<Entity>();
        protected List<Entity> output = new List<Entity>();

        private List<Project> notReadyProjectQueue = new List<Project>();
        private List<Project> readyProjectQueue = new List<Project>();

        public List<Entity> getInputs()
        {
            return input;
        }

        public void setInputs(List<Entity> inputs)
        {
            input = inputs;
        }

        public void addInput(Entity input)
        {
            this.input.Add(input);
        }

        public bool hasInputs()
        {
            return input.Count > 0;
        }

        public List<Entity> getOutputs()
        {
            return output;
        }

        public void setOutputs(List<Entity> output)
        {
            this.output = output;
        }

        public void addOutput(Entity output)
        {
            this.output.Add(output);
        }

        public bool hasOutputs()
        {
            return output.Count > 0;
        }

        public List<Project> getReadyProjectQueue()
        {
            return readyProjectQueue;
        }

        public void setReadyProjectQueue(List<Project> inputQueue)
        {
            this.readyProjectQueue = new List<Project>(inputQueue);
        }

        public List<Project> getNotReadyProjectQueue()
        {
            return notReadyProjectQueue;
        }

        public void setNotReadyProjectQueue(List<Project> inputQueue)
        {
            this.notReadyProjectQueue = new List<Project>(inputQueue);
        }

        public Project getProjectFromReadyQueue()
        {
            List<Project> prjQueue = getReadyProjectQueue();
            if (prjQueue != null && prjQueue.Count != 0)
            {
                return prjQueue[0];
            }

            return null;
        }

        public void removeProjectFromReadyQueue()
        {
            List<Project> prjQueue = getReadyProjectQueue();
            if (prjQueue != null && prjQueue.Count != 0)
            {
                prjQueue.RemoveAt(0);
            }
        }

        public void addProjectToQueue(Project project)
        {
            List<Project> prjQueue = getNotReadyProjectQueue();
            prjQueue.Add(project);
            setNotReadyProjectQueue(prjQueue);
        }

        public void proceed()
        {
           execute();
           List<Project> projects = getNotReadyProjectQueue();

           foreach (Project project in projects)
           {
               getReadyProjectQueue().Add(project);
           }
        }

        public abstract void execute();

        public abstract bool canUseAsInput(Entity entity);

        public abstract bool canUseAsOutput(Entity entity);

        public abstract bool correctInputCount();

        public abstract bool correctOutputCount();

        public override string ToString()
        {
            return name;
        }

    }
}
