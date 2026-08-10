using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Overtime.Data;

// Used only by `dotnet ef` design-time tooling to generate migrations.
public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=Overtime;Trusted_Connection=True")
            .Options;
        return new AppDbContext(options);
    }
}
