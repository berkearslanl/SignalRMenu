using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SignalR.DtoLayer.Dtos.Errors;
using SignalR.DtoLayer.Dtos.TestimonialDtos;
using System.Text;

namespace SignalRWebUI.Controllers
{
    public class TestimonialController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public TestimonialController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
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
        [HttpGet]
        public IActionResult CreateTestimonial()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateTestimonial(CreateTestimonialDto dto)
        {
            ModelState.Clear();
            dto.Status = true;
            var client = _httpClientFactory.CreateClient();
            var json = JsonConvert.SerializeObject(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("https://localhost:7003/api/Testimonials", content);
            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index", "Testimonial");
            var errorContent = await response.Content.ReadAsStringAsync();
            var errors = JsonConvert.DeserializeObject<List<ApiError>>(errorContent);
            foreach (var err in errors)
                ModelState.AddModelError(err.PropertyName, err.ErrorMessage);
            return View(dto);
        }
        [HttpGet]
        public async Task<IActionResult> UpdateTestimonial(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"https://localhost:7003/api/Testimonials/GetTestimonialById/{id}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<UpdateTestimonialDto>(json);
                return View(values);
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> UpdateTestimonial(UpdateTestimonialDto dto)
        {
            ModelState.Clear();
            var client = _httpClientFactory.CreateClient();
            var json = JsonConvert.SerializeObject(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PutAsync("https://localhost:7003/api/Testimonials", content);
            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index", "Testimonial");
            var errorContent = await response.Content.ReadAsStringAsync();
            var errors = JsonConvert.DeserializeObject<List<ApiError>>(errorContent);
            foreach (var err in errors)
                ModelState.AddModelError(err.PropertyName, err.ErrorMessage);
            return View(dto);
        }
        public async Task<IActionResult> DeleteTestimonial(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.DeleteAsync($"https://localhost:7003/api/Testimonials/{id}");
            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index", "Testimonial");
            return View();
        }
        public async Task<IActionResult> ChangeTestimonialStatus(int id)
        {
            var client = _httpClientFactory.CreateClient();
            await client.GetAsync($"https://localhost:7003/api/Testimonials/ChangeStatus/{id}");
            return RedirectToAction("Index", "Testimonial");
        }
    }
}
