using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityValidator.exeptions
{
    public class ValidationError
    {

        private string message;
        private Entities.Entity failedEntity;

        public ValidationError(string message) 
        {
            this.message = message;
        }

        public ValidationError(string message, Entities.Entity failedEntity) : this(message)
        {
            this.failedEntity = failedEntity;
        }

        public string Message
        {
            get { return message; }
        }

        public Entities.Entity FailedEntity
        { 
            get { return failedEntity; } 
        }

    }
}
