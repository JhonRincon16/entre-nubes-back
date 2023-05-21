using AutoMapper;
using EntreNubesBack.BLL.Services.Contracts;
using EntreNubesBack.DAL.Repositories.Contracts;
using EntreNubesBack.DTO.Product;
using EntreNubesBack.Models;

namespace EntreNubesBack.BLL.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public ProductService(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<List<ProductDto>> List()
    {
        var productsList = await _productRepository.Consult(p => p.State);
        return _mapper.Map<List<ProductDto>>(productsList.ToList());
    }

    public async Task<ProductDto> CreateProduct(CreateProductDto productInfo)
    {
        try
        {
            var product = await _productRepository.Get(p => p.ProductName.ToUpper() == productInfo.ProductName.ToUpper());
            if (product != null)
                throw new TaskCanceledException("El nombre del producto ya existe");
            var newProduct = await _productRepository.Create(new Product()
            {
                ProductName = productInfo.ProductName,
                ProductCategory = productInfo.ProductCategory,
                State = true,
                ProductPrice = productInfo.ProductPrice,
                ProductStock = productInfo.ProductStock
            });
            if (newProduct == null)
                throw new TaskCanceledException("Error al crear el producto");
            return _mapper.Map<ProductDto>(newProduct);
        }
        catch
        {
            throw;
        }
    }

    public async Task<bool> EditProduct(ProductDto productInfo)
    {
        try
        {
            var product = await _productRepository.Get(p => p.IdProduct != productInfo.IdProduct && 
                                                            p.ProductName.Trim() == productInfo.ProductName.Trim());
            if (product != null)
                throw new TaskCanceledException("Nombre de producto respetido");
        
            var productToEdit = await _productRepository.Get(p => p.IdProduct == productInfo.IdProduct);
            if (productToEdit == null)
                throw new TaskCanceledException("El producto no existe");
            productToEdit.ProductName = productInfo.ProductName;
            productToEdit.ProductCategory = productInfo.ProductCategory;
            productToEdit.ProductPrice = productInfo.ProductPrice;
            productToEdit.ProductStock = productInfo.ProductStock;
            bool result = await _productRepository.Edit(productToEdit);
            if (!result)
                throw new TaskCanceledException("Error al editar el producto");
            return result;
        }
        catch
        {
            throw;
        }
    }

    public async Task<bool> DeleteProduct(int productId)
    {
        try
        {
            var product = await _productRepository.Get(p => p.IdProduct == productId);
            if (product == null)
                throw new TaskCanceledException("No existe el producto");
            product.State = false;
            bool result = await _productRepository.Edit(product);
            if (!result)
                throw new TaskCanceledException("No se pudo eliminar el producto");
            return result;
        }
        catch
        {
            throw;
        }
    }
}