using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SignalR.DtoLayer.Dtos.TestimonialDtos;

namespace SignalRWebUI.ViewComponents.MainViewComponents
{
    public class MainClientViewComponent : ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public MainClientViewComponent(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync("https://localhost:7003/api/Testimonials");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<ResultTestimonialDto>>(json);
                return View(values);
            }
            return View();
        }
    }
}
