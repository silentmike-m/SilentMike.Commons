using SilentMike.SqlLite.Data.Interfaces.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SilentMike.SqlLite.Data.Interfaces.Interfaces
{
    public interface IReadOnlyRepository<T> where T : IEntity
    {
        IEnumerable<T> GetAll();
        IEnumerable<T> GetAll(List<Expression<Func<T, object>>> includes);
        IEnumerable<T> GetAllOrdered<TKey>(PageInfo pageInfo);
        IEnumerable<T> GetAllOrdered<TKey>(List<Expression<Func<T, object>>> includes, PageInfo pageInfo);
        int GetItemsCount(Expression<Func<T, bool>> predicate);
        T GetById(int id, List<Expression<Func<T, object>>> includes = null);
        IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate);
        IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate, List<Expression<Func<T, object>>> includes);
        IEnumerable<T> FindByOrdered<TKey>(Expression<Func<T, bool>> predicate, PageInfo pageInfo);
        IEnumerable<T> FindByOrdered<TKey>(Expression<Func<T, bool>> predicate, List<Expression<Func<T, object>>> includes, PageInfo pageInfo);
        IEnumerable<T> FindByOrderedWithId<TKey>(Expression<Func<T, bool>> predicate, PageInfo pageInfo, int id);
    }
}
