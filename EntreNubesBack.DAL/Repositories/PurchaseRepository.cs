using EntreNubesBack.DAL.DBContext;
using EntreNubesBack.DAL.Repositories.Contracts;
using EntreNubesBack.DTO.Purchase;
using EntreNubesBack.Models;
using Microsoft.EntityFrameworkCore;

namespace EntreNubesBack.DAL.Repositories;

public class PurchaseRepository : GenericRepository<Purchase>, IPurchaseRepository
{
    private readonly IProductRepository _productRepository;
    
    public PurchaseRepository(EntrenubesContext dbContext, IProductRepository productRepository) : base(dbContext)
    {
        _productRepository = productRepository;
    }


    public async Task<Purchase> CreatePurchase(CreatePurchaseDto info, int personId, int cashClosing)
    {
        using (var transaction = _dbContext.Database.BeginTransaction())
        {
            try
            {
                var purchase = await base.Create(new Purchase()
                {
                    State = true, 
                    CreationDate = DateTime.Now,
                    IdPerson = personId,
                    PurchaseDescription = info.Description,
                    IdPaymentType = info.PaymentTypeId
                });
                info.products.ForEach(async (p) =>
                {
                    purchase.PurchaseDetails.Add(new PurchaseDetail()
                    {
                        IdPurchase = purchase.IdPurchase,
                        IdProduct = p.ProductId,
                        Quantity = p.TotalUnits,
                        TotalPrice = p.UnitPrice * p.TotalUnits
                    });
                    var product = await _productRepository.Get(pro => pro.IdProduct == p.ProductId);
                    product.ProductStock += p.TotalUnits;
                    bool respose = await _productRepository.Edit(product);
                    if (!respose)
                        throw new TaskCanceledException("Error al agregar las unidades al producto");
                });
                transaction.Commit();
                return purchase;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
    }
    
    public async Task<bool> DeletePurchase(int purchaseId)
    {
        using (var transaction = _dbContext.Database.BeginTransaction())
        {
            try
            {
                var purchase = await Consult(p => p.IdPurchase == purchaseId && p.State);
                var purchaseDetails = purchase.Include(p => p.PurchaseDetails).FirstOrDefault();
                if (purchaseDetails != null)
                {
                    foreach (var detail in purchaseDetails.PurchaseDetails)
                    {
                        var product = await _productRepository.Get(p => p.State && p.IdProduct == detail.IdProduct);
                        if (product != null)
                        {
                            product.ProductStock -= detail.Quantity;
                            await _productRepository.Edit(product);
                        }
                    }
                    purchaseDetails.State = false;
                    bool response = await Edit(purchaseDetails);
                    transaction.Commit();
                    return response;
                }
                return false;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}