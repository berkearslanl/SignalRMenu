using SignalR.EntityLayer.Entities;

namespace SignalR.DataAccessLayer.Abstract
{
    public interface IBasketDal : IGenericDal<Basket>
    {
        Task<List<Basket>> GetBasketByTable(int id);
        Task DeleteBasketByTableId(int id);
        Task<int> GetBasketCountByCurrentTable(int id);
    }
}