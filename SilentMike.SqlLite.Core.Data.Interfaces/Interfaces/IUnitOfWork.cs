using System;

namespace SilentMike.SqlLite.Core.Data.Interfaces.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        int Commit();
        void ResetEntries();
    }
}
