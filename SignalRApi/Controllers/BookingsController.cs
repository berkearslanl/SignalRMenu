using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SignalR.BusinessLayer.Abstract;
using SignalR.DtoLayer.Dtos.BookingDtos;
using SignalR.DtoLayer.Dtos.NotificationDtos;
using SignalR.EntityLayer.Entities;
using SignalRApi.Hubs;

namespace SignalRApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        private readonly IMapper _mapper;
        private readonly IHubContext<SignalRHub> _hub;
        private readonly INotificationService _notificationService;
        private readonly IValidator<CreateBookingDto> _createValidator;
        private readonly IValidator<UpdateBookingDto> _updateValidator;

        public BookingsController(IBookingService BookingService, IMapper mapper, IHubContext<SignalRHub> hub, INotificationService notificationService, IValidator<CreateBookingDto> createValidator, IValidator<UpdateBookingDto> updateValidator)
        {
            _bookingService = BookingService;
            _mapper = mapper;
            _hub = hub;
            _notificationService = notificationService;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }
        [HttpGet]
        public async Task<IActionResult> BookingList()
        {
            var values = await _bookingService.TGetAllAsync();
            var Bookings = _mapper.Map<List<ResultBookingDto>>(values);
            return Ok(Bookings);
        }
        [HttpGet("BookingCount")]
        public async Task<IActionResult> BookingCount()
        {
            var values = await _bookingService.TGetBookingCountAsync();
            return Ok(values);
        }
        [HttpGet("GetBookingById/{id}")]
        public async Task<IActionResult> GetBookingById(int id)
        {
            var values = await _bookingService.TGetByIdAsync(id);
            var Bookings = _mapper.Map<ResultBookingDto>(values);
            return Ok(Bookings);
        }
        [HttpPost]
        public async Task<IActionResult> CreateBooking(CreateBookingDto dto)
        {
            var result = await _createValidator.ValidateAsync(dto);
            if (!result.IsValid)
                return BadRequest(result.Errors.Select(e => new {e.PropertyName,e.ErrorMessage}));

            var values = _mapper.Map<Booking>(dto);
            values.CreatedAt = DateTime.Now;
            await _bookingService.TCreateAsync(values);
            await GetBookingListAsync();

            var notification = new Notification
            {
                AtDate = DateTime.Now,
                Type = "Rezervasyon",
                Status = false,
                Description = $"{dto.Name} adına {dto.PersonCount} kişilik yeni rezervasyon: {dto.BookDate:dd.MM.yyyy HH:mm}"
            };
            await _notificationService.TCreateAsync(notification);
            await GetNotificationListAsync();

            return Ok("Ekleme işlemi başarılı.");
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            await _bookingService.TDeleteAsync(id);
            await GetBookingListAsync();
            return Ok("Silme işlemi başarılı.");
        }
        [HttpPut]
        public async Task<IActionResult> UpdateBooking(UpdateBookingDto dto)
        {
            var result = await _updateValidator.ValidateAsync(dto);
            if (!result.IsValid)
                return BadRequest(result.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }));

            var values = _mapper.Map<Booking>(dto);
            await _bookingService.TUpdateAsync(values);
            await GetBookingListAsync();
            return Ok("Güncelleme işlemi başarılı.");
        }
        //listeyi gönderen yardımcı metot
        private async Task GetBookingListAsync()
        {
            var values = await _bookingService.TGetAllAsync();
            var bookings = _mapper.Map<List<ResultBookingDto>>(values);
            await _hub.Clients.All.SendAsync("ReceiveBookingList", bookings);
        }
        private async Task GetNotificationListAsync()
        {
            var values = await _notificationService.TGetAllFilterAsync(x => x.Status == false);
            var notifications = _mapper.Map<List<ResultNotificationDto>>(values.OrderByDescending(x => x.AtDate).ToList());
            await _hub.Clients.All.SendAsync("ReceiveNotificationList", notifications);
            await _hub.Clients.All.SendAsync("ReceiveNotificationCount", notifications.Count);
        }
    }
}