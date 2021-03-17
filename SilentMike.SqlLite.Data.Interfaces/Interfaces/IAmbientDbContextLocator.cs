using Microsoft.EntityFrameworkCore;

namespace SilentMike.SqlLite.Data.Interfaces.Interfaces
{
    public interface IAmbientDbContextLocator
    {
        TDbContext Get<TDbContext>() where TDbContext : DbContext;
    }
}
