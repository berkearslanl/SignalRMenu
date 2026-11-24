using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SignalR.BusinessLayer.Abstract;
using SignalR.DtoLayer.Dtos.NotificationDtos;
using SignalR.EntityLayer.Entities;
using SignalRApi.Hubs;

namespace SignalRApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateNotificationDto> _createValidator;
        private readonly IValidator<UpdateNotificationDto> _updateValidator;
        private readonly IHubContext<SignalRHub> _hub;

        public NotificationsController(INotificationService NotificationService, IMapper mapper, IHubContext<SignalRHub> hub, IValidator<CreateNotificationDto> createValidator, IValidator<UpdateNotificationDto> updateValidator)
        {
            _notificationService = NotificationService;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _hub = hub;
        }
        [HttpGet]
        public async Task<IActionResult> NotificationList()
        {
            var values = await _notificationService.TGetAllAsync();
            var notifications = _mapper.Map<List<ResultNotificationDto>>(values.OrderByDescending(x=>x.NotificationId));
            return Ok(notifications);
        }
        [HttpGet("GetNotificationByStatusFalse")]
        public async Task<IActionResult> GetNotificationByStatusFalse()
        {
            var values = await _notificationService.TGetAllFilterAsync(x => x.Status == false);
            var notifications = _mapper.Map<List<ResultNotificationDto>>(values.OrderByDescending(x=>x.AtDate).ToList());
            return Ok(notifications);
        }
        [HttpGet("NotificationCountByStatusFalse")]
        public async Task<IActionResult> NotificationCountByStatusFalse()
        {
            var values = await _notificationService.TGetNotificationCountByStatusFalse();
            return Ok(values);
        }
        [HttpGet("GetNotificationById/{id}")]
        public async Task<IActionResult> GetNotificationById(int id)
        {
            var values = await _notificationService.TGetByIdAsync(id);
            var notifications = _mapper.Map<ResultNotificationDto>(values);
            return Ok(notifications);
        }
        [HttpPost]
        public async Task<IActionResult> CreateNotification(CreateNotificationDto dto)
        {
            var result = await _createValidator.ValidateAsync(dto);
            if (!result.IsValid)
                return BadRequest(result.Errors.Select(e => e.ErrorMessage));

            var values = _mapper.Map<Notification>(dto);
            await _notificationService.TCreateAsync(values);
            await GetNotificationListAsync();
            return Ok("Ekleme işlemi başarılı.");
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNotification(int id)
        {
            await _notificationService.TDeleteAsync(id);
            await GetNotificationListAsync();
            return Ok("Silme işlemi başarılı.");
        }
        [HttpPut]
        public async Task<IActionResult> UpdateNotification(UpdateNotificationDto dto)
        {
            var result = await _updateValidator.ValidateAsync(dto);
            if (!result.IsValid)
                return BadRequest(result.Errors.Select(e => e.ErrorMessage));

            var values = _mapper.Map<Notification>(dto);
            await _notificationService.TUpdateAsync(values);
            await GetNotificationListAsync();
            return Ok("Güncelleme işlemi başarılı.");
        }
        [HttpGet("ChangeNotificationStatus/{id}")]
        public async Task<IActionResult> ChangeNotificationStatus(int id)
        {
            await _notificationService.TChangeNotificationStatus(id);
            await GetNotificationListAsync();
            return Ok("Durum başarıyla değiştirildi.");
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