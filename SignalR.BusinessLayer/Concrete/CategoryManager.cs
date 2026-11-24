using SignalR.BusinessLayer.Abstract;
using SignalR.DataAccessLayer.Abstract;
using SignalR.EntityLayer.Entities;
using System.Linq.Expressions;

namespace SignalR.BusinessLayer.Concrete
{
    public class CategoryManager : ICategoryService
    {
        private readonly ICategoryDal _categoryDal;

        public CategoryManager(ICategoryDal CategoryDal)
        {
            _categoryDal = CategoryDal;
        }

        public async Task TCreateAsync(Category entity)
        {
            await _categoryDal.CreateAsync(entity);
        }

        public async Task TDeleteAsync(int id)
        {
            await _categoryDal.DeleteAsync(id);
        }

        public async Task<int> TGetActiveCategoryCountAsync()
        {
            return await _categoryDal.GetActiveCategoryCountAsync();
        }

        public async Task<List<Category>> TGetAllAsync()
        {
            return await _categoryDal.GetAllAsync();
        }

        public async Task<List<Category>> TGetAllFilterAsync(Expression<Func<Category, bool>> filter)
        {
            return await _categoryDal.GetAllFilterAsync(filter);
        }

        public async Task<Category?> TGetByIdAsync(int id)
        {
            return await _categoryDal.GetByIdAsync(id);
        }

        public async Task<int> TGetCategoryCountAsync()
        {
            return await _categoryDal.GetCategoryCountAsync();
        }

        public async Task<int> TGetPasiveCategoryCountAsync()
        {
            return await _categoryDal.GetPasiveCategoryCountAsync();
        }

        public async Task TUpdateAsync(Category entity)
        {
            await _categoryDal.UpdateAsync(entity);
        }
    }
}
