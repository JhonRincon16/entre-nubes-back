using EntreNubesBack.DTO.Purchase;
using EntreNubesBack.Models;

namespace EntreNubesBack.DAL.Repositories.Contracts;

public interface IPurchaseRepository : IGenericRepository<Purchase>
{
    Task<Purchase> CreatePurchase(CreatePurchaseDto info, int personId, int cashClosing);

    Task<bool> DeletePurchase(int purchaseId);
}