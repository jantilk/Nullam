using Microsoft.EntityFrameworkCore;
using Persistence;

namespace UnitTests;

public class TestDbContextFactory : IDbContextFactory<NullamDbContext>
{
    public NullamDbContext CreateDbContext()
    {
        var optionsBuilder = new DbContextOptionsBuilder<NullamDbContext>();
        optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());

        return new NullamDbContext(optionsBuilder.Options);
    }
}
