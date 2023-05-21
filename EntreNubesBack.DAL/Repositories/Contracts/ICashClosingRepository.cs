using EntreNubesBack.DTO.CashClosing;
using EntreNubesBack.Models;

namespace EntreNubesBack.DAL.Repositories.Contracts;

public interface ICashClosingRepository : IGenericRepository<CashClosing>
{
    Task<CashClosing> GetLastCashClosing();
}