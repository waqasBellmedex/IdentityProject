using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    public interface IRepository<Entity> where Entity : class
    {
        public DbSet<Entity>  Table { get; }
        public IQueryable<Entity> TableNoTracking { get; }
    }
}
