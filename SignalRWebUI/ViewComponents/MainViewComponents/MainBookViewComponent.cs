using Microsoft.AspNetCore.Mvc;
using SignalR.DtoLayer.Dtos.BookingDtos;

namespace SignalRWebUI.ViewComponents.MainViewComponents
{
    public class MainBookViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(new CreateBookingDto());
        }
    }
}
