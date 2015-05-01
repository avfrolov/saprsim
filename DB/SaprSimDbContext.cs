using Entities;
using Entities.impl;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB
{
    public class SaprSimDbContext : DbContext
    {


        public SaprSimDbContext() : base("SaprSimDbContext")
        {
           // this.Configuration.ProxyCreationEnabled = false;
        }

        //public DbSet<Project> projects { get; set; }
        public DbSet<Model> models { get; set; }

        //public DbSet<Entity> entities { get; set; }
        //public DbSet<Resource> resources { get; set; }

        //public DbSet<Procedure> procedures { get; set; }

    }
}
