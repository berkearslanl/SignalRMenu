using SignalR.EntityLayer.Entities;

namespace SignalR.BusinessLayer.Abstract
{
    public interface ICategoryService : IGenericService<Category>
    {
        Task<int> TGetCategoryCountAsync();
        Task<int> TGetActiveCategoryCountAsync();
        Task<int> TGetPasiveCategoryCountAsync();
    }
}