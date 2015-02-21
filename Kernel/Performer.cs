using Entities;
using EntityValidator.validator;
using Simulation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kernel
{
    public class Performer
    {

        private List<Entity> entities;

        public Performer(List<Entity> entities)
        {
            this.entities = entities;
        }

        public void simulate()
        {
            
            Model model = Model.Instance;

            // clear everything
            model.getEntities().Clear();
            model.getProject().Clear();
            Timer.Instance.resetTime();

            foreach (Entity ent in entities)
                model.addEntity(ent);

            SystemValidator validator = new SystemValidator();
            validator.startValidation();
           
            Simulation.Simulation.simulate();
        }

        public double SimulationTime
        {
            get { return Timer.Instance.getTime(); }
        }
    }
}
