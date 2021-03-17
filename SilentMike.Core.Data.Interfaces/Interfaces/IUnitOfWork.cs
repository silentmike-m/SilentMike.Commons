using System;

namespace SilentMike.Core.Data.Interfaces.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        int Commit();
        void ResetEntries();
    }
}
