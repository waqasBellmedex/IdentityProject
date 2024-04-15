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
        DbSet<Entity>  Table { get; }
        IQueryable<Entity> TableNoTracking { get; }
        Task<bool> AddAsync(Entity entity);
        Task<bool> AddRangeAsync(IList<Entity> entity);
        Task<bool> UpdateAsync(Entity entity);
    }
}
