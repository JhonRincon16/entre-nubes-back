using EntreNubesBack.DTO.CashClosing;

namespace EntreNubesBack.BLL.Services.Contracts;

public interface ICashClosingService
{
    Task<CashClosingInfoDto> GetCashClosingInfo();
    Task<List<CashClosingInfoDto>> GetCashClosingList();
    Task<bool> CloseCashClosing(CloseCashClosing info);
    Task<bool> UpdateBaseCash(int cashClosingId, double baseCash);
}