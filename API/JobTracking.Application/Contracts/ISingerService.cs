using JobTracking.DataAccess.Data;
using JobTracking.DataAccess.Data.Models;

namespace JobTracking.Application.Contracts;

public interface ISingerService
{
    public Task<IQueryable<Temp>> GetAllSingers(int page, int pageCount);
    public Task<Temp> GetSinger(int id);
}