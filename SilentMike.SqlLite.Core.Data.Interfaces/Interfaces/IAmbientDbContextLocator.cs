using Microsoft.EntityFrameworkCore;

namespace SilentMike.SqlLite.Core.Data.Interfaces.Interfaces
{
    public interface IAmbientDbContextLocator
    {
        TDbContext Get<TDbContext>() where TDbContext : DbContext;
    }
}
