using SignalR.EntityLayer.Entities;

namespace SignalR.DataAccessLayer.Abstract
{
    public interface IProductDal : IGenericDal<Product>
    {
        Task<List<Product>> GetAllProductsWithCategoriesAsync();
        Task<List<Product>> Last9ProductsWithCategoriesAsync();
        Task<int> GetProductCountAsync();
        Task<int> GetProductCountByCategoryNameHamburger();
        Task<int> GetProductCountByCategoryNameDrink();
        Task<decimal> GetAvgProductPrice();
        Task<string> GetProductNameByMaxPrice();
        Task<string> GetProductNameByMinPrice();
    }
}