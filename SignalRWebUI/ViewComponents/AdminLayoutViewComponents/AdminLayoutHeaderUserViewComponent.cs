using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SignalR.DtoLayer.Dtos.SettingDtos;

namespace SignalRWebUI.ViewComponents.AdminLayoutViewComponents
{
    public class AdminLayoutHeaderUserViewComponent : ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AdminLayoutHeaderUserViewComponent(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"https://localhost:7003/api/Settings/{User.Identity.Name}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<ResultUserInfoDto>(json);
                return View(values);
            }
            return View();
        }
    }
}
