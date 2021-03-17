using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SilentMike.Core.Data.Interfaces.Abstract;
using SilentMike.Core.Data.Interfaces.Interfaces;
using SilentMike.Core.Data.Interfaces.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;

namespace SilentMike.Core.Data.Interfaces.Repositories
{
    public class ReadOnlyRepository<T, TUser> : IReadOnlyRepository<T>
        where T : Entity, new()
        where TUser : IdentityUser
    {
        protected AbstractDbContext<TUser> DbContext;
        protected readonly DbSet<T> DbSet;

        public ReadOnlyRepository(AbstractDbContext<TUser> dbContext)
        {
            DbContext = dbContext;
            DbSet = dbContext.Set<T>();
        }

        public IEnumerable<T> GetAll()
        {
            IQueryable<T> items = DbSet;
            return items.AsEnumerable();
        }

        public IEnumerable<T> GetAll(List<Expression<Func<T, object>>> includes)
        {
            IQueryable<T> query = DbSet;
            query = GetIncluded(query, includes);
            return query.AsEnumerable();
        }

        public IEnumerable<T> GetAllOrdered<TKey>(PageInfo pageInfo)
        {
            IQueryable<T> query = DbSet;
            query = GetPaged<TKey>(query, pageInfo);
            return query.AsEnumerable();
        }

        public IEnumerable<T> GetAllOrdered<TKey>(List<Expression<Func<T, object>>> includes, PageInfo pageInfo)
        {
            IQueryable<T> query = DbSet;
            query = GetIncluded(query, includes);
            query = GetPaged<TKey>(query, pageInfo);
            return query.AsEnumerable();
        }

        public int GetItemsCount(Expression<Func<T, bool>> predicate)
        {
            IQueryable<T> items = DbSet;
            if (predicate != null)
                items = items.Where(predicate);
            return items.Count();
        }

        public T GetById(int id, List<Expression<Func<T, object>>> includes = null)
        {
            var query = DbSet.Where(c => c.Id == id);

            if (includes != null)
                query = GetIncluded(query, includes);

            var entity = query.FirstOrDefault();

            if (entity == null)
                throw new Exception($"Obiekt typu {typeof(T).Name} id: {id} nie został znaleziony!");
            return entity;
        }

        public IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            var query = DbSet.Where(predicate);
            return query.AsEnumerable();
        }

        public IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate,
            List<Expression<Func<T, object>>> includes)
        {
            var query = DbSet.Where(predicate);
            query = GetIncluded(query, includes);
            return query.AsEnumerable();
        }

        public IEnumerable<T> FindByOrdered<TKey>(Expression<Func<T, bool>> predicate, PageInfo pageInfo)
        {
            var query = DbSet.Where(predicate);
            query = GetPaged<TKey>(query, pageInfo);
            return query.AsEnumerable();
        }

        public IEnumerable<T> FindByOrdered<TKey>(Expression<Func<T, bool>> predicate,
            List<Expression<Func<T, object>>> includes, PageInfo pageInfo)
        {
            var query = DbSet.Where(predicate);
            query = GetIncluded(query, includes);
            query = GetPaged<TKey>(query, pageInfo);
            return query.AsEnumerable();
        }

        public IEnumerable<T> FindByOrderedWithId<TKey>(Expression<Func<T, bool>> predicate, PageInfo pageInfo, int id)
        {
            var query = DbSet.Where(predicate);
            var resultQuery = GetPagedWithId<TKey>(query, pageInfo, id);

            return resultQuery;
        }

        private IQueryable<T> GetIncluded(IQueryable<T> query, List<Expression<Func<T, object>>> includes)
        {
            if (includes == null) return query;
            foreach (var include in includes)
                query = query.Include(include);
            return query;
        }

        private IQueryable<T> GetPaged<TKey>(IQueryable<T> query, PageInfo pageInfo)
        {
            if (pageInfo == null)
                return query;
            var prop = string.IsNullOrEmpty(pageInfo.OrderByPropertyName) ?
                null : TypeDescriptor.GetConverter(typeof(T));
            if (prop != null)
            {
                query = GetOrderedQuery<TKey>(query, pageInfo)
                    .Skip(pageInfo.CurrentPageNumber * pageInfo.ItemsPerPage).Take(pageInfo.ItemsPerPage);
            }
            else
                query = query.OrderBy(e => e.Id).Skip(pageInfo.CurrentPageNumber * pageInfo.ItemsPerPage).Take(pageInfo.ItemsPerPage);
            return query;
        }

        private IEnumerable<T> GetPagedWithId<TKey>(IQueryable<T> query, PageInfo pageInfo, int id)
        {
            if (pageInfo == null)
                return query;
            var prop = string.IsNullOrEmpty(pageInfo.OrderByPropertyName) ?
                null : TypeDescriptor.GetConverter(typeof(T));

            if (prop != null)
            {
                var primaryQuery = GetOrderedQuery<TKey>(query, pageInfo).Skip(pageInfo.CurrentPageNumber * pageInfo.ItemsPerPage)
                    .Take(pageInfo.ItemsPerPage).ToList();

                var searchedItem = primaryQuery.FirstOrDefault(item => item.Id == id);

                if (searchedItem != null)
                {
                    return primaryQuery;
                }

                var itemId = GetOrderedQuery<TKey>(query, pageInfo).ToList().FindIndex(item => item.Id == id);
                var pageWithItem = itemId > -1
                    ? itemId / pageInfo.ItemsPerPage
                    : pageInfo.CurrentPageNumber;

                query = GetOrderedQuery<TKey>(query, pageInfo)
                    .Skip(pageWithItem * pageInfo.ItemsPerPage).Take(pageInfo.ItemsPerPage);

                pageInfo.CurrentPageNumber = pageWithItem;
            }
            else
            {
                var primaryQuery = query.OrderBy(e => e.Id).Skip(pageInfo.CurrentPageNumber * pageInfo.ItemsPerPage).Take(pageInfo.ItemsPerPage).AsEnumerable();

                var searchedItem = primaryQuery.FirstOrDefault(item => item.Id == id);

                if (searchedItem != null)
                {
                    return primaryQuery;
                }

                var itemId = query.OrderBy(e => e.Id).ToList().FindIndex(item => item.Id == id);
                var pageWithItem = itemId > -1
                    ? itemId / pageInfo.ItemsPerPage
                    : pageInfo.CurrentPageNumber;

                query = query.OrderBy(e => e.Id).Skip(pageWithItem * pageInfo.ItemsPerPage)
                    .Take(pageInfo.ItemsPerPage);

                pageInfo.CurrentPageNumber = pageWithItem;
            }

            return query.AsEnumerable();
        }

        private IOrderedQueryable<T> GetOrderedQuery<TKey>(IQueryable<T> query, PageInfo pageInfo)
        {
            return pageInfo.IsDescending ?
                query.OrderByDescending(ToLambda<TKey>(pageInfo.OrderByPropertyName)) :
                query.OrderBy(ToLambda<TKey>(pageInfo.OrderByPropertyName));
        }

        private Expression<Func<T, TKey>> ToLambda<TKey>(string propertyName)
        {
            var parameter = Expression.Parameter(typeof(T));
            var property = Expression.Property(parameter, propertyName);

            return Expression.Lambda<Func<T, TKey>>(property, parameter);
        }
    }
}
