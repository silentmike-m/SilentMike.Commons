using System.Collections.Generic;

namespace SilentMike.Core5.Data.Interfaces.Interfaces
{
    public interface IGenericRepository<T> : IReadOnlyRepository<T> where T : ITrackedEntity
    {
        T Add(T entity, HashSet<string> includedEntities = null);
        T Edit(T entity, bool withReflection = true);
        void Delete(T entity);
        void Delete(int id);
    }
}
