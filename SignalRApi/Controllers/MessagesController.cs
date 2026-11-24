using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SignalR.BusinessLayer.Abstract;
using SignalR.DtoLayer.Dtos.MessageDtos;
using SignalR.EntityLayer.Entities;
using SignalRApi.Hubs;

namespace SignalRApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageService _messageService;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateMessageDto> _createValidator;
        private readonly IHubContext<SignalRHub> _hub;

        public MessagesController(IMessageService MessageService, IMapper mapper, IHubContext<SignalRHub> hub, IValidator<CreateMessageDto> createValidator)
        {
            _messageService = MessageService;
            _mapper = mapper;
            _createValidator = createValidator;
            _hub = hub;
        }
        [HttpGet]
        public async Task<IActionResult> MessageList()
        {
            var values = await _messageService.TGetAllAsync();
            var Messages = _mapper.Map<List<ResultMessageDto>>(values.OrderByDescending(x => x.SendDate).ToList());
            return Ok(Messages);
        }
        [HttpGet("GetMessageById/{id}")]
        public async Task<IActionResult> GetMessageById(int id)
        {
            var values = await _messageService.TGetByIdAsync(id);
            var Messages = _mapper.Map<ResultMessageDto>(values);
            return Ok(Messages);
        }
        [HttpPost]
        public async Task<IActionResult> CreateMessage(CreateMessageDto dto)
        {
            var result = await _createValidator.ValidateAsync(dto);
            if (!result.IsValid)
                return BadRequest(result.Errors.Select(e => e.ErrorMessage));

            var values = _mapper.Map<Message>(dto);
            values.SendDate = DateTime.Now;
            await _messageService.TCreateAsync(values);
            await GetMessageListAsync();
            return Ok("Ekleme işlemi başarılı.");
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMessage(int id)
        {
            await _messageService.TDeleteAsync(id);
            await GetMessageListAsync();
            return Ok("Silme işlemi başarılı.");
        }
        private async Task GetMessageListAsync()
        {
            var values = await _messageService.TGetAllAsync();
            var messages = _mapper.Map<List<ResultMessageDto>>(values.OrderByDescending(x=>x.SendDate).ToList());
            await _hub.Clients.All.SendAsync("ReceiveMessageList", messages);
            await _hub.Clients.All.SendAsync("ReceiveMessageCount", messages.Count);
        }
    }
}