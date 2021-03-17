using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SilentMike.SqlLite.Core.Data.Interfaces.Interfaces
{
    public interface IGenericRepository<T> where T : ITrackedEntity
    {
        IEnumerable<T> GetAll();
        IEnumerable<T> GetAll(List<Expression<Func<T, object>>> includes);
        int GetItemsCount(Expression<Func<T, bool>> predicate);
        T GetById(int id, List<Expression<Func<T, object>>> includes = null);
        IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate);
        T Add(T entity, HashSet<string> includedEntities = null);
        T Edit(T entity, bool withReflection = true);
        void Delete(T entity);
        void Delete(int id);
    }
}
