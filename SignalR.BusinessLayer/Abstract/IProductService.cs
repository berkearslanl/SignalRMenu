using SignalR.EntityLayer.Entities;

namespace SignalR.BusinessLayer.Abstract
{
    public interface IProductService : IGenericService<Product>
    {
        Task<List<Product>> TGetAllProductsWithCategoriesAsync();
        Task<List<Product>> TLast9ProductsWithCategoriesAsync();
        Task<int> TGetProductCountAsync();
        Task<int> TGetProductCountByCategoryNameHamburger();
        Task<int> TGetProductCountByCategoryNameDrink();
        Task<decimal> TGetAvgProductPrice();
        Task<string> TGetProductNameByMaxPrice();
        Task<string> TGetProductNameByMinPrice();
    }
}