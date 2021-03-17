using System;

namespace SilentMike.Core5.Data.Interfaces.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        int Commit();
        void ResetEntries();
    }
}
