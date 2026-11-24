using SignalR.EntityLayer.Entities;

namespace SignalR.DataAccessLayer.Abstract
{
    public interface ICategoryDal : IGenericDal<Category>
    {
        Task<int> GetCategoryCountAsync();
        Task<int> GetActiveCategoryCountAsync();
        Task<int> GetPasiveCategoryCountAsync();
    }
}