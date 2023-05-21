using EntreNubesBack.DTO.Purchase;

namespace EntreNubesBack.BLL.Services.Contracts;

public interface IPurchaseService
{
    Task<List<PurchaseDto>> PurchaseList();
    Task<PurchaseDto> CreatePurchase(CreatePurchaseDto info);
    Task<bool> DeletePurchase(int purchaseId);
}