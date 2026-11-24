using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SignalR.DtoLayer.Dtos.NotificationDtos;

namespace SignalRWebUI.ViewComponents.AdminLayoutViewComponents
{
    public class AdminLayoutHeaderNotificationViewComponent : ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AdminLayoutHeaderNotificationViewComponent(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync("https://localhost:7003/api/Notifications/GetNotificationByStatusFalse");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var notifications = JsonConvert.DeserializeObject<List<ResultNotificationDto>>(json);
                return View(notifications);
            }
            return View();
        }
    }
}
