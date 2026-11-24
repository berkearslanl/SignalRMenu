using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SignalR.DtoLayer.Dtos.MessageDtos;

namespace SignalRWebUI.ViewComponents.AdminLayoutViewComponents
{
    public class AdminLayoutHeaderMessageViewComponent : ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AdminLayoutHeaderMessageViewComponent(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync("https://localhost:7003/api/Messages");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<ResultMessageDto>>(json);
                return View(values);
            }
            return View();
        }
    }
}
