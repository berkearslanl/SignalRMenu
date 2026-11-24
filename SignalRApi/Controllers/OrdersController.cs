using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SignalR.BusinessLayer.Abstract;
using SignalR.DtoLayer.Dtos.NotificationDtos;
using SignalR.DtoLayer.Dtos.OrderDtos;
using SignalR.EntityLayer.Entities;
using SignalR.EntityLayer.Enums;
using SignalRApi.Hubs;

namespace SignalRApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateOrderDto> _createValidator;
        private readonly IValidator<UpdateOrderDto> _updateValidator;
        private readonly INotificationService _notificationService;
        private readonly IHubContext<SignalRHub> _hub;
        private readonly IBasketService _basketService;
        private readonly IRestaurantTableService _tableService;

        public OrdersController(IOrderService OrderService, IMapper mapper, INotificationService notificationService, IHubContext<SignalRHub> hub, IValidator<CreateOrderDto> createValidator, IValidator<UpdateOrderDto> updateValidator, IBasketService basketService, IRestaurantTableService tableService)
        {
            _orderService = OrderService;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _notificationService = notificationService;
            _hub = hub;
            _basketService = basketService;
            _tableService = tableService;
        }
        [HttpGet]
        public async Task<IActionResult> OrderList()
        {
            var values = await _orderService.TGetAllAsync();
            var Orders = _mapper.Map<List<ResultOrderDto>>(values);
            return Ok(Orders);
        }
        [HttpGet("GetTotalOrder")]
        public async Task<IActionResult> GetTotalOrder()
        {
            var values = await _orderService.TGetTotalOrderCount();
            return Ok(values);
        }
        [HttpGet("GetTodayTotalPrice")]
        public async Task<IActionResult> GetTodayTotalPrice()
        {
            var values = await _orderService.TGetTodayTotalPrice();
            return Ok(values);
        }
        [HttpGet("LastOrderPrice")]
        public async Task<IActionResult> LastOrderPrice()
        {
            var values = await _orderService.TGetLastOrderTotalPrice();
            return Ok(values);
        }
        [HttpGet("GetTableNumberByMaxOrder")]
        public async Task<IActionResult> GetTableNumberByMaxOrder()
        {
            var values = await _orderService.TGetTableNumberByMaxOrder();
            return Ok(values);
        }
        [HttpGet("GetOrdersWithTable")]
        public async Task<IActionResult> GetOrdersWithTable()
        {
            var values = await _orderService.TGetAllOrdersWithTableAsync();
            var Orders = _mapper.Map<List<GetOrdersWithTableDto>>(values);
            return Ok(Orders);
        }
        [HttpGet("GetOrderById/{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var values = await _orderService.TGetByIdAsync(id);
            var Orders = _mapper.Map<ResultOrderDto>(values);
            return Ok(Orders);
        }
        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreateOrderDto dto)
        {
            var result = await _createValidator.ValidateAsync(dto);
            if (!result.IsValid)
                return BadRequest(result.Errors.Select(e => e.ErrorMessage));

            var values = _mapper.Map<Order>(dto);
            values.OrderDate = DateTime.Now;
            await _orderService.TCreateAsync(values);

            var notification = new Notification
            {
                Type = "Sipariş",
                Status = false,
                AtDate = DateTime.Now,
                Description = $"Masa {dto.RestaurantTableId} için yeni sipariş alındı. Tutar: {dto.TotalPrice:N2} ₺"
            };
            await _notificationService.TCreateAsync(notification);
            await GetNotificationListAsync();

            return Ok("Ekleme işlemi başarılı.");
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            await _orderService.TDeleteAsync(id);
            return Ok("Silme işlemi başarılı.");
        }
        [HttpPut]
        public async Task<IActionResult> UpdateOrder(UpdateOrderDto dto)
        {
            var result = await _updateValidator.ValidateAsync(dto);
            if (!result.IsValid)
                return BadRequest(result.Errors.Select(e => e.ErrorMessage));

            var values = _mapper.Map<Order>(dto);
            await _orderService.TUpdateAsync(values);
            return Ok("Güncelleme işlemi başarılı.");
        }
        [HttpPost("CompleteOrder/{tableId}")]
        public async Task<IActionResult> CompleteOrder(int tableId)
        {
            var table = await _tableService.TGetByIdAsync(tableId);
            if (table == null)
                return NotFound("Masa bulunamadı.");
            if (table.Status != TableStatus.Dolu)
                return Conflict("Bu masa için aktif oturum yok. Lütfen masaya oturun.");

            var basketItems = await _basketService.TGetBasketByTable(tableId);
            if (basketItems == null || basketItems.Count == 0)
                return BadRequest("Sepetiniz boş.");

            var details = basketItems.Select(b => new OrderDetail
            {
                ProductId = b.ProductId,
                Count = b.Count,
                UnitPrice = b.Product.Price,
                TotalPrice = b.Product.Price * b.Count
            }).ToList();

            var subTotal = details.Sum(d => d.TotalPrice);
            var grandTotal = Math.Round(subTotal * 1.20m, 2);

            var order = new Order
            {
                RestaurantTableId = tableId,
                OrderDate = DateTime.Now,
                Description = "Sepetten verilen sipariş",
                TotalPrice = 0,
                OrderDetails = details
            };

            await _orderService.TCreateAsync(order);
            await _basketService.TDeleteBasketByTableId(tableId);
            var notification = new Notification
            {
                Type = "Sipariş",
                Status = false,
                AtDate = DateTime.Now,
                Description = $"Masa {tableId} için yeni sipariş alındı. Tutar: {grandTotal:N2} ₺"
            };
            await _notificationService.TCreateAsync(notification);
            await GetNotificationListAsync();
            await _hub.Clients.Group($"Table-{tableId}").SendAsync("ReceiveBasketCount", 0);
            return Ok(new { orderId = order.OrderID, totalPrice = grandTotal });
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
