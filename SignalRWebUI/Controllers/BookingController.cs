using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SignalR.DtoLayer.Dtos.Errors;
using SignalR.DtoLayer.Dtos.BookingDtos;
using System.Text;

namespace SignalRWebUI.Controllers
{
    public class BookingController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public BookingController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync("https://localhost:7003/api/Bookings");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<ResultBookingDto>>(json);
                return View(values);
            }
            return View();
        }
        [HttpGet]
        public IActionResult CreateBooking()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateBooking(CreateBookingDto dto)
        {
            ModelState.Clear();
            var client = _httpClientFactory.CreateClient();
            var json = JsonConvert.SerializeObject(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("https://localhost:7003/api/Bookings", content);
            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index", "Booking");
            var errorContent = await response.Content.ReadAsStringAsync();
            var errors = JsonConvert.DeserializeObject<List<ApiError>>(errorContent);
            foreach (var err in errors)
                ModelState.AddModelError(err.PropertyName, err.ErrorMessage);
            return View(dto);
        }
        [HttpGet]
        public async Task<IActionResult> UpdateBooking(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"https://localhost:7003/api/Bookings/GetBookingById/{id}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<UpdateBookingDto>(json);
                return View(values);
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> UpdateBooking(UpdateBookingDto dto)
        {
            ModelState.Clear();
            var client = _httpClientFactory.CreateClient();
            var json = JsonConvert.SerializeObject(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PutAsync("https://localhost:7003/api/Bookings", content);
            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index", "Booking");
            var errorContent = await response.Content.ReadAsStringAsync();
            var errors = JsonConvert.DeserializeObject<List<ApiError>>(errorContent);
            foreach (var err in errors)
                ModelState.AddModelError(err.PropertyName, err.ErrorMessage);
            return View(dto);
        }
        public async Task<IActionResult> DeleteBooking(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.DeleteAsync($"https://localhost:7003/api/Bookings/{id}");
            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index", "Booking");
            return View();
        }
    }
}
