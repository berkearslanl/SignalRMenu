using Microsoft.EntityFrameworkCore;
using SignalR.DataAccessLayer.Abstract;
using SignalR.DataAccessLayer.Concrete;
using SignalR.DataAccessLayer.Repositories;
using SignalR.EntityLayer.Entities;

namespace SignalR.DataAccessLayer.EntityFramework
{
    public class EfCategoryDal : GenericRepository<Category>, ICategoryDal
    {
        public EfCategoryDal(SignalrContext context) : base(context)
        {
        }

        public async Task<int> GetActiveCategoryCountAsync()
        {
            var value = await _context.Categories.Where(x => x.Status == true).CountAsync();
            return value;
        }

        public async Task<int> GetCategoryCountAsync()
        {
            var value = await _context.Categories.CountAsync();
            return value;
        }

        public async Task<int> GetPasiveCategoryCountAsync()
        {
            var value = await _context.Categories.Where(x => x.Status == false).CountAsync();
            return value;
        }
    }
}