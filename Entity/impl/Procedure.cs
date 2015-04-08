using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.impl
{
    public class Procedure : Entity
    {
        public double manHour { get; set; }

        private double needTime;
        private double operationTime;
        List<Resource> resources = new List<Resource>();

        public override void execute()
        {
            Timer timer = Timer.Instance;

            double overallEfficiency = 0.0000001; //  some inaccuracy 

            Project prj = getProjectFromReadyQueue();

            if (prj != null)
            {
                needTime = getNeedTime(overallEfficiency, prj.complexity);

                operationTime += timer.getStep();

                if (operationTime >= needTime)
                {
                    operationTime = 0;
                    Entity outputEntity = getOutputs()[0];
                    outputEntity.addProjectToQueue(prj);
                    getReadyProjectQueue().Remove(prj);

                    foreach (Resource res in getResources())
                    {
                        res.users.Remove(this.id);
                        res.isBusy = false;
                    }
                }
            }
        }

        public override bool canUseAsInput(Entity entity)
        {
            return entity is Procedure || entity is Synchronization || entity is Parallel || entity is EntityStart || entity is Submodel;
        }

        public override bool canUseAsOutput(Entity entity)
        {
            return entity is Procedure || entity is Synchronization || entity is Parallel || entity is EntityDestination || entity is Submodel;
        }

        public void addResource(Resource res)
        {
            resources.Add(res);
        }

        public List<Resource> getResources()
        {
            return resources;
        }

        public override bool correctInputCount()
        {
            return input.Count == 1;
        }

        public override bool correctOutputCount()
        {
            return output.Count == 1;
        }

        private double getNeedTime(double overallEfficiency, int complexity)
        {
            foreach (Resource res in resources)
            {
                bool isUsedByThis = res.users.Contains(this.id);

                if (res.users.Count == 0)
                    res.isBusy = false;

                if (res.isBusy && !res.isShared && !isUsedByThis)
                    return Double.MaxValue;

                if (res.isBusy && res.isShared || res.isBusy && isUsedByThis)
                {
                    if (!res.users.Contains(this.id))
                    {
                        res.users.Add(this.id);
                    }

                    overallEfficiency += res.efficiency * res.count / (res.users.Count > 0 ? res.users.Count : 1);
                }

                if (!res.isBusy)
                {
                    overallEfficiency += res.efficiency * res.count;
                    if (!res.users.Contains(this.id))
                    {
                        res.users.Add(this.id);
                        res.isBusy = true;
                    }
                }

            }

            return needTime = (1 / overallEfficiency) * manHour * complexity;
        }

        private void calculatePerformanse(Project project, double needTime)
        {
            double sumPrice = 0.0;
            foreach (Resource res in resources)
            {
                sumPrice += res.price;
            }

            project.performance += needTime / sumPrice;
        }

    }
}
