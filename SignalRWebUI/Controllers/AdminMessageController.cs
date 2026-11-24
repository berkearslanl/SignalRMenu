using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SignalR.DtoLayer.Dtos.MessageDtos;

namespace SignalRWebUI.Controllers
{
    public class AdminMessageController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AdminMessageController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
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
        public async Task<IActionResult> DeleteMessage(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.DeleteAsync($"https://localhost:7003/api/Messages/{id}");
            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index", "AdminMessage");
            return View();
        }
    }
}
