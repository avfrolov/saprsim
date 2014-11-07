using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    class Model
    {
        List<Entity> entities = new List<Entity>();

        public void addEntity(Entity entity)
        {
            entities.Add(entity);
        }

        public List<Entity> getEntities()
        {
            return entities;
        }
    }
}
