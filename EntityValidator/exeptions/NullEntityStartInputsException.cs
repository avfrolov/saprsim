using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityValidator.exeptions
{
    public class NullEntityStartInputsException : Exception
    {
        public NullEntityStartInputsException() : base("Выходной порт сущности 'Старт' не задан")
        {            
        }
    }
}
