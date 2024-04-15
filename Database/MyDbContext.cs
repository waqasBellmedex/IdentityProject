using Domain.Common;
using Domain.Entities;
using Domain.Interface;
using Domain.Model;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Reflection;

namespace Database
{
    public class MyDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, long, IdentityUserClaim<long>, ApplicationUserRole, IdentityUserLogin<long>, IdentityRoleClaim<long>, IdentityUserToken<long>>, ITransactionService
    {
        private readonly ICurrentUserService _currentUserService;

        public MyDbContext(DbContextOptions<MyDbContext> options,ICurrentUserService currentUserService) : base(options)
        {
            _currentUserService = currentUserService;
        }




        ///Write SaveChangesAsync Override function;
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries().ToList())
            {
                switch(entry.State)
                {
                    case EntityState.Added:
                        SetCreationProperties(entry.Entity, _currentUserService.UserId.ToString());
                        break;

                    case EntityState.Modified:
                        SetModificationProperties(entry.Entity, _currentUserService.UserId.ToString());
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }

        private static void SetCreationProperties(object entityAsObj,string userId)
        {
            if(string.IsNullOrWhiteSpace(userId))
            {
                return;
            }
            var baseEntity = entityAsObj.As<BaseEntity>();
            if(baseEntity!=null)
            {
                if (entityAsObj is ICreationAudited)
                {
                    baseEntity.CreatedDate = DateTime.Now;
                    baseEntity.ModifiedDate = DateTime.Now;
                }
                if (baseEntity!.CreatedBy != null)
                    return;

                baseEntity.CreatedBy = userId;
                baseEntity.ModifiedBy = userId;
            }
        }

        private static void SetModificationProperties(object entityAsObj, string userId)
        {
            var baseEntity = entityAsObj.As<BaseEntity>();
            if(!(baseEntity is IModificationAudited))
            {
                baseEntity.ModifiedDate = DateTime.Now;
            }
            if(userId==null)
            {
                baseEntity.ModifiedBy = null;
                return;
            }
            baseEntity.ModifiedBy = userId;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());


            base.OnModelCreating(builder);
        }


    }
}
