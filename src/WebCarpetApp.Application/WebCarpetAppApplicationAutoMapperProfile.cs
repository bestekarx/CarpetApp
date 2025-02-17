using AutoMapper;
using WebCarpetApp.Areas;
using WebCarpetApp.Books;
using WebCarpetApp.Areas.Dtos;
using WebCarpetApp.Companies;
using WebCarpetApp.Companies.Dtos;
using WebCarpetApp.Customers;
using WebCarpetApp.Customers.Dtos;
using WebCarpetApp.Invoices;
using WebCarpetApp.Invoices.Dtos;
using WebCarpetApp.MessageLogs.Dtos;
using WebCarpetApp.Messages;
using WebCarpetApp.MessageSettings.Dtos;
using WebCarpetApp.MessageTemplates.Dtos;
using WebCarpetApp.MessageUsers.Dtos;
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

        CreateMap<MessageLog, MessageLogDto>();
        CreateMap<CreateUpdateMessageLogDto, MessageLog>();

        CreateMap<Messages.MessageSettings, MessageSettingsDto>();
        CreateMap<CreateUpdateMessageSettingsDto, Messages.MessageSettings>();

        CreateMap<MessageTemplate, MessageTemplateDto>();
        CreateMap<CreateUpdateMessageTemplateDto, MessageTemplate>();

        CreateMap<MessageUser, MessageUserDto>();
        CreateMap<CreateUpdateMessageUserDto, MessageUser>();

        CreateMap<Order, OrderDto>();
        CreateMap<CreateUpdateOrderDto, Order>();

        CreateMap<OrderedProduct, OrderedProductDto>();
        CreateMap<CreateUpdateOrderedProductDto, OrderedProduct>();

        CreateMap<OrderImage, OrderImageDto>();
        CreateMap<CreateUpdateOrderImageDto, OrderImage>();

        CreateMap<Printer, PrinterDto>();
        CreateMap<CreateUpdatePrinterDto, Printer>();

        CreateMap<Product, ProductDto>();
        CreateMap<CreateUpdateProductDto, Product>();

        CreateMap<Received, ReceivedDto>();
        CreateMap<CreateUpdateReceivedDto, Received>();

        CreateMap<UserTenantMapping, UserTenantMappingDto>();
        CreateMap<CreateUpdateUserTenantMappingDto, UserTenantMapping>();

        CreateMap<Vehicle, VehicleDto>();
        CreateMap<CreateUpdateVehicleDto, Vehicle>();
    }
}
