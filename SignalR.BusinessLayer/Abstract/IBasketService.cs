using SignalR.EntityLayer.Entities;

namespace SignalR.BusinessLayer.Abstract
{
    public interface IBasketService : IGenericService<Basket>
    {
        Task<List<Basket>> TGetBasketByTable(int id);
        Task TDeleteBasketByTableId(int id);
        Task<int> TGetBasketCountByCurrentTable(int id);
    }
}