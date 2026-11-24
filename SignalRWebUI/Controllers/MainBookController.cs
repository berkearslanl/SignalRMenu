using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SignalR.DtoLayer.Dtos.BookingDtos;
using SignalR.DtoLayer.Dtos.Errors;
using System.Text;

namespace SignalRWebUI.Controllers
{
    [AllowAnonymous]
    public class MainBookController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public MainBookController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(CreateBookingDto dto)
        {
            ModelState.Clear();
            var client = _httpClientFactory.CreateClient();
            var json = JsonConvert.SerializeObject(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("https://localhost:7003/api/Bookings", content);
            if (response.IsSuccessStatusCode)
                TempData["BookingSuccess"] = "Rezervasyonunuz alındı.";
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                var errors = JsonConvert.DeserializeObject<List<ApiError>>(errorContent);
                foreach (var err in errors)
                    ModelState.AddModelError(err.PropertyName, err.ErrorMessage);

                return View(dto);
            }
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}