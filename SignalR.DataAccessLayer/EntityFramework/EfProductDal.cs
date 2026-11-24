using Microsoft.EntityFrameworkCore;
using SignalR.DataAccessLayer.Abstract;
using SignalR.DataAccessLayer.Concrete;
using SignalR.DataAccessLayer.Repositories;
using SignalR.EntityLayer.Entities;

namespace SignalR.DataAccessLayer.EntityFramework
{
    public class EfProductDal : GenericRepository<Product>, IProductDal
    {
        public EfProductDal(SignalrContext context) : base(context)
        {
        }
        public async Task<List<Product>> GetAllProductsWithCategoriesAsync()
        {
            return await _context.Products.Include(x => x.Category).ToListAsync();
        }

        public async Task<decimal> GetAvgProductPrice()
        {
            return await _context.Products.AverageAsync(x => x.Price);
        }

        public async Task<string> GetProductNameByMaxPrice()
        {
            return await _context.Products.Where(x => x.Price == (_context.Products.Max(t => t.Price))).Select(y => y.ProductName).FirstOrDefaultAsync();
        }

        public async Task<string> GetProductNameByMinPrice()
        {
            return await _context.Products.Where(x => x.Price == (_context.Products.Min(t => t.Price))).Select(y => y.ProductName).FirstOrDefaultAsync();
        }

        public async Task<int> GetProductCountAsync()
        {
            return await _context.Products.CountAsync();
        }

        public async Task<int> GetProductCountByCategoryNameDrink()
        {
            return await _context.Products.Include(x => x.Category).Where(y=>y.Category.CategoryName== "İçecekler").CountAsync();
        }

        public async Task<int> GetProductCountByCategoryNameHamburger()
        {
            return await _context.Products.Include(x => x.Category).Where(y => y.Category.CategoryName == "Hamburgerler").CountAsync();
        }

        public async Task<List<Product>> Last9ProductsWithCategoriesAsync()
        {
            return await _context.Products.Include(t=>t.Category).OrderByDescending(x => x.ProductId).Take(9).ToListAsync();
        }
    }
}