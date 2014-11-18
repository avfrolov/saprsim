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
        
        public override void execute() {
            Timer timer = Timer.Instance;

            double overallEfficiency = 0.0000001; //  some inaccuracy 

            needTime = getNeedTime(overallEfficiency);

            operationTime += timer.getStep(); 

            if (operationTime >= needTime)
            {
                operationTime = 0;
                Project prj = getProjectFromReadyQueue();
                Entity outputEntity = getOutputs()[0];
                outputEntity.addProjectToQueue(prj);

                getReadyProjectQueue().Remove(prj);
            }
        }

        private double getNeedTime(double overallEfficiency)
        {
            foreach (Resource res in resources)
            {
                overallEfficiency += res.efficiency;
            }

            return needTime = (1 / overallEfficiency) * manHour;
        }

        public void addResource(Resource res)
        {
            resources.Add(res);
        }
    }
}
