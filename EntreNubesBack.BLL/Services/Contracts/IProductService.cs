using EntreNubesBack.DTO.Product;

namespace EntreNubesBack.BLL.Services.Contracts;

public interface IProductService
{
    Task<List<ProductDto>> List();
    Task<ProductDto> CreateProduct(CreateProductDto productInfo);
    Task<bool> EditProduct(ProductDto productInfo);
    Task<bool> DeleteProduct(int productId);
}