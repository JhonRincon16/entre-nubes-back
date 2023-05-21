using EntreNubesBack.Models;
using Microsoft.EntityFrameworkCore;
using Action = EntreNubesBack.Models.Action;

namespace EntreNubesBack.DAL.DBContext;

public partial class EntrenubesContext : DbContext
{
    public EntrenubesContext()
    {
    }

    public EntrenubesContext(DbContextOptions<EntrenubesContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Action> Actions { get; set; }

    public virtual DbSet<AddProductUnitsDetail> AddProductUnitsDetails { get; set; }

    public virtual DbSet<CashClosing> CashClosings { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<EmployeesIncome> EmployeesIncomes { get; set; }

    public virtual DbSet<Expense> Expenses { get; set; }

    public virtual DbSet<HistoryChange> HistoryChanges { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<PaymentType> PaymentTypes { get; set; }

    public virtual DbSet<Person> People { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductsDetail> ProductsDetails { get; set; }

    public virtual DbSet<Purchase> Purchases { get; set; }

    public virtual DbSet<PurchaseDetail> PurchaseDetails { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Sale> Sales { get; set; }

    public virtual DbSet<ThirdParty> ThirdParties { get; set; }

    public virtual DbSet<TypesExpense> TypesExpenses { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.IdAccount).HasName("PRIMARY");

            entity.ToTable("accounts");

            entity.HasIndex(e => e.IdSale, "FK_ACCOUNTS_SALES");

            entity.Property(e => e.IdAccount).HasColumnName("ID_ACCOUNT");
            entity.Property(e => e.AccountName)
                .HasMaxLength(20)
                .HasColumnName("ACCOUNT_NAME");
            entity.Property(e => e.CreationDate)
                .HasColumnType("datetime")
                .HasColumnName("CREATION_DATE");
            entity.Property(e => e.IdSale).HasColumnName("ID_SALE");
            entity.Property(e => e.IsClosed).HasColumnName("IS_CLOSED");
            entity.Property(e => e.State).HasColumnName("STATE");

            entity.HasOne(d => d.IdSaleNavigation).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.IdSale)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_ACCOUNTS_SALES");
        });

        modelBuilder.Entity<Action>(entity =>
        {
            entity.HasKey(e => e.IdAction).HasName("PRIMARY");

            entity.ToTable("actions");

            entity.Property(e => e.IdAction).HasColumnName("ID_ACTION");
            entity.Property(e => e.ActionName)
                .HasMaxLength(50)
                .HasColumnName("ACTION_NAME");
            entity.Property(e => e.State).HasColumnName("STATE");
        });

        modelBuilder.Entity<AddProductUnitsDetail>(entity =>
        {
            entity.HasKey(e => e.IdAddUnits).HasName("PRIMARY");

            entity.ToTable("add_product_units_detail");

            entity.HasIndex(e => e.IdDetail, "FK_PROUNIDET_PRODET");

            entity.Property(e => e.IdAddUnits).HasColumnName("ID_ADD_UNITS");
            entity.Property(e => e.CreationDate)
                .HasColumnType("datetime")
                .HasColumnName("CREATION_DATE");
            entity.Property(e => e.Description)
                .HasMaxLength(200)
                .HasColumnName("DESCRIPTION");
            entity.Property(e => e.IdDetail).HasColumnName("ID_DETAIL");
            entity.Property(e => e.ProductQuantity).HasColumnName("PRODUCT_QUANTITY");

            entity.HasOne(d => d.IdDetailNavigation).WithMany(p => p.AddProductUnitsDetails)
                .HasForeignKey(d => d.IdDetail)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_PROUNIDET_PRODET");
        });

        modelBuilder.Entity<CashClosing>(entity =>
        {
            entity.HasKey(e => e.IdCashClosing).HasName("PRIMARY");

            entity.ToTable("cash_closing");

            entity.HasIndex(e => e.IdUser, "FK_CASHCLOSING_USERS");

            entity.Property(e => e.IdCashClosing).HasColumnName("ID_CASH_CLOSING");
            entity.Property(e => e.BaseCash).HasColumnName("BASE_CASH");
            entity.Property(e => e.DateCashClosing)
                .HasColumnType("datetime")
                .HasColumnName("date_cash_closing");
            entity.Property(e => e.IdUser).HasColumnName("ID_USER");
            entity.Property(e => e.StartDate)
                .HasColumnType("datetime")
                .HasColumnName("START_DATE");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.CashClosings)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_CASHCLOSING_USERS");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.IdEmployee).HasName("PRIMARY");

            entity.ToTable("employees");

            entity.HasIndex(e => e.IdPerson, "FK_EMPLOYEES_PEOPLE");

            entity.Property(e => e.IdEmployee).HasColumnName("ID_EMPLOYEE");
            entity.Property(e => e.EmployeeType)
                .HasMaxLength(20)
                .HasColumnName("EMPLOYEE_TYPE");
            entity.Property(e => e.IdPerson).HasColumnName("ID_PERSON");
            entity.Property(e => e.Salary).HasColumnName("SALARY");
            entity.Property(e => e.SalaryType)
                .HasMaxLength(20)
                .HasColumnName("SALARY_TYPE");
            entity.Property(e => e.State).HasColumnName("STATE");

            entity.HasOne(d => d.IdPersonNavigation).WithMany(p => p.Employees)
                .HasForeignKey(d => d.IdPerson)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_EMPLOYEES_PEOPLE");
        });

        modelBuilder.Entity<EmployeesIncome>(entity =>
        {
            entity.HasKey(e => e.IdEmployeeIncome).HasName("PRIMARY");

            entity.ToTable("employees_income");

            entity.HasIndex(e => e.IdEmployee, "FK_EMPLOYEESINCOME_EMPLOYEES");

            entity.Property(e => e.IdEmployeeIncome).HasColumnName("ID_EMPLOYEE_INCOME");
            entity.Property(e => e.DepartureDate)
                .HasColumnType("datetime")
                .HasColumnName("DEPARTURE_DATE");
            entity.Property(e => e.IdEmployee).HasColumnName("ID_EMPLOYEE");
            entity.Property(e => e.IncomeDate)
                .HasColumnType("datetime")
                .HasColumnName("INCOME_DATE");

            entity.HasOne(d => d.IdEmployeeNavigation).WithMany(p => p.EmployeesIncomes)
                .HasForeignKey(d => d.IdEmployee)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_EMPLOYEESINCOME_EMPLOYEES");
        });

        modelBuilder.Entity<Expense>(entity =>
        {
            entity.HasKey(e => e.IdExpense).HasName("PRIMARY");

            entity.ToTable("expenses");

            entity.HasIndex(e => e.IdEmployee, "FK_EMPLOYEES_EXPENSES");

            entity.HasIndex(e => e.IdCashClosing, "FK_ESPENSE_CASHCLOSING");

            entity.HasIndex(e => e.IdTypeExpense, "FK_EXPENSES_TYPES_EXPENSES");

            entity.HasIndex(e => e.IdPaymentType, "FK_PAYTYP_EXPENSES");

            entity.HasIndex(e => e.IdThirdParty, "FK_THIRDPARTIES_EXPENSES");

            entity.Property(e => e.IdExpense).HasColumnName("ID_EXPENSE");
            entity.Property(e => e.CreationDate)
                .HasColumnType("datetime")
                .HasColumnName("CREATION_DATE");
            entity.Property(e => e.Date)
                .HasColumnType("datetime")
                .HasColumnName("DATE");
            entity.Property(e => e.ExpenseDescription)
                .HasMaxLength(200)
                .HasColumnName("EXPENSE_DESCRIPTION");
            entity.Property(e => e.ExpenseTotal).HasColumnName("EXPENSE_TOTAL");
            entity.Property(e => e.IdCashClosing).HasColumnName("ID_CASH_CLOSING");
            entity.Property(e => e.IdEmployee).HasColumnName("ID_EMPLOYEE");
            entity.Property(e => e.IdPaymentType).HasColumnName("ID_PAYMENT_TYPE");
            entity.Property(e => e.IdThirdParty).HasColumnName("ID_THIRD_PARTY");
            entity.Property(e => e.IdTypeExpense).HasColumnName("ID_TYPE_EXPENSE");
            entity.Property(e => e.LastIncomeId).HasColumnName("LAST_INCOME_ID");
            entity.Property(e => e.State).HasColumnName("STATE");

            entity.HasOne(d => d.IdCashClosingNavigation).WithMany(p => p.Expenses)
                .HasForeignKey(d => d.IdCashClosing)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_ESPENSE_CASHCLOSING");

            entity.HasOne(d => d.IdEmployeeNavigation).WithMany(p => p.Expenses)
                .HasForeignKey(d => d.IdEmployee)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_EMPLOYEES_EXPENSES");

            entity.HasOne(d => d.IdPaymentTypeNavigation).WithMany(p => p.Expenses)
                .HasForeignKey(d => d.IdPaymentType)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_PAYTYP_EXPENSES");

            entity.HasOne(d => d.IdThirdPartyNavigation).WithMany(p => p.Expenses)
                .HasForeignKey(d => d.IdThirdParty)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_THIRDPARTIES_EXPENSES");

            entity.HasOne(d => d.IdTypeExpenseNavigation).WithMany(p => p.Expenses)
                .HasForeignKey(d => d.IdTypeExpense)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_EXPENSES_TYPES_EXPENSES");

            entity.HasMany(d => d.IdPaymentTypes).WithMany(p => p.IdExpenses)
                .UsingEntity<Dictionary<string, object>>(
                    "PaymentTypeExpense",
                    r => r.HasOne<PaymentType>().WithMany()
                        .HasForeignKey("IdPaymentType")
                        .OnDelete(DeleteBehavior.Restrict)
                        .HasConstraintName("FK_PAYTYPEEXPENSES_PAYTYPE"),
                    l => l.HasOne<Expense>().WithMany()
                        .HasForeignKey("IdExpense")
                        .OnDelete(DeleteBehavior.Restrict)
                        .HasConstraintName("FK_PAYMENTTYPE_EXPENSES"),
                    j =>
                    {
                        j.HasKey("IdExpense", "IdPaymentType").HasName("PRIMARY");
                        j.ToTable("payment_type_expenses");
                        j.HasIndex(new[] { "IdPaymentType" }, "FK_PAYTYPEEXPENSES_PAYTYPE");
                        j.IndexerProperty<int>("IdExpense").HasColumnName("ID_EXPENSE");
                        j.IndexerProperty<int>("IdPaymentType").HasColumnName("ID_PAYMENT_TYPE");
                    });
        });

        modelBuilder.Entity<HistoryChange>(entity =>
        {
            entity.HasKey(e => e.IdHistoryChanges).HasName("PRIMARY");

            entity.ToTable("history_changes");

            entity.HasIndex(e => e.IdUser, "FK_CHANGES_USERS");

            entity.Property(e => e.IdHistoryChanges).HasColumnName("ID_HISTORY_CHANGES");
            entity.Property(e => e.Actions)
                .HasMaxLength(1024)
                .HasColumnName("ACTIONS");
            entity.Property(e => e.Date)
                .HasColumnType("datetime")
                .HasColumnName("DATE");
            entity.Property(e => e.IdUser).HasColumnName("ID_USER");
            entity.Property(e => e.TableName)
                .HasMaxLength(1024)
                .HasColumnName("TABLE_NAME");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.HistoryChanges)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_CHANGES_USERS");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.IdPayment).HasName("PRIMARY");

            entity.ToTable("payments");

            entity.HasIndex(e => e.IdPaymentType, "FK_PAYMENTS_PAYTYP");

            entity.HasIndex(e => e.IdProductDetail, "FK_PAYMENTS_PRODET");

            entity.HasIndex(e => e.IdProduct, "FK_PAYMENTS_PRODUCTS");

            entity.HasIndex(e => e.IdSale, "FK_PAYMENTS_SALES");

            entity.Property(e => e.IdPayment).HasColumnName("ID_PAYMENT");
            entity.Property(e => e.AmountToPay).HasColumnName("AMOUNT_TO_PAY");
            entity.Property(e => e.Date)
                .HasColumnType("datetime")
                .HasColumnName("DATE");
            entity.Property(e => e.IdPaymentType).HasColumnName("ID_PAYMENT_TYPE");
            entity.Property(e => e.IdProduct).HasColumnName("ID_PRODUCT");
            entity.Property(e => e.IdProductDetail).HasColumnName("ID_PRODUCT_DETAIL");
            entity.Property(e => e.IdSale).HasColumnName("ID_SALE");
            entity.Property(e => e.Quantity).HasColumnName("QUANTITY");
            entity.Property(e => e.State).HasColumnName("STATE");

            entity.HasOne(d => d.IdPaymentTypeNavigation).WithMany(p => p.Payments)
                .HasForeignKey(d => d.IdPaymentType)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_PAYMENTS_PAYTYP");

            entity.HasOne(d => d.IdProductNavigation).WithMany(p => p.Payments)
                .HasForeignKey(d => d.IdProduct)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_PAYMENTS_PRODUCTS");

            entity.HasOne(d => d.IdProductDetailNavigation).WithMany(p => p.Payments)
                .HasForeignKey(d => d.IdProductDetail)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_PAYMENTS_PRODET");

            entity.HasOne(d => d.IdSaleNavigation).WithMany(p => p.Payments)
                .HasForeignKey(d => d.IdSale)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_PAYMENTS_SALES");
        });

        modelBuilder.Entity<PaymentType>(entity =>
        {
            entity.HasKey(e => e.IdPaymentType).HasName("PRIMARY");

            entity.ToTable("payment_types");

            entity.Property(e => e.IdPaymentType).HasColumnName("ID_PAYMENT_TYPE");
            entity.Property(e => e.PaymentTypeName)
                .HasMaxLength(50)
                .HasColumnName("PAYMENT_TYPE_NAME");
            entity.Property(e => e.State).HasColumnName("STATE");
        });

        modelBuilder.Entity<Person>(entity =>
        {
            entity.HasKey(e => e.IdPerson).HasName("PRIMARY");

            entity.ToTable("people");

            entity.Property(e => e.IdPerson).HasColumnName("ID_PERSON");
            entity.Property(e => e.DocumentNumber)
                .HasMaxLength(50)
                .HasColumnName("DOCUMENT_NUMBER");
            entity.Property(e => e.DocumentType)
                .HasMaxLength(4)
                .IsFixedLength()
                .HasColumnName("DOCUMENT_TYPE");
            entity.Property(e => e.PersonName)
                .HasMaxLength(50)
                .HasColumnName("PERSON_NAME");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(50)
                .HasColumnName("PHONE_NUMBER");
            entity.Property(e => e.State).HasColumnName("STATE");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.IdProduct).HasName("PRIMARY");

            entity.ToTable("products");

            entity.Property(e => e.IdProduct).HasColumnName("ID_PRODUCT");
            entity.Property(e => e.ProductCategory)
                .HasMaxLength(20)
                .HasColumnName("PRODUCT_CATEGORY");
            entity.Property(e => e.ProductName)
                .HasMaxLength(50)
                .HasColumnName("PRODUCT_NAME");
            entity.Property(e => e.ProductPrice).HasColumnName("PRODUCT_PRICE");
            entity.Property(e => e.ProductStock).HasColumnName("PRODUCT_STOCK");
            entity.Property(e => e.State).HasColumnName("STATE");
        });

        modelBuilder.Entity<ProductsDetail>(entity =>
        {
            entity.HasKey(e => e.IdDetail).HasName("PRIMARY");

            entity.ToTable("products_details");

            entity.HasIndex(e => e.IdAccount, "FK_PRODETAIL_ACCOUNT");

            entity.HasIndex(e => e.IdProduct, "FK_PRODETAIL_PRODUCTS");

            entity.Property(e => e.IdDetail).HasColumnName("ID_DETAIL");
            entity.Property(e => e.IdAccount).HasColumnName("ID_ACCOUNT");
            entity.Property(e => e.IdProduct).HasColumnName("ID_PRODUCT");
            entity.Property(e => e.ProductPrice).HasColumnName("PRODUCT_PRICE");
            entity.Property(e => e.ProductQuantity).HasColumnName("PRODUCT_QUANTITY");
            entity.Property(e => e.State).HasColumnName("STATE");
            entity.Property(e => e.TotalPrice).HasColumnName("TOTAL_PRICE");

            entity.HasOne(d => d.IdAccountNavigation).WithMany(p => p.ProductsDetails)
                .HasForeignKey(d => d.IdAccount)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_PRODETAIL_ACCOUNT");

            entity.HasOne(d => d.IdProductNavigation).WithMany(p => p.ProductsDetails)
                .HasForeignKey(d => d.IdProduct)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_PRODETAIL_PRODUCTS");
        });

        modelBuilder.Entity<Purchase>(entity =>
        {
            entity.HasKey(e => e.IdPurchase).HasName("PRIMARY");

            entity.ToTable("purchases");

            entity.HasIndex(e => e.IdPaymentType, "FK_PAYTYP_PURCHASES");

            entity.HasIndex(e => e.IdPerson, "FK_PURCHASES_PEOPLE");

            entity.HasIndex(e => e.IdCashClosing, "FK_PURCHASE_CASHCLOSING");

            entity.Property(e => e.IdPurchase).HasColumnName("ID_PURCHASE");
            entity.Property(e => e.CreationDate)
                .HasColumnType("datetime")
                .HasColumnName("CREATION_DATE");
            entity.Property(e => e.IdCashClosing).HasColumnName("ID_CASH_CLOSING");
            entity.Property(e => e.IdPaymentType).HasColumnName("ID_PAYMENT_TYPE");
            entity.Property(e => e.IdPerson).HasColumnName("ID_PERSON");
            entity.Property(e => e.PurchaseDescription)
                .HasMaxLength(200)
                .HasColumnName("PURCHASE_DESCRIPTION");
            entity.Property(e => e.State).HasColumnName("STATE");

            entity.HasOne(d => d.IdCashClosingNavigation).WithMany(p => p.Purchases)
                .HasForeignKey(d => d.IdCashClosing)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_PURCHASE_CASHCLOSING");

            entity.HasOne(d => d.IdPaymentTypeNavigation).WithMany(p => p.Purchases)
                .HasForeignKey(d => d.IdPaymentType)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_PAYTYP_PURCHASES");

            entity.HasOne(d => d.IdPersonNavigation).WithMany(p => p.Purchases)
                .HasForeignKey(d => d.IdPerson)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_PURCHASES_PEOPLE");
        });

        modelBuilder.Entity<PurchaseDetail>(entity =>
        {
            entity.HasKey(e => new { e.IdPurchase, e.IdProduct }).HasName("PRIMARY");

            entity.ToTable("purchase_detail");

            entity.HasIndex(e => e.IdProduct, "FK_PURDETAIL_PRODUCTS");

            entity.Property(e => e.IdPurchase).HasColumnName("ID_PURCHASE");
            entity.Property(e => e.IdProduct).HasColumnName("ID_PRODUCT");
            entity.Property(e => e.Quantity).HasColumnName("QUANTITY");
            entity.Property(e => e.TotalPrice).HasColumnName("TOTAL_PRICE");

            entity.HasOne(d => d.IdProductNavigation).WithMany(p => p.PurchaseDetails)
                .HasForeignKey(d => d.IdProduct)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_PURDETAIL_PRODUCTS");

            entity.HasOne(d => d.IdPurchaseNavigation).WithMany(p => p.PurchaseDetails)
                .HasForeignKey(d => d.IdPurchase)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_PURDETAIL_PURCHASE");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.IdRol).HasName("PRIMARY");

            entity.ToTable("roles");

            entity.Property(e => e.IdRol).HasColumnName("ID_ROL");
            entity.Property(e => e.RolName)
                .HasMaxLength(100)
                .HasColumnName("ROL_NAME");
            entity.Property(e => e.State).HasColumnName("STATE");

            entity.HasMany(d => d.IdActions).WithMany(p => p.IdRols)
                .UsingEntity<Dictionary<string, object>>(
                    "ActionsRole",
                    r => r.HasOne<Action>().WithMany()
                        .HasForeignKey("IdAction")
                        .OnDelete(DeleteBehavior.Restrict)
                        .HasConstraintName("FK_ACTIONSROLES_ACTIONS"),
                    l => l.HasOne<Role>().WithMany()
                        .HasForeignKey("IdRol")
                        .OnDelete(DeleteBehavior.Restrict)
                        .HasConstraintName("FK_ACTIONS_ROLES"),
                    j =>
                    {
                        j.HasKey("IdRol", "IdAction").HasName("PRIMARY");
                        j.ToTable("actions_roles");
                        j.HasIndex(new[] { "IdAction" }, "FK_ACTIONSROLES_ACTIONS");
                        j.IndexerProperty<int>("IdRol").HasColumnName("ID_ROL");
                        j.IndexerProperty<int>("IdAction").HasColumnName("ID_ACTION");
                    });
        });

        modelBuilder.Entity<Sale>(entity =>
        {
            entity.HasKey(e => e.IdSale).HasName("PRIMARY");

            entity.ToTable("sales");

            entity.HasIndex(e => e.IdCashClosing, "FK_SALES_CASHCLOSING");

            entity.Property(e => e.IdSale).HasColumnName("ID_SALE");
            entity.Property(e => e.IdCashClosing).HasColumnName("ID_CASH_CLOSING");
            entity.Property(e => e.SaleDate)
                .HasColumnType("datetime")
                .HasColumnName("SALE_DATE");
            entity.Property(e => e.State).HasColumnName("STATE");
            entity.Property(e => e.TotalSale).HasColumnName("TOTAL_SALE");

            entity.HasOne(d => d.IdCashClosingNavigation).WithMany(p => p.Sales)
                .HasForeignKey(d => d.IdCashClosing)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_SALES_CASHCLOSING");
        });

        modelBuilder.Entity<ThirdParty>(entity =>
        {
            entity.HasKey(e => e.IdThirdParty).HasName("PRIMARY");

            entity.ToTable("third_parties");

            entity.HasIndex(e => e.IdPerson, "FK_PEOPLE_THIRDPARTIES");

            entity.Property(e => e.IdThirdParty).HasColumnName("ID_THIRD_PARTY");
            entity.Property(e => e.Address)
                .HasMaxLength(200)
                .HasColumnName("ADDRESS");
            entity.Property(e => e.BusinessName)
                .HasMaxLength(100)
                .HasColumnName("BUSINESS_NAME");
            entity.Property(e => e.Category)
                .HasMaxLength(80)
                .HasColumnName("CATEGORY");
            entity.Property(e => e.CompanyName)
                .HasMaxLength(100)
                .HasColumnName("COMPANY_NAME");
            entity.Property(e => e.IdPerson).HasColumnName("ID_PERSON");
            entity.Property(e => e.Nit)
                .HasMaxLength(20)
                .HasColumnName("NIT");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .HasColumnName("PHONE");
            entity.Property(e => e.ProductServiceName)
                .HasMaxLength(80)
                .HasColumnName("PRODUCT_SERVICE_NAME");
            entity.Property(e => e.State).HasColumnName("STATE");

            entity.HasOne(d => d.IdPersonNavigation).WithMany(p => p.ThirdParties)
                .HasForeignKey(d => d.IdPerson)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_PEOPLE_THIRDPARTIES");
        });

        modelBuilder.Entity<TypesExpense>(entity =>
        {
            entity.HasKey(e => e.IdTypeExpense).HasName("PRIMARY");

            entity.ToTable("types_expenses");

            entity.Property(e => e.IdTypeExpense).HasColumnName("ID_TYPE_EXPENSE");
            entity.Property(e => e.State).HasColumnName("STATE");
            entity.Property(e => e.TypeExpenseName)
                .HasMaxLength(50)
                .HasColumnName("TYPE_EXPENSE_NAME");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.IdUser).HasName("PRIMARY");

            entity.ToTable("users");

            entity.HasIndex(e => e.IdPerson, "FK_USERS_PEOPLE");

            entity.HasIndex(e => e.IdRol, "FK_USERS_ROLES");

            entity.Property(e => e.IdUser).HasColumnName("ID_USER");
            entity.Property(e => e.Email)
                .HasMaxLength(200)
                .HasColumnName("EMAIL");
            entity.Property(e => e.IdPerson).HasColumnName("ID_PERSON");
            entity.Property(e => e.IdRol).HasColumnName("ID_ROL");
            entity.Property(e => e.Password)
                .HasMaxLength(200)
                .HasColumnName("PASSWORD");
            entity.Property(e => e.State).HasColumnName("STATE");

            entity.HasOne(d => d.IdPersonNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.IdPerson)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_USERS_PEOPLE");

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.IdRol)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_USERS_ROLES");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}