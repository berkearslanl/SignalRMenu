using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SignalR.BusinessLayer.Abstract;
using SignalR.DtoLayer.Dtos.RestaurantTableDtos;
using SignalR.EntityLayer.Entities;
using SignalR.EntityLayer.Enums;
using SignalRApi.Hubs;

namespace SignalRApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantTablesController : ControllerBase
    {
        private readonly IRestaurantTableService _RestaurantTableService;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateRestaurantTableDto> _createValidator;
        private readonly IValidator<UpdateRestaurantTableDto> _updateValidator;
        private readonly IBasketService _basketService;
        private readonly IHubContext<SignalRHub> _hub;

        public RestaurantTablesController(IRestaurantTableService RestaurantTableService, IMapper mapper, IValidator<CreateRestaurantTableDto> createValidator, IValidator<UpdateRestaurantTableDto> updateValidator, IBasketService basketService, IHubContext<SignalRHub> hub)
        {
            _RestaurantTableService = RestaurantTableService;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _basketService = basketService;
            _hub = hub;
        }
        [HttpGet]
        public async Task<IActionResult> RestaurantTableList()
        {
            var values = await _RestaurantTableService.TGetAllAsync();
            var RestaurantTables = _mapper.Map<List<ResultRestaurantTableDto>>(values);
            return Ok(RestaurantTables);
        }
        [HttpGet("RestaurantTableCount")]
        public async Task<IActionResult> RestaurantTableCount()
        {
            var values = await _RestaurantTableService.TGetTableCount();
            return Ok(values);
        }
        [HttpGet("GetRestaurantTableById/{id}")]
        public async Task<IActionResult> GetRestaurantTableById(int id)
        {
            var values = await _RestaurantTableService.TGetByIdAsync(id);
            var RestaurantTables = _mapper.Map<ResultRestaurantTableDto>(values);
            return Ok(RestaurantTables);
        }
        [HttpPost]
        public async Task<IActionResult> CreateRestaurantTable(CreateRestaurantTableDto dto)
        {
            var result = await _createValidator.ValidateAsync(dto);
            if (!result.IsValid)
                return BadRequest(result.Errors.Select(e => e.ErrorMessage));

            var values = _mapper.Map<RestaurantTable>(dto);
            values.Status = SignalR.EntityLayer.Enums.TableStatus.Boş;
            await _RestaurantTableService.TCreateAsync(values);
            return Ok("Ekleme işlemi başarılı.");
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRestaurantTable(int id)
        {
            await _RestaurantTableService.TDeleteAsync(id);
            return Ok("Silme işlemi başarılı.");
        }
        [HttpPut]
        public async Task<IActionResult> UpdateRestaurantTable(UpdateRestaurantTableDto dto)
        {
            var result = await _updateValidator.ValidateAsync(dto);
            if (!result.IsValid)
                return BadRequest(result.Errors.Select(e => e.ErrorMessage));

            var values = _mapper.Map<RestaurantTable>(dto);
            await _RestaurantTableService.TUpdateAsync(values);
            return Ok("Güncelleme işlemi başarılı.");
        }
        [HttpGet("SitTable/{id}")]
        public async Task<IActionResult> SitTable(int id)
        {
            var table = await _RestaurantTableService.TGetByIdAsync(id);
            if (table == null)
                return NotFound("Masa bulunamadı.");
            if (table.Status == TableStatus.Dolu)
                return Conflict("Masa dolu.");

            table.Status = TableStatus.Dolu;
            await _RestaurantTableService.TUpdateAsync(table);
            return Ok();
        }
        [HttpGet("LeaveTable/{id}")]
        public async Task<IActionResult> LeaveTable(int id)
        {
            var table = await _RestaurantTableService.TGetByIdAsync(id);
            if (table == null)
                return NotFound("Masa bulunamadı.");

            await _basketService.TDeleteBasketByTableId(id);

            table.Status = TableStatus.Boş;
            await _RestaurantTableService.TUpdateAsync(table);
            await _hub.Clients.Group($"Table-{id}").SendAsync("ReceiveBasketCount", 0);
            return Ok();
        }
    }
}
