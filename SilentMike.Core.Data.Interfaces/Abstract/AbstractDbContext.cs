using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SilentMike.Core.Data.Interfaces.Interfaces;
using SilentMike.Core.Data.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SilentMike.Core.Data.Interfaces.Abstract
{
    public abstract class AbstractDbContext<T> : IdentityDbContext<T> where T : IdentityUser
    {
        protected AbstractDbContext(DbContextOptions options) : base(options) { }
        protected abstract List<AssemblyName> GetModelMappingAssemblyNames();
        protected abstract string GetCurrentUserId();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            EntityMappingConfig.CreateMappings(modelBuilder, GetModelMappingAssemblyNames());
            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            var currentUserId = GetCurrentUserId();
            var modifiedEntries = ChangeTracker.Entries().Where(e => e.Entity is ITrackedEntity
                                                                     && (e.State == EntityState.Added || e.State == EntityState.Modified));
            foreach (var entry in modifiedEntries)
            {
                if (!(entry.Entity is ITrackedEntity entity))
                    continue;
                var operationTime = DateTime.UtcNow;

                if (operationTime.Kind == DateTimeKind.Utc)
                {
                    TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
                    operationTime = TimeZoneInfo.ConvertTimeFromUtc(operationTime, easternZone);
                }

                if (entry.State == EntityState.Added)
                {
                    entity.CreatedBy = currentUserId;
                    entity.CreatedDate = operationTime;
                }
                else
                {
                    Entry(entity).Property(e => e.CreatedBy).IsModified = false;
                    Entry(entity).Property(e => e.CreatedDate).IsModified = false;
                }
                entity.UpdatedBy = currentUserId;
                entity.UpdatedDate = operationTime;
            }

            return base.SaveChanges();
        }

        public IEnumerable<EntityEntry> GetChangedEntries()
        {
            var modifiedEntries = ChangeTracker.Entries().Where(e => e.Entity is ITrackedEntity
                                                                     && (e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted));

            return modifiedEntries;

        }
    }
}
