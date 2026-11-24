using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SignalR.DtoLayer.Dtos.BasketDtos;

namespace SignalRWebUI.ViewComponents.UILayoutViewComponents
{
    public class UILayoutHeaderViewComponent : ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public UILayoutHeaderViewComponent(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            int count = 0;
            if (int.TryParse(Request.Cookies["TableId"], out var tableId))
            {
                try
                {
                    var client = _httpClientFactory.CreateClient();
                    var response = await client.GetAsync($"https://localhost:7003/api/Baskets/{tableId}");
                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        var items = JsonConvert.DeserializeObject<List<GetBasketWithProductAndTableDto>>(json);
                        count = items?.Sum(x => x.Count) ?? 0;
                    }
                }
                catch { }
            }
            return View(count);
        }
    }
}
