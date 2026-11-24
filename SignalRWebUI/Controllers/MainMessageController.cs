using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SignalR.DtoLayer.Dtos.Errors;
using SignalR.DtoLayer.Dtos.MessageDtos;
using System.Text;

namespace SignalRWebUI.Controllers
{
    [AllowAnonymous]
    public class MainMessageController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public MainMessageController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(CreateMessageDto dto)
        {
            ModelState.Clear();
            var client = _httpClientFactory.CreateClient();
            var json = JsonConvert.SerializeObject(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("https://localhost:7003/api/Messages", content);
            if (response.IsSuccessStatusCode)
            {
                TempData["MessageSent"] = "Mesajınız başarıyla iletildi. En kısa sürede size dönüş yapacağız.";
                return RedirectToAction("Index");
            }
            var errorContent = await response.Content.ReadAsStringAsync();
            var errors = JsonConvert.DeserializeObject<List<ApiError>>(errorContent);
            foreach (var err in errors)
                ModelState.AddModelError(err.PropertyName, err.ErrorMessage);
            return View(dto);
        }
    }
}