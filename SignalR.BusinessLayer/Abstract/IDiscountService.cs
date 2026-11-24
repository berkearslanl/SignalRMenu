using SignalR.EntityLayer.Entities;

namespace SignalR.BusinessLayer.Abstract
{
    public interface IDiscountService : IGenericService<Discount>
    {
        Task<int> TGetDiscountCount();
        Task<List<Discount>> TGetLast2DiscountAsync();
    }
}