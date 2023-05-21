using AutoMapper;
using EntreNubesBack.BLL.Services.Contracts;
using EntreNubesBack.DAL.Repositories.Contracts;
using EntreNubesBack.DTO;
using EntreNubesBack.DTO.Expense;
using EntreNubesBack.Models;
using Microsoft.EntityFrameworkCore;

namespace EntreNubesBack.BLL.Services;

public class ExpenseService : IExpenseService
{
    private readonly IExpenseRepository _expenseRepository;
    private readonly IThirdPartyRepository _thirdPartyRepository;
    private readonly IGenericRepository<TypesExpense> _typeExpenseRepository;
    private readonly IGenericRepository<PaymentType> _paymentTypeRepository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly ICashClosingRepository _cashClosingRepository;
    private readonly IMapper _mapper;

    public ExpenseService(IExpenseRepository expenseRepository, 
                          IThirdPartyRepository thirdPartyRepository, 
                          IGenericRepository<TypesExpense> typeExpenseRepository,
                          IMapper mapper, IGenericRepository<PaymentType> paymentTypeRepository, 
                          IEmployeeRepository employeeRepository, 
                          ICashClosingRepository cashClosingRepository)
    {
        _expenseRepository = expenseRepository;
        _thirdPartyRepository = thirdPartyRepository;
        _paymentTypeRepository = paymentTypeRepository;
        _employeeRepository = employeeRepository;
        _cashClosingRepository = cashClosingRepository;
        _typeExpenseRepository = typeExpenseRepository;
        _mapper = mapper;
    }

    public async Task<List<TypeExpenseDto>> ExpenseTypesList()
    {
        var expenseTypes = await _typeExpenseRepository.Consult(et => et.State);
        return _mapper.Map<List<TypeExpenseDto>>(expenseTypes.ToList());
    }

    public async Task<List<PaymentTypeDto>> PaymentMethodsList()
    {
        var paymentMethods = await _paymentTypeRepository.Consult(pt => pt.State);
        return _mapper.Map<List<PaymentTypeDto>>(paymentMethods.ToList());
    }

    public async Task<List<ExpenseDto>> ExpensesList()
    {
        var expenses = await _expenseRepository.Consult(e => e.State);
        var expensesData = expenses.Include(e => e.IdPaymentTypeNavigation)
                .Include(e => e.IdThirdPartyNavigation)
                .Include(e => e.IdEmployeeNavigation)
                .Include(e => e.IdTypeExpenseNavigation);
        return _mapper.Map<List<ExpenseDto>>(expensesData.ToList());
    }

    public async Task<ExpenseDto> CreateExpense(CreateExpenseDto info)
    {
        try
        {
            int? providerId = null;
            if(info.ProviderId != 0)
            {
                var provider = await _thirdPartyRepository.Get(tp => tp.IdThirdParty == info.ProviderId);
                if (provider == null)
                    throw new TaskCanceledException("El proveedor no existe");
                providerId = provider.IdThirdParty;
            }
            var type = await _typeExpenseRepository.Get(et => et.TypeExpenseName.Trim().ToLower() == info.ExpenseType.Trim().ToLower());
            if (type == null)
            {
                type = await _typeExpenseRepository.Create(new TypesExpense()
                {
                    TypeExpenseName = info.ExpenseType,
                    State = true,
                });
            }
            var paymentMethod = await _paymentTypeRepository.Get(pt => pt.IdPaymentType == info.PaymentMethodId);
            if (paymentMethod == null)
                throw new TaskCanceledException("El tipo de pago no existe");

            var actualCashClosing = await _cashClosingRepository.GetLastCashClosing();
            var expense = await _expenseRepository.Create(new Expense()
            {
                Date = DateTime.Now,
                ExpenseDescription = info.Description,
                ExpenseTotal = info.Total,
                IdTypeExpense = type.IdTypeExpense,
                IdPaymentType = info.PaymentMethodId,
                IdThirdParty = providerId,
                State = true,
                CreationDate = DateTime.Now,
                IdCashClosing = actualCashClosing.IdCashClosing
            }) ;
            if (expense == null)
                throw new TaskCanceledException("Error al crear el gasto");
            return _mapper.Map<ExpenseDto>(expense);
        }
        catch
        {
            throw;
        }
    }

    public async Task<bool> EditExpense(EditExpenseDto info)
    {
        try
        {
            var expense = await _expenseRepository.Get(e => e.IdExpense == info.IdExpense);
            if (expense == null)
                throw new TaskCanceledException("El gasto no existe");
            ThirdParty provider = new ThirdParty();
            if (info.ProviderId != 0)
            {
                provider = await _thirdPartyRepository.Get(tp => tp.IdThirdParty == info.ProviderId);
                if(provider.IdThirdParty == 0)
                    throw new TaskCanceledException("El proveedor no existe");
            }
            var type = await _typeExpenseRepository.Get(et => et.TypeExpenseName.Trim().ToLower() == info.ExpenseType.Trim().ToLower());
            if (type == null)
            {
                type = await _typeExpenseRepository.Create(new TypesExpense()
                {
                    TypeExpenseName = info.ExpenseType,
                    State = true,
                });
            }
            var paymentMethod = await _paymentTypeRepository.Get(pt => pt.IdPaymentType == info.PaymentMethodId);
            if (paymentMethod == null)
                throw new TaskCanceledException("El tipo de pago no existe");
            expense.ExpenseDescription = info.Description;
            expense.ExpenseTotal = info.Total;
            expense.IdTypeExpense = type.IdTypeExpense;
            expense.IdPaymentType = info.PaymentMethodId;
            expense.IdThirdParty = provider.IdThirdParty == 0 ? null : provider.IdThirdParty;

            bool result = await _expenseRepository.Edit(expense);
            if (!result)
                throw new TaskCanceledException("Error al editar el gasto");
            return result;
        }
        catch
        {
            throw;
        }
    }

    public async Task<bool> DeleteExpense(int expenseId)
    {
        try
        {
            var expense = await _expenseRepository.Get(e => e.IdExpense == expenseId);
            if (expense == null)
                throw new TaskCanceledException("No existe el gasto");
            expense.State = false;
            bool result = await _expenseRepository.Edit(expense);
            if(!result)
                throw new TaskCanceledException("Error al eliminar el gasto");
            return result;
        }
        catch
        {
            throw;
        }
    }

    public async Task<ExpenseDto> CreatePayrollExpense(CreatePayrollExpenseDto info)
    {
        try
        {
            var employee = await _employeeRepository.Get(e => e.IdEmployee == info.EmployeeId);
            if (employee == null)
                throw new TaskCanceledException("No existe el empleado");
            var type = await _typeExpenseRepository.Get(et => et.TypeExpenseName.Trim().ToLower() == "Nomina");
            if (type == null)
            {
                type = await _typeExpenseRepository.Create(new TypesExpense()
                {
                    TypeExpenseName = "Nomina",
                    State = true,
                });
            }
            var paymentMethod = await _paymentTypeRepository.Get(pt => pt.IdPaymentType == info.PaymentMethodId);
            if (paymentMethod == null)
                throw new TaskCanceledException("El tipo de pago no existe");
            
            var actualCashClosing = await _cashClosingRepository.GetLastCashClosing();
            var expense = await _expenseRepository.Create(new Expense()
            {
                Date = DateTime.Now,
                ExpenseDescription = info.Description,
                ExpenseTotal = info.Total,
                IdTypeExpense = type.IdTypeExpense,
                IdPaymentType = info.PaymentMethodId,
                IdEmployee = employee.IdEmployee,
                State = true,
                CreationDate = DateTime.Now,
                LastIncomeId = info.LastIncomeId,
                IdCashClosing = actualCashClosing.IdCashClosing
            });
            if (expense == null)
                throw new TaskCanceledException("Error al crear el gasto de nomina");
            return _mapper.Map<ExpenseDto>(expense);
        }
        catch
        {
            throw;
        }
    }
}