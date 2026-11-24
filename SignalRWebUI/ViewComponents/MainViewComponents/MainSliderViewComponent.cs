using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SignalR.DtoLayer.Dtos.FeatureDtos;

namespace SignalRWebUI.ViewComponents.MainViewComponents
{
    public class MainSliderViewComponent : ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public MainSliderViewComponent(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync("https://localhost:7003/api/Features");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<ResultFeatureDto>>(json);
                return View(values);
            }
            return View();
        }
    }
}
