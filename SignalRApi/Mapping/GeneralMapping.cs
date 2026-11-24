using AutoMapper;
using SignalR.DtoLayer.Dtos.AboutDtos;
using SignalR.DtoLayer.Dtos.BasketDtos;
using SignalR.DtoLayer.Dtos.BookingDtos;
using SignalR.DtoLayer.Dtos.CategoryDtos;
using SignalR.DtoLayer.Dtos.ContactDtos;
using SignalR.DtoLayer.Dtos.DiscountDtos;
using SignalR.DtoLayer.Dtos.FeatureDtos;
using SignalR.DtoLayer.Dtos.MessageDtos;
using SignalR.DtoLayer.Dtos.NotificationDtos;
using SignalR.DtoLayer.Dtos.OrderDetailDtos;
using SignalR.DtoLayer.Dtos.OrderDtos;
using SignalR.DtoLayer.Dtos.ProductDtos;
using SignalR.DtoLayer.Dtos.RestaurantTableDtos;
using SignalR.DtoLayer.Dtos.TestimonialDtos;
using SignalR.EntityLayer.Entities;

namespace SignalRApi.Mapping
{
    public class GeneralMapping : Profile
    {
        public GeneralMapping()
        {
            CreateMap<About, ResultAboutDto>().ReverseMap();
            CreateMap<About, CreateAboutDto>().ReverseMap();
            CreateMap<About, UpdateAboutDto>().ReverseMap();

            CreateMap<Basket, ResultBasketDto>().ReverseMap();
            CreateMap<Basket, CreateBasketDto>().ReverseMap();
            CreateMap<Basket, UpdateBasketDto>().ReverseMap();
            CreateMap<Basket, GetBasketWithProductAndTableDto>()
                .ForMember(x => x.ProductName, t => t.MapFrom(s => s.Product.ProductName))
                .ForMember(x => x.UnitPrice, t => t.MapFrom(s => s.Product.Price))
                .ForMember(x => x.TableName, t => t.MapFrom(s => s.RestaurantTable.TableName));

            CreateMap<Booking, ResultBookingDto>().ReverseMap();
            CreateMap<Booking, CreateBookingDto>().ReverseMap();
            CreateMap<Booking, UpdateBookingDto>().ReverseMap();

            CreateMap<Category, ResultCategoryDto>().ReverseMap();
            CreateMap<Category, CreateCategoryDto>().ReverseMap();
            CreateMap<Category, UpdateCategoryDto>().ReverseMap();

            CreateMap<Contact, ResultContactDto>().ReverseMap();
            CreateMap<Contact, CreateContactDto>().ReverseMap();
            CreateMap<Contact, UpdateContactDto>().ReverseMap();

            CreateMap<Discount, ResultDiscountDto>().ReverseMap();
            CreateMap<Discount, CreateDiscountDto>().ReverseMap();
            CreateMap<Discount, UpdateDiscountDto>().ReverseMap();

            CreateMap<Feature, ResultFeatureDto>().ReverseMap();
            CreateMap<Feature, CreateFeatureDto>().ReverseMap();
            CreateMap<Feature, UpdateFeatureDto>().ReverseMap();

            CreateMap<Notification, ResultNotificationDto>().ReverseMap();
            CreateMap<Notification, CreateNotificationDto>().ReverseMap();
            CreateMap<Notification, UpdateNotificationDto>().ReverseMap();

            CreateMap<Order, ResultOrderDto>().ReverseMap();
            CreateMap<Order, CreateOrderDto>().ReverseMap();
            CreateMap<Order, UpdateOrderDto>().ReverseMap();
            CreateMap<Order, GetOrdersWithTableDto>().ForMember(x => x.TableName, t => t.MapFrom(s => s.RestaurantTable.TableName));

            CreateMap<OrderDetail, ResultOrderDetailDto>().ReverseMap();
            CreateMap<OrderDetail, CreateOrderDetailDto>().ReverseMap();
            CreateMap<OrderDetail, UpdateOrderDetailDto>().ReverseMap();
            CreateMap<OrderDetail, GetOrderDetailsWithProductDto>()
                .ForMember(x => x.ProductName, t => t.MapFrom(s => s.Product.ProductName))
                .ForMember(x => x.TableName, t => t.MapFrom(s => s.Order.RestaurantTable.TableName));

            CreateMap<Product, ResultProductDto>().ReverseMap();
            CreateMap<Product, CreateProductDto>().ReverseMap();
            CreateMap<Product, UpdateProductDto>().ReverseMap();
            CreateMap<Product, GetProductsWithCategoryDto>().ForMember(x => x.CategoryName, t => t.MapFrom(s => s.Category.CategoryName));

            CreateMap<Testimonial, ResultTestimonialDto>().ReverseMap();
            CreateMap<Testimonial, CreateTestimonialDto>().ReverseMap();
            CreateMap<Testimonial, UpdateTestimonialDto>().ReverseMap();

            CreateMap<RestaurantTable, ResultRestaurantTableDto>().ReverseMap();
            CreateMap<RestaurantTable, CreateRestaurantTableDto>().ReverseMap();
            CreateMap<RestaurantTable, UpdateRestaurantTableDto>().ReverseMap();

            CreateMap<Message, ResultMessageDto>().ReverseMap();
            CreateMap<Message, CreateMessageDto>().ReverseMap();
        }
    }
}