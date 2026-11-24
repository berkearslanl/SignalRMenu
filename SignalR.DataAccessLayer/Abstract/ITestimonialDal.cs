using SignalR.EntityLayer.Entities;

namespace SignalR.DataAccessLayer.Abstract
{
    public interface ITestimonialDal : IGenericDal<Testimonial>
    {
        Task<int> GetTestimonialCountAsync();
    }
}