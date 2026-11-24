using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SignalR.BusinessLayer.Abstract;
using SignalR.DtoLayer.Dtos.MessageDtos;
using SignalR.DtoLayer.Dtos.NotificationDtos;
using SignalR.DtoLayer.Dtos.OrderDtos;
using SignalR.EntityLayer.Entities;

namespace SignalRApi.Hubs
{
    public class SignalRHub : Hub
    {
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        private readonly IRestaurantTableService _restaurantTableService;
        private readonly IOrderService _orderService;
        private readonly IMoneyCaseService _moneyCaseService;
        private readonly IBookingService _bookingService;
        private readonly IDiscountService _discountService;
        private readonly INotificationService _notificationService;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly ITestimonialService _testimonialService;
        private readonly IBasketService _basketService;

        public SignalRHub(ICategoryService categoryService, IProductService productService, IRestaurantTableService restaurantTableService, IOrderService orderService, IMoneyCaseService moneyCaseService, IBookingService bookingService, IDiscountService discountService, IMapper mapper, INotificationService notificationService, UserManager<AppUser> userManager, ITestimonialService testimonialService, IBasketService basketService)
        {
            _categoryService = categoryService;
            _productService = productService;
            _restaurantTableService = restaurantTableService;
            _orderService = orderService;
            _moneyCaseService = moneyCaseService;
            _bookingService = bookingService;
            _discountService = discountService;
            _mapper = mapper;
            _notificationService = notificationService;
            _userManager = userManager;
            _testimonialService = testimonialService;
            _basketService = basketService;
        }

        public async Task SendStatisticsCount()
        {
            var categoryValue = await _categoryService.TGetCategoryCountAsync();
            var productValue = await _productService.TGetProductCountAsync();
            var activeCategoryValue = await _categoryService.TGetActiveCategoryCountAsync();
            var passiveCategoryValue = await _categoryService.TGetPasiveCategoryCountAsync();
            var productCountByDrinkValue = await _productService.TGetProductCountByCategoryNameDrink();
            var productCountByHamburgerValue = await _productService.TGetProductCountByCategoryNameHamburger();
            var avgProductPrice = await _productService.TGetAvgProductPrice();
            var productNameMaxPrice = await _productService.TGetProductNameByMaxPrice();
            var productNameMinPrice = await _productService.TGetProductNameByMinPrice();
            var tableCount = await _restaurantTableService.TGetTableCount();
            var totalOrderCount = await _orderService.TGetTotalOrderCount();
            var lastOrderPrice = await _orderService.TGetLastOrderTotalPrice();
            var todayOrderPrice = await _orderService.TGetTodayTotalPrice();
            var tableMaxOrder = await _orderService.TGetTableNumberByMaxOrder();
            var totalAmountMoneyCase = await _moneyCaseService.TGetTotalAmountMoneyCase();
            var bookingCount = await _bookingService.TGetBookingCountAsync();

            await Clients.All.SendAsync("ReceiveCategoryCount", categoryValue);
            await Clients.All.SendAsync("ReceiveProductCount", productValue);
            await Clients.All.SendAsync("ReceiveActiveCategoryCount", activeCategoryValue);
            await Clients.All.SendAsync("ReceivePassiveCategoryCount", passiveCategoryValue);
            await Clients.All.SendAsync("ReceiveProductCountByDrink", productCountByDrinkValue);
            await Clients.All.SendAsync("ReceiveProductCountByHamburger", productCountByHamburgerValue);
            await Clients.All.SendAsync("ReceiveAvgProductPrice", avgProductPrice.ToString("0.00") + "₺");
            await Clients.All.SendAsync("ReceiveProductNameByMaxPrice", productNameMaxPrice);
            await Clients.All.SendAsync("ReceiveProductNameByMinPrice", productNameMinPrice);
            await Clients.All.SendAsync("ReceiveTableCount", tableCount);
            await Clients.All.SendAsync("ReceiveTotalOrderCount", totalOrderCount);
            await Clients.All.SendAsync("ReceiveLastOrderPrice", lastOrderPrice);
            await Clients.All.SendAsync("ReceiveTodayOrderPrice", todayOrderPrice.ToString("0.00") + "₺");
            await Clients.All.SendAsync("ReceiveTableMaxOrder", tableMaxOrder);
            await Clients.All.SendAsync("ReceiveTotalAmountMoneyCase", totalAmountMoneyCase.ToString("0.00") + "₺");
            await Clients.All.SendAsync("ReceiveBookingCount", bookingCount);
        }
        public async Task GetOrderList()
        {
            var values = await _orderService.TGetAllOrdersWithTableAsync();
            var orders = _mapper.Map<List<GetOrdersWithTableDto>>(values);
            await Clients.All.SendAsync("ReceiveOrderList", orders);
        }
        public async Task GetNotificationList()
        {
            var values = await _notificationService.TGetAllFilterAsync(x => x.Status == false);
            var notifications = _mapper.Map<List<ResultNotificationDto>>(values.OrderByDescending(x => x.AtDate).ToList());
            await Clients.All.SendAsync("ReceiveNotificationList", notifications);
            await Clients.All.SendAsync("ReceiveNotificationCount", notifications.Count);
        }
        public async Task GetTableList()
        {
            var tables = await _restaurantTableService.TGetAllAsync();
            var emptyTables = await _restaurantTableService.TGetAllFilterAsync(x => x.Status == SignalR.EntityLayer.Enums.TableStatus.Boş);
            var fullTables = await _restaurantTableService.TGetAllFilterAsync(x => x.Status == SignalR.EntityLayer.Enums.TableStatus.Dolu);
            await Clients.All.SendAsync("ReceiveTableList", tables);
            await Clients.All.SendAsync("ReceiveTableCount", tables.Count);
            await Clients.All.SendAsync("ReceiveEmptyTableCount", emptyTables.Count);
            await Clients.All.SendAsync("ReceiveFullTableCount", fullTables.Count);
        }
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
        public static int clientCount { get; set; } = 0;
        public override async Task OnConnectedAsync()
        {
            var http = Context.GetHttpContext();
            if (int.TryParse(http?.Request.Query["tableId"], out var tableId) && tableId > 0)
            {
                Context.Items["tableId"] = tableId;
                await Groups.AddToGroupAsync(Context.ConnectionId, $"Table-{tableId}");
            }
            if (http?.Request.Query["type"].ToString() == "customer")
            {
                Context.Items["counted"] = true;
                clientCount++;
                await Clients.All.SendAsync("ReceiveClientCount", clientCount);
            }
            else
            {
                await Clients.Caller.SendAsync("ReceiveClientCount", clientCount);
            }
            await base.OnConnectedAsync();
        }
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            if (Context.Items.ContainsKey("counted"))
            {
                clientCount -= 1;
                await Clients.All.SendAsync("ReceiveClientCount", clientCount);
            }
            await base.OnDisconnectedAsync(exception);
        }
        public async Task DashboardValues()
        {
            var testimonialCount = await _testimonialService.TGetTestimonialCountAsync();
            await Clients.All.SendAsync("ReceiveTestimonialCount",testimonialCount);

            var adminCount = await _userManager.Users.CountAsync();
            await Clients.All.SendAsync("ReceiveAdminCount", adminCount);

            var totalAmountMoneyCase = await _moneyCaseService.TGetTotalAmountMoneyCase();
            await Clients.All.SendAsync("ReceiveTotalAmountMoneyCase", totalAmountMoneyCase.ToString("0.00") + " ₺");

            var activeTableCount = await _restaurantTableService.TGetActiveTableCount();
            await Clients.All.SendAsync("ReceiveActiveTableCount", activeTableCount);

            var discountCount = await _discountService.TGetDiscountCount();
            await Clients.All.SendAsync("ReceiveDiscountCount", discountCount);

            var orders = await _orderService.TGetTotalOrderCount();
            await Clients.All.SendAsync("ReceiveOrderCount", orders);

            var tables = await _restaurantTableService.TGetTableCount();
            await Clients.All.SendAsync("ReceiveMenuTableCount", tables);

            var avgProductPrice = await _productService.TGetAvgProductPrice();
            await Clients.All.SendAsync("ReceiveAvgProductPrice",avgProductPrice);

            var totalProduct = await _productService.TGetProductCountAsync();
            await Clients.All.SendAsync("ReceiveProductCount", totalProduct);

            var activeBooking = await _bookingService.TActiveBookingCountAsync();
            await Clients.All.SendAsync("ReceiveActiveBookingCount", activeBooking);
        }
        public async Task SendMoneyCase()
        {
            var moneys = await _moneyCaseService.TGetTotalAmountMoneyCase();
            await Clients.All.SendAsync("ReceiveMoneyCase", moneys.ToString("N2", new System.Globalization.CultureInfo("tr-TR")) + " ₺");
        }
    }
}