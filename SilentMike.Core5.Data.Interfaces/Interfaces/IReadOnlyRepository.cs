using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SilentMike.Core5.Data.Interfaces.Interfaces
{
    public interface IReadOnlyRepository<T> where T : IEntity
    {
        IEnumerable<T> GetAll();
        IEnumerable<T> GetAll(List<Expression<Func<T, object>>> includes);
        IEnumerable<T> GetAllOrdered<TKey>(IPageInfo pageInfo);
        IEnumerable<T> GetAllOrdered<TKey>(List<Expression<Func<T, object>>> includes, IPageInfo pageInfo);
        int GetItemsCount(Expression<Func<T, bool>> predicate);
        T GetById(int id, List<Expression<Func<T, object>>> includes = null);
        IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate);
        IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate, List<Expression<Func<T, object>>> includes);
        IEnumerable<T> FindByOrdered<TKey>(Expression<Func<T, bool>> predicate, IPageInfo pageInfo);
        IEnumerable<T> FindByOrdered<TKey>(Expression<Func<T, bool>> predicate, List<Expression<Func<T, object>>> includes, IPageInfo pageInfo);
        IEnumerable<T> FindByOrderedWithId<TKey>(Expression<Func<T, bool>> predicate, IPageInfo pageInfo, int id);
    }
}
