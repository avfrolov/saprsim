
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
        public void simulate(List<Entity> entities)
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
    }
}
