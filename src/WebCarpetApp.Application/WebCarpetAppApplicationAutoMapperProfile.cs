using AutoMapper;
using WebCarpetApp.Areas;
using WebCarpetApp.Books;
using WebCarpetApp.Companies;
using WebCarpetApp.Companies.Dtos;
using WebCarpetApp.Customers;
using WebCarpetApp.Customers.Dtos;
using WebCarpetApp.Invoices;
using WebCarpetApp.Invoices.Dtos;
using WebCarpetApp.Messaging;
using WebCarpetApp.Messaging.Dtos;
using WebCarpetApp.OrderedProducts.Dtos;
using WebCarpetApp.OrderImages.Dtos;
using WebCarpetApp.Orders;
using WebCarpetApp.Orders.Dtos;
using WebCarpetApp.Printers;
using WebCarpetApp.Printers.Dtos;
using WebCarpetApp.Products;
using WebCarpetApp.Products.Dtos;
using WebCarpetApp.Receiveds;
using WebCarpetApp.Receiveds.Dtos;
using WebCarpetApp.UserTenantMappings.Dtos;
using WebCarpetApp.UserTenants;
using WebCarpetApp.Vehicles;
using WebCarpetApp.Vehicles.Dtos;

namespace WebCarpetApp;

public class WebCarpetAppApplicationAutoMapperProfile : Profile
{
    public WebCarpetAppApplicationAutoMapperProfile()
    {
        CreateMap<Book, BookDto>();
        CreateMap<CreateUpdateBookDto, Book>();

        CreateMap<Area, AreaDto>();
        CreateMap<CreateUpdateAreaDto, Area>();

        CreateMap<Company, CompanyDto>();
        CreateMap<CreateUpdateCompanyDto, Company>();

        CreateMap<Customer, CustomerDto>();
        CreateMap<CreateUpdateCustomerDto, Customer>();

        CreateMap<Invoice, InvoiceDto>();
        CreateMap<CreateUpdateInvoiceDto, Invoice>();

        CreateMap<MessageUser, MessageUserDto>();
        CreateMap<CreateUpdateMessageUserDto, MessageUser>();

        CreateMap<MessageConfiguration, MessageConfigurationDto>();
        CreateMap<CreateUpdateMessageConfigurationDto, MessageConfiguration>();

        CreateMap<MessageTask, MessageTaskDto>();
        CreateMap<CreateUpdateMessageTaskDto, MessageTask>();

        CreateMap<MessageTemplate, MessageTemplateDto>();
        CreateMap<CreateUpdateMessageTemplateDto, MessageTemplate>();

        // Order Mappings - Enhanced
        CreateMap<Order, OrderDto>();
        CreateMap<CreateUpdateOrderDto, Order>()
            .ForMember(dest => dest.OrderStatus, opt => opt.MapFrom(src => src.OrderStatus));
        CreateMap<CreateOrderDto, Order>();

        // OrderedProduct Mappings - Enhanced
        CreateMap<OrderedProduct, OrderedProductDto>();
        CreateMap<CreateUpdateOrderedProductDto, OrderedProduct>();
        CreateMap<OrderedProductDto, OrderedProduct>();

        // OrderImage Mappings - Enhanced
        CreateMap<OrderImage, OrderImageDto>();
        CreateMap<CreateUpdateOrderImageDto, OrderImage>();
        CreateMap<OrderImageDto, OrderImage>();

        CreateMap<Printer, PrinterDto>();
        CreateMap<CreateUpdatePrinterDto, Printer>();

        CreateMap<Product, ProductDto>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.ProductType));
        CreateMap<CreateUpdateProductDto, Product>()
            .ForMember(dest => dest.ProductType, opt => opt.MapFrom(src => src.Type));

        CreateMap<Received, ReceivedDto>();
        CreateMap<CreateUpdateReceivedDto, Received>();

        CreateMap<UserTenantMapping, UserTenantMappingDto>();
        CreateMap<CreateUpdateUserTenantMappingDto, UserTenantMapping>();

        CreateMap<Vehicle, VehicleDto>();
        CreateMap<CreateUpdateVehicleDto, Vehicle>();
    }
}
