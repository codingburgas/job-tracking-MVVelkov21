using JobTracking.Application.Contracts;
using JobTracking.Application.Contracts.Base;
using JobTracking.DataAccess.Data.Models;

namespace JobTracking.Application.Implementation;

public class SingerService : ISingerService
{
    protected DependencyProvider Provider { get; set; }
    public SingerService(DependencyProvider provider)
    {
        Provider = provider;
    }
    
    public Task<IQueryable<Temp>> GetAllSingers(int page, int pageCount)
    {
        return Provider.Db.Temporary.Skip(page-1 * pageCount).Take(pageCount).ToListAsync();
    }

    public Task<Temp> GetSinger(int singerId)
    {
        return Provider.Db.Temporary.Where(x => x.Id == singerId).FirstOrDefaultAsync();
    }
}