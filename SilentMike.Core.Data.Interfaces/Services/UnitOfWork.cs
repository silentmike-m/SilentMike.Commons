using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SilentMike.Core.Data.Interfaces.Abstract;
using SilentMike.Core.Data.Interfaces.Interfaces;
using System;
using System.Linq;

namespace SilentMike.Core.Data.Interfaces.Services
{
    public class UnitOfWork<TUser> : IUnitOfWork where TUser : IdentityUser
    {
        private AbstractDbContext<TUser> _dbContext;

        public UnitOfWork(AbstractDbContext<TUser> dbContext)
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
