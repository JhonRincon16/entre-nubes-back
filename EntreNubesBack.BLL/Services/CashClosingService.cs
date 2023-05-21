using AutoMapper;
using EntreNubesBack.BLL.Services.Contracts;
using EntreNubesBack.DAL.Repositories.Contracts;
using EntreNubesBack.DTO.CashClosing;
using EntreNubesBack.DTO.Payment;
using EntreNubesBack.Models;
using Microsoft.EntityFrameworkCore;

namespace EntreNubesBack.BLL.Services;

public class CashClosingService : ICashClosingService
{
    private readonly ICashClosingRepository _cashClosingRepository;
    private readonly IExpenseRepository _expenseRepository;
    private readonly IPurchaseRepository _purchaseRepository;
    private readonly IGenericRepository<PaymentType> _paymentTypeRepository;
    private readonly IGenericRepository<Payment> _paymentRepository;
    private readonly IMapper _mapper;

    public CashClosingService(ICashClosingRepository cashClosingRepository, 
                              IMapper mapper,
                              IExpenseRepository expenseRepository, 
                              IGenericRepository<PaymentType> paymentTypeRepository, 
                              IGenericRepository<Payment> paymentRepository, 
                              IPurchaseRepository purchaseRepository)
    {
        _cashClosingRepository = cashClosingRepository;
        _mapper = mapper;
        _expenseRepository = expenseRepository;
        _paymentTypeRepository = paymentTypeRepository;
        _paymentRepository = paymentRepository;
        _purchaseRepository = purchaseRepository;
    }

    public async Task<CashClosingInfoDto> GetCashClosingInfo()
    {
        var lastCashClosing = await _cashClosingRepository.GetLastCashClosing();
        var expenses = lastCashClosing == null
            ? await _expenseRepository.ConsultAsNoTacking(e => e.State)
            : await _expenseRepository.ConsultAsNoTacking(e => e.State && e.CreationDate > lastCashClosing.StartDate);
        var paymentTypes = _paymentTypeRepository.ConsultAsNoTacking().Result.Select(pt => new PaymentTypeTotalDto(){Id = pt.IdPaymentType, Name = pt.PaymentTypeName}).ToList();
        var payments = lastCashClosing == null 
            ? await _paymentRepository.ConsultAsNoTacking(p => p.State && p.AmountToPay > 0)
            : await _paymentRepository.ConsultAsNoTacking(p => p.State && p.AmountToPay > 0 && p.Date > lastCashClosing.StartDate);;
        var purchases = lastCashClosing == null 
            ? await _purchaseRepository.ConsultAsNoTacking(p => p.State)
            : await _purchaseRepository.ConsultAsNoTacking(p => p.State && p.CreationDate > lastCashClosing.StartDate);
        foreach (var payment in paymentTypes)
        {
            payment.TotalSales = payments.Where(p => p.IdPaymentType == payment.Id).Sum(p => p.AmountToPay);
            payment.TotalExpenses = expenses.Where(e => e.IdPaymentType == payment.Id).Sum(e => e.ExpenseTotal);
            payment.TotalPurchases = purchases.Where(p => p.IdPaymentType == payment.Id).Sum(p => p.PurchaseDetails.Sum(pd => pd.TotalPrice));
        }
        var totalExpenses = paymentTypes.Sum(pt => pt.TotalExpenses);
        var totalSales = paymentTypes.Sum(pt => pt.TotalSales);
        var totalPurchases = paymentTypes.Sum(pt => pt.TotalPurchases);
        return new CashClosingInfoDto()
            { 
                TotalExpenses = totalExpenses, 
                TotalSales = totalSales, 
                TotalPurchases = totalPurchases, 
                TotalByPaymentType = paymentTypes.ToList() 
            };
    }

    public async Task<bool> CloseCashClosing(CloseCashClosing info)
    {
        var cashClosing = await _cashClosingRepository.Get(cc => cc.IdCashClosing == info.CashClosingId && cc.DateCashClosing == null);
        if (cashClosing == null)
            throw new TaskCanceledException("No existe el cuadre de caja o ya fue cerrado");
        cashClosing.DateCashClosing = DateTime.Now;
        cashClosing.IdUser = info.UserId;
        bool result = await _cashClosingRepository.Edit(cashClosing);
        if (!result)
            throw new TaskCanceledException("Error al cerrar el cuadre de caja");
        var newCashClosing = await _cashClosingRepository.Create(new CashClosing()
        {
            StartDate = DateTime.Now,
            BaseCash = info.BaseCash
        });
        if (newCashClosing == null)
            throw new TaskCanceledException("Error al crear el nuevo cuadre de caja");
        return result;
    }

    public async Task<List<CashClosingInfoDto>> GetCashClosingList()
    {
        List<CashClosingInfoDto> list = new List<CashClosingInfoDto>();
        var cashClosingList = await _cashClosingRepository.Consult(cc => cc.DateCashClosing != null);
        foreach (var cashClosing in cashClosingList.ToList())
        {
            var expenses = await _expenseRepository.ConsultAsNoTacking(e => e.State && e.IdCashClosing == cashClosing.IdCashClosing);
            var paymentTypes = _paymentTypeRepository.ConsultAsNoTacking().Result.Select(pt => new PaymentTypeTotalDto(){Id = pt.IdPaymentType, Name = pt.PaymentTypeName}).ToList();
            var payments = await _paymentRepository.Consult(p => p.State && p.AmountToPay > 0);
            var paymentsDetail = payments.Include(p => p.IdSaleNavigation).Where(pd => pd.IdSaleNavigation.IdCashClosing == cashClosing.IdCashClosing).ToList();
            var purchases = await _purchaseRepository.ConsultAsNoTacking(p => p.State && p.IdCashClosing == cashClosing.IdCashClosing);
            foreach (var payment in paymentTypes)
            {
                payment.TotalSales = paymentsDetail.Where(p => p.IdPaymentType == payment.Id).Sum(p => p.AmountToPay);
                payment.TotalExpenses = expenses.Where(e => e.IdPaymentType == payment.Id).Sum(e => e.ExpenseTotal);
                payment.TotalPurchases = purchases.Where(p => p.IdPaymentType == payment.Id).Sum(p => p.PurchaseDetails.Sum(pd => pd.TotalPrice));
            }
            var totalExpenses = paymentTypes.Sum(pt => pt.TotalExpenses);
            var totalSales = paymentTypes.Sum(pt => pt.TotalSales);
            var totalPurchases = paymentTypes.Sum(pt => pt.TotalPurchases);
            CashClosingInfoDto info = new CashClosingInfoDto()
            { 
                TotalExpenses = totalExpenses, 
                TotalSales = totalSales, 
                TotalPurchases = totalPurchases, 
                TotalByPaymentType = paymentTypes.ToList() 
            };
            list.Add(info);
        }
        return list;
    }

    public async Task<bool> UpdateBaseCash(int cashClosingId, double baseCash)
    {
        var cashClosing = await _cashClosingRepository.Get(cc => cc.IdCashClosing == cashClosingId);
        cashClosing.BaseCash = baseCash;
        bool result = await _cashClosingRepository.Edit(cashClosing);
        if (!result)
            throw new TaskCanceledException("Error al asignar el nuevo efectivo base");
        return result;
    }
}