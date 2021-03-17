using System;

namespace SilentMike.SqlLite.Data.Interfaces.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        int Commit();
        void ResetEntries();
    }
}
