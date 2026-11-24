using System.Linq.Expressions;

namespace SignalR.BusinessLayer.Abstract
{
    public interface IGenericService<T> where T : class
    {
        Task TCreateAsync(T entity);
        Task TUpdateAsync(T entity);
        Task TDeleteAsync(int id);
        Task<T?> TGetByIdAsync(int id);
        Task<List<T>> TGetAllAsync();
        Task<List<T>> TGetAllFilterAsync(Expression<Func<T, bool>> filter);
    }
}