using JobTracking.DataAccess;

namespace JobTracking.Application.Contracts.Base;

public class DependencyProvider
{
    public DependencyProvider(AppDbContext dbContext)
    {
        Db = dbContext;
    }
    public AppDbContext Db { get; set; }
}