using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SignalR.DtoLayer.Dtos.AboutDtos;

namespace SignalRWebUI.ViewComponents.MainViewComponents
{
    public class MainAboutViewComponent : ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public MainAboutViewComponent(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync("https://localhost:7003/api/Abouts");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<ResultAboutDto>>(json);
                return View(values);
            }
            return View();
        }
    }
}
