using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB
{
    interface DAO <T>
    {
        void save(T value);

        void saveAll(ICollection<T> values);

        T get(int id);

        ICollection<T> getAll();

        void remove(int id);

        void removeAll();

        DbContext getContext();

    }
}
