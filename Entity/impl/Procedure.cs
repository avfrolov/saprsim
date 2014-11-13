using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.impl
{
    public class Procedure : Entity
    {
        double manHour { get; set; }
        double needTime { get; set; }

        private double operationTime;
        List<Resource> resources = new List<Resource>();
        
        public override void execute() {
            Timer timer = Timer.Instance;

            double time = timer.getTime();
            double overallEfficiency = double.MaxValue;

            foreach (Resource res in resources){
                overallEfficiency += res.efficiency;
            }

            needTime = (1/overallEfficiency) * manHour;

            if (needTime < timer.getStep())
            {
                operationTime = 0;
                Project prj = getProjectFromQueue();
                Entity outputEntity = getOutputs()[0];
                outputEntity.addProjectToQueue(prj);
            }
            else
            {
                if (operationTime >= needTime)
                {
                    operationTime = 0;
                    Project prj = getProjectFromQueue();
                    Entity outputEntity = getOutputs()[0];
                    outputEntity.addProjectToQueue(prj);
                }
                else
                {
                    operationTime += timer.getStep(); 
                }
            }
        }

        public void addResource(Resource res)
        {
            resources.Add(res);
        }
    }
}
