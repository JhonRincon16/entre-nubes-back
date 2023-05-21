using EntreNubesBack.DAL.DBContext;
using EntreNubesBack.DAL.Repositories.Contracts;
using EntreNubesBack.DTO.CashClosing;
using EntreNubesBack.Models;
using Microsoft.EntityFrameworkCore;

namespace EntreNubesBack.DAL.Repositories;

public class CashClosingRepository : GenericRepository<CashClosing>, ICashClosingRepository
{
    public CashClosingRepository(EntrenubesContext dbContext) : base(dbContext)
    {
    }

    public async Task<CashClosing> GetLastCashClosing()
    {
        var cashClosings = await ConsultAsNoTacking();
        var lastCashClosing = cashClosings.OrderByDescending(cc => cc.DateCashClosing).LastOrDefault();
        if (lastCashClosing == null)
        {
            lastCashClosing = await Create(new CashClosing()
            {
                StartDate = DateTime.Now
            }); 
        }
        return lastCashClosing;
    }
}