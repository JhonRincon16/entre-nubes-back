using AutoMapper;
using EntreNubesBack.BLL.Services.Contracts;
using EntreNubesBack.DAL.Repositories.Contracts;
using EntreNubesBack.DTO.Purchase;
using EntreNubesBack.Models;
using Microsoft.EntityFrameworkCore;

namespace EntreNubesBack.BLL.Services;

public class PurchaseService : IPurchaseService
{
    private readonly IPurchaseRepository _purchaseRepository;
    private readonly IGenericRepository<User> _userRepository;
    private readonly ICashClosingRepository _cashClosingRepository;
    private readonly IMapper _mapper;
    
    public PurchaseService(IPurchaseRepository purchaseRepository, 
                           IMapper mapper, 
                           IGenericRepository<User> userRepository, 
                           ICashClosingRepository cashClosingRepository)
    {
        _purchaseRepository = purchaseRepository;
        _mapper = mapper;
        _userRepository = userRepository;
        _cashClosingRepository = cashClosingRepository;
    }

    public async Task<List<PurchaseDto>> PurchaseList()
    {
        var list = await _purchaseRepository.Consult(p => p.State);
        var aux = list.Include(p => p.IdPersonNavigation)
            .Include(p => p.IdPaymentTypeNavigation)
            .Include(p => p.PurchaseDetails)
            .ThenInclude(p => p.IdProductNavigation);
        return _mapper.Map<List<PurchaseDto>>(aux);
    }

    public async Task<PurchaseDto> CreatePurchase(CreatePurchaseDto info)
    {
        try
        {
            var user = await _userRepository.Consult(u => u.Email == info.EmployeeEmail);
            var person = user.Include(u => u.IdPersonNavigation).FirstOrDefault();
            if (user == null)
                throw new TaskCanceledException("El usuario ingresado no existe");
            var actualCashClosing = await _cashClosingRepository.GetLastCashClosing();
            var purchase = await _purchaseRepository.CreatePurchase(info, person.IdPersonNavigation.IdPerson, actualCashClosing.IdCashClosing);
            if (purchase == null)
                throw new TaskCanceledException("Error al crear la compra");
            return _mapper.Map<PurchaseDto>(purchase);
        }
        catch
        {
            throw;
        }
    }

    public async Task<bool> DeletePurchase(int purchaseId)
    {
        var purchase = await _purchaseRepository.Get(p => p.State && p.IdPurchase == purchaseId);
        if (purchase == null)
            throw new TaskCanceledException("La compra ingresada no existe");
        bool response = await _purchaseRepository.DeletePurchase(purchaseId);
        if (!response)
            throw new TaskCanceledException("Error al eliminar la compra");
        return response;
    }
}