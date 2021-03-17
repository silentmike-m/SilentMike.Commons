using Microsoft.EntityFrameworkCore;

namespace SilentMike.Core.Data.Interfaces.Interfaces
{
    public interface IAmbientDbContextLocator
    {
        TDbContext Get<TDbContext>() where TDbContext : DbContext;
    }
}
