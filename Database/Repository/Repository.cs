using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    public class Repository<Entity> : IRepository<Entity> where Entity : class
    {
        #region private Members
        private readonly MyDbContext _context = null!;
        private DbSet<Entity> _entities = null!;

        #endregion private Members

        protected virtual DbSet<Entity> Entities => _entities ??= _context.Set<Entity>();

        public DbSet<Entity> Table => Entities;


        public IQueryable<Entity> TableNoTracking => Entities.AsNoTracking();

        public async Task<bool> AddAsync(Entity entity)
        {
            await Entities.AddAsync(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> AddRangeAsync(IList<Entity> entity)
        {
            await Entities.AddRangeAsync(entity);
            return await _context.SaveChangesAsync() > 0;

        }
        public async Task<bool> UpdateAsync(Entity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            return await _context.SaveChangesAsync() > 0;
        }
            
    }
}
 