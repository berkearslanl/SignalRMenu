using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SignalR.DtoLayer.Dtos.DiscountDtos;

namespace SignalRWebUI.ViewComponents.MainViewComponents
{
    public class MainOfferViewComponent : ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public MainOfferViewComponent(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync("https://localhost:7003/api/Discounts/Last2Discount");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<ResultDiscountDto>>(json);
                return View(values);
            }
            return View();
        }
    }
}
