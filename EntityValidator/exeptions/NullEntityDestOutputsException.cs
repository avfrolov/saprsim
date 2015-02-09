using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityValidator.exeptions
{
    public class NullEntityDestOutputsException : Exception
    {
        public NullEntityDestOutputsException() : base("Входной порт сущности 'Конец' не задан")
        {            
        }
    }
}
