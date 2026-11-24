using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SignalR.DtoLayer.Dtos.CategoryDtos;

namespace SignalRWebUI.ViewComponents.MainViewComponents
{
    public class MainCategoryViewComponent : ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public MainCategoryViewComponent(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync("https://localhost:7003/api/Categories");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<ResultCategoryDto>>(json);
                return View(values);
            }
            return View();
        }
    }
}
