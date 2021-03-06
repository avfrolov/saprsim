﻿using Entities;
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
        private Model model = Model.Instance;

        public Controller(List<Entity> entities, List<Resource> resources)
        {
            // clear everything
            model.getEntities().Clear();
            model.getProjects().Clear();
            model.getResources().Clear();
            Timer.Instance.resetTime();

            this.entities = entities;
            model.setResources(resources);
        }

        public void simulate()
        {
            foreach (Entity ent in entities)
                model.addEntity(ent);

            SystemValidator validator = new SystemValidator(model.getEntities(), model.getResources());
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
