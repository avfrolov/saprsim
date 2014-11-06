using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public abstract class Entity
    {
        String name {get; set;}
        long id {get; set;}

        List<Entity> input = new List<Entity>();
        List<Entity> output = new List<Entity>();

        public abstract void execute();
    }
}
