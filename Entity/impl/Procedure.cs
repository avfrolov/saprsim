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

            Project prj = getProjectFromReadyQueue();

            needTime = getNeedTime(overallEfficiency, prj.complexity);

            operationTime += timer.getStep(); 

            if (operationTime >= needTime)
            {
                operationTime = 0;
                Entity outputEntity = getOutputs()[0];
                outputEntity.addProjectToQueue(prj);
                getReadyProjectQueue().Remove(prj);
            }
        }

        private double getNeedTime(double overallEfficiency, int complexity)
        {
            foreach (Resource res in resources)
            {
                overallEfficiency += res.efficiency;
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

        public void addResource(Resource res)
        {
            resources.Add(res);
        }
    }
}
