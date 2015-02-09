using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityValidator.exeptions
{
    public class NotOneStartException : Exception
    {

        public NotOneStartException() : base("Сущность 'Начало' не найдена")
        {            
        }

    }
}
