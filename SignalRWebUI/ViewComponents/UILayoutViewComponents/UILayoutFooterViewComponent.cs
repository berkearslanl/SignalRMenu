using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SignalR.DtoLayer.Dtos.ContactDtos;

namespace SignalRWebUI.ViewComponents.UILayoutViewComponents
{
    public class UILayoutFooterViewComponent : ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public UILayoutFooterViewComponent(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync("https://localhost:7003/api/Contacts");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<ResultContactDto>>(json);
                return View(values);
            }
            return View();
        }
    }
}