using Entities;
using EntityValidator;
using EntityValidator.exeptions;
using EntityValidator.validator;
using Simulation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kernel
{
    public class Controller
    {

        private List<Entity> entities;

        public Controller(List<Entity> entities)
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
            ValidationResult result = validator.startValidation();

            if (result.Success)
            {
                Simulation.Simulation.simulate();
            } 
            else
            {
                ValidationException ex = new ValidationException();
                foreach(ValidationError err in result.Errors)
                {
                    ex.addError(err.Message, err.FailedEntity);
                }
                throw ex;
            }
        }

        public double SimulationTime
        {
            get { return Timer.Instance.getTime(); }
        }
    }
}
