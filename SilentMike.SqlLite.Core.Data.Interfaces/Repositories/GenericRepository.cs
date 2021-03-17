using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SilentMike.SqlLite.Core.Data.Interfaces.Abstract;
using SilentMike.SqlLite.Core.Data.Interfaces.Interfaces;
using SilentMike.SqlLite.Core.Data.Interfaces.Models;
using SilentMike.SqlLite.Core.Data.Interfaces.Services;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SilentMike.SqlLite.Core.Data.Interfaces.Repositories
{
    public class GenericRepository<T, TUser> : ReadOnlyRepository<T, TUser>, IGenericRepository<T>
         where T : TrackedEntity, new()
         where TUser : IdentityUser
    {
        public GenericRepository(AbstractDbContext<TUser> dbContext) : base(dbContext)
        {

        }

        public T Add(T entity, HashSet<string> includedEntities = null)
        {
            RemoveSubEntities(entity, includedEntities);
            var result = DbSet.Add(entity);
            return result.Entity;
        }

        public T Edit(T entity, bool withReflection = true)
        {
            T entityDestination;
            if (!withReflection)
            {
                entityDestination = entity;
            }
            else
            {
                entityDestination = GetById(entity.Id);
                entityDestination = ReflectionMapper<T, T>.Map(entity, entityDestination);
            }

            RemoveSubEntities(entityDestination, null);
            DetachDiffEntities(entity);
            AttachEntity(entityDestination);

            DbContext.Entry(entityDestination).State = EntityState.Modified;
            return entityDestination;
        }

        public void Delete(T entity)
        {
            AttachEntity(entity);
            DbContext.Remove(entity);
        }

        public void Delete(int id)
        {
            var dEntity = new T { Id = id };
            Delete(dEntity);
        }

        private void AttachEntity(T entity)
        {   
            DbSet.Attach(entity);
        }

        private void DetachDiffEntities(T entity)
        {
            foreach (var changedEntity in DbContext.GetChangedEntries().ToList())
            {
                changedEntity.State = EntityState.Detached;
            }
        }

        private void RemoveSubEntities(T entity, HashSet<string> excludedEntities)
        {
            if (excludedEntities == null)
                excludedEntities = new HashSet<string>();

            foreach (var property in typeof(T).GetRuntimeProperties())
            {
                var propertyType = property.PropertyType;
                if (propertyType.GetInterfaces().Contains(typeof(IEntity)))
                {
                    if (excludedEntities.Contains(property.Name))
                        continue;

                    property.SetValue(entity, null);
                    continue;
                }

                if (!propertyType.IsGenericType || propertyType.GetGenericTypeDefinition() != typeof(ICollection<>))
                    continue;
                var itemType = propertyType.GenericTypeArguments.First();
                if (!itemType.GetInterfaces().Contains(typeof(IEntity)))
                    continue;

                if (excludedEntities.Contains(property.Name))
                    continue;

                property.SetValue(entity, null);
            }
        }
    }
}
