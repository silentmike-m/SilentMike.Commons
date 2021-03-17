using Microsoft.EntityFrameworkCore;
using SilentMike.SqlLite.Data.Interfaces.Abstract;
using SilentMike.SqlLite.Data.Interfaces.Interfaces;
using System;
using System.Linq;

namespace SilentMike.SqlLite.Data.Interfaces.Services
{
    public class UnitOfWork : IUnitOfWork
    {
        private AbstractDbContext _dbContext;

        public UnitOfWork(AbstractDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int Commit()
        {
            return _dbContext.SaveChanges();
        }

        public void ResetEntries()
        {
            foreach (var a in _dbContext.GetChangedEntries().ToList())
                a.State = EntityState.Detached;
        }

        public void Dispose()
        {
            if (_dbContext != null)
            {
                _dbContext.Dispose();
                _dbContext = null;
            }
            GC.SuppressFinalize(this);
        }
    }
}
