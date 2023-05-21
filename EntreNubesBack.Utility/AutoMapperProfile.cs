using AutoMapper;
using EntreNubesBack.DTO;
using EntreNubesBack.DTO.Account;
using EntreNubesBack.DTO.Employee;
using EntreNubesBack.DTO.Expense;
using EntreNubesBack.DTO.Payment;
using EntreNubesBack.DTO.Product;
using EntreNubesBack.DTO.Purchase;
using EntreNubesBack.DTO.Sale;
using EntreNubesBack.Models;
using Action = EntreNubesBack.Models.Action;

namespace EntreNubesBack.Utility;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        #region Actions
        CreateMap<Action, ActionDto>();
        CreateMap<ActionDto, Action>();
        #endregion
        
        #region Rol
        CreateMap<Role, RolDto>()
            .ForMember(o => o.IdActions, opt => opt.MapFrom(o => o.IdActions));
        CreateMap<CreateRolDto, Role>();
        CreateMap<EditRolDto, Role>()
            .ForMember(o => o.IdActions, opt => opt.MapFrom(o => o.Name));
        #endregion
        
        #region User
        CreateMap<User, UserDto>()
            .ForMember(d => d.Rol, opt => opt.MapFrom(o => o.IdRolNavigation))
            .ForMember(d => d.Person, opt => opt.MapFrom(o => o.IdPersonNavigation));

        CreateMap<CreateUserDto, User>()
            .ForMember(d => d.IdRolNavigation, opt => opt.Ignore())
            .ForMember(d => d.IdPersonNavigation, opt => opt.Ignore());

        CreateMap<User, BaseUserDto>()
            .ForMember(d => d.IsWorker, opt => opt.MapFrom(o => o.IdPersonNavigation.Employees.Count > 0));
        #endregion

        #region Person
        CreateMap<Person, PersonDto>();
        #endregion

        #region ThirdParty
        CreateMap<ThirdParty, ThirdPartyDto>()
            .ForMember(d => d.AdvisorDto,  opt => opt.MapFrom(o => o.IdPersonNavigation));
        CreateMap<ThirdPartyDto, ThirdParty>();
        #endregion

        #region Product
        CreateMap<Product, ProductDto>();
        CreateMap<ProductsDetail, ProductDetailDto>();
        #endregion

        #region Employee
        CreateMap<Employee, EmployeeDto>()
            .ForMember(d => d.UserDto, opt => opt.MapFrom(o => o.IdPersonNavigation.Users.First()));
        #endregion

        #region Expense
        CreateMap<Expense, ExpenseDto>();
        #endregion

        #region Account
        CreateMap<Account, AccountDto>();
        CreateMap<AddProductUnitsDetail, AddProductUnitsDetailDto>();
        #endregion
        
        #region Purchase
        CreateMap<Purchase, PurchaseDto>()
            .ForMember(d => d.Person, opt => opt.MapFrom(o => o.IdPersonNavigation))
            .ForMember(d => d.Products, opt => opt.MapFrom(o => o.PurchaseDetails));
        CreateMap<PurchaseDetail, PurchaseDetailDto>();
        #endregion

        CreateMap<TypesExpense, TypeExpenseDto>();
        CreateMap<PaymentType, PaymentTypeDto>();
        CreateMap<Payment, PaymentDto>();
        CreateMap<Sale, SaleDto>();
    }
}