using SignalR.EntityLayer.Entities;

namespace SignalR.DataAccessLayer.Abstract
{
    public interface IDiscountDal : IGenericDal<Discount>
    {
        Task<int> GetDiscountCount();
        Task<List<Discount>> GetLast2DiscountAsync();
    }
}