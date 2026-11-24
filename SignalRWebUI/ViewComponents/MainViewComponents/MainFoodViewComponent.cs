using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SignalR.DtoLayer.Dtos.ProductDtos;

namespace SignalRWebUI.ViewComponents.MainViewComponents
{
    public class MainFoodViewComponent : ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public MainFoodViewComponent(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync("https://localhost:7003/api/Products/Last9Product");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<ResultProductDto>>(json);
                return View(values);
            }
            return View();
        }
    }
}