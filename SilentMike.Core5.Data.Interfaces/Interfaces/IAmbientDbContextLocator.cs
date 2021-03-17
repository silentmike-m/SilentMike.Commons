using Microsoft.EntityFrameworkCore;

namespace SilentMike.Core5.Data.Interfaces.Interfaces
{
    public interface IAmbientDbContextLocator
    {
        TDbContext Get<TDbContext>() where TDbContext : DbContext;
    }
}
