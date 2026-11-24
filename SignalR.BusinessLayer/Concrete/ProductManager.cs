using SignalR.BusinessLayer.Abstract;
using SignalR.DataAccessLayer.Abstract;
using SignalR.EntityLayer.Entities;
using System.Linq.Expressions;

namespace SignalR.BusinessLayer.Concrete
{
    public class ProductManager : IProductService
    {
        private readonly IProductDal _productDal;

        public ProductManager(IProductDal ProductDal)
        {
            _productDal = ProductDal;
        }

        public async Task<List<Product>> TGetAllProductsWithCategoriesAsync()
        {
            return await _productDal.GetAllProductsWithCategoriesAsync();
        }

        public async Task TCreateAsync(Product entity)
        {
            await _productDal.CreateAsync(entity);
        }

        public async Task TDeleteAsync(int id)
        {
            await _productDal.DeleteAsync(id);
        }

        public async Task<List<Product>> TGetAllAsync()
        {
            return await _productDal.GetAllAsync();
        }

        public async Task<List<Product>> TGetAllFilterAsync(Expression<Func<Product, bool>> filter)
        {
            return await _productDal.GetAllFilterAsync(filter);
        }

        public async Task<Product?> TGetByIdAsync(int id)
        {
            return await _productDal.GetByIdAsync(id);
        }

        public async Task TUpdateAsync(Product entity)
        {
            await _productDal.UpdateAsync(entity);
        }

        public async Task<int> TGetProductCountAsync()
        {
            return await _productDal.GetProductCountAsync();
        }

        public async Task<int> TGetProductCountByCategoryNameHamburger()
        {
            return await _productDal.GetProductCountByCategoryNameHamburger();
        }

        public async Task<int> TGetProductCountByCategoryNameDrink()
        {
            return await _productDal.GetProductCountByCategoryNameDrink();
        }

        public async Task<decimal> TGetAvgProductPrice()
        {
            return await _productDal.GetAvgProductPrice();
        }

        public async Task<string> TGetProductNameByMaxPrice()
        {
            return await _productDal.GetProductNameByMaxPrice();
        }

        public async Task<string> TGetProductNameByMinPrice()
        {
            return await _productDal.GetProductNameByMinPrice();
        }

        public async Task<List<Product>> TLast9ProductsWithCategoriesAsync()
        {
            return await _productDal.Last9ProductsWithCategoriesAsync();
        }
    }
}