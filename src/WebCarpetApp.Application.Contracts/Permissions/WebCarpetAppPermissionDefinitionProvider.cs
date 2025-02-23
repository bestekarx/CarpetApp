using WebCarpetApp.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace WebCarpetApp.Permissions;

public class WebCarpetAppPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(WebCarpetAppPermissions.GroupName);

        var booksPermission = myGroup.AddPermission(WebCarpetAppPermissions.Books.Default, L("Permission:Books"));
        booksPermission.AddChild(WebCarpetAppPermissions.Books.Create, L("Permission:Books.Create"));
        booksPermission.AddChild(WebCarpetAppPermissions.Books.Edit, L("Permission:Books.Edit"));
        booksPermission.AddChild(WebCarpetAppPermissions.Books.Delete, L("Permission:Books.Delete"));

        var areasPermission = myGroup.AddPermission(WebCarpetAppPermissions.Areas.Default, L("Permission:Areas"));
        areasPermission.AddChild(WebCarpetAppPermissions.Areas.Create, L("Permission:Areas.Create"));
        areasPermission.AddChild(WebCarpetAppPermissions.Areas.Edit, L("Permission:Areas.Edit"));
        areasPermission.AddChild(WebCarpetAppPermissions.Areas.Delete, L("Permission:Areas.Delete"));

        var companiesPermission = myGroup.AddPermission(WebCarpetAppPermissions.Companies.Default, L("Permission:Companies"));
        companiesPermission.AddChild(WebCarpetAppPermissions.Companies.Create, L("Permission:Companies.Create"));
        companiesPermission.AddChild(WebCarpetAppPermissions.Companies.Edit, L("Permission:Companies.Edit"));
        companiesPermission.AddChild(WebCarpetAppPermissions.Companies.Delete, L("Permission:Companies.Delete"));

        var productsPermission = myGroup.AddPermission(WebCarpetAppPermissions.Products.Default, L("Permission:Products"));
        productsPermission.AddChild(WebCarpetAppPermissions.Products.Create, L("Permission:Products.Create"));
        productsPermission.AddChild(WebCarpetAppPermissions.Products.Edit, L("Permission:Products.Edit"));
        productsPermission.AddChild(WebCarpetAppPermissions.Products.Delete, L("Permission:Products.Delete"));

        var customersPermission = myGroup.AddPermission(WebCarpetAppPermissions.Customers.Default, L("Permission:Customers"));
        customersPermission.AddChild(WebCarpetAppPermissions.Customers.Create, L("Permission:Customers.Create"));
        customersPermission.AddChild(WebCarpetAppPermissions.Customers.Edit, L("Permission:Customers.Edit"));
        customersPermission.AddChild(WebCarpetAppPermissions.Customers.Delete, L("Permission:Customers.Delete"));

        var vehiclesPermission = myGroup.AddPermission(WebCarpetAppPermissions.Vehicles.Default, L("Permission:Vehicles"));
        vehiclesPermission.AddChild(WebCarpetAppPermissions.Vehicles.Create, L("Permission:Vehicles.Create"));
        vehiclesPermission.AddChild(WebCarpetAppPermissions.Vehicles.Edit, L("Permission:Vehicles.Edit"));
        vehiclesPermission.AddChild(WebCarpetAppPermissions.Vehicles.Delete, L("Permission:Vehicles.Delete"));

        var receivedsPermission = myGroup.AddPermission(WebCarpetAppPermissions.Receiveds.Default, L("Permission:Receiveds"));
        receivedsPermission.AddChild(WebCarpetAppPermissions.Receiveds.Create, L("Permission:Receiveds.Create"));
        receivedsPermission.AddChild(WebCarpetAppPermissions.Receiveds.Edit, L("Permission:Receiveds.Edit"));
        receivedsPermission.AddChild(WebCarpetAppPermissions.Receiveds.Delete, L("Permission:Receiveds.Delete"));

        var ordersPermission = myGroup.AddPermission(WebCarpetAppPermissions.Orders.Default, L("Permission:Orders"));
        ordersPermission.AddChild(WebCarpetAppPermissions.Orders.Create, L("Permission:Orders.Create"));
        ordersPermission.AddChild(WebCarpetAppPermissions.Orders.Edit, L("Permission:Orders.Edit"));
        ordersPermission.AddChild(WebCarpetAppPermissions.Orders.Delete, L("Permission:Orders.Delete"));

        var invoicesPermission = myGroup.AddPermission(WebCarpetAppPermissions.Invoices.Default, L("Permission:Invoices"));
        invoicesPermission.AddChild(WebCarpetAppPermissions.Invoices.Create, L("Permission:Invoices.Create"));
        invoicesPermission.AddChild(WebCarpetAppPermissions.Invoices.Edit, L("Permission:Invoices.Edit"));
        invoicesPermission.AddChild(WebCarpetAppPermissions.Invoices.Delete, L("Permission:Invoices.Delete"));

        var messageConfigurationPermission = myGroup.AddPermission(WebCarpetAppPermissions.MessageConfigurations.Default, L("Permission:MessageConfigurations"));
        messageConfigurationPermission.AddChild(WebCarpetAppPermissions.MessageConfigurations.Create, L("Permission:Create"));
        messageConfigurationPermission.AddChild(WebCarpetAppPermissions.MessageConfigurations.Edit, L("Permission:Edit"));
        messageConfigurationPermission.AddChild(WebCarpetAppPermissions.MessageConfigurations.Delete, L("Permission:Delete"));

        var messageUserPermission = myGroup.AddPermission(WebCarpetAppPermissions.MessageUsers.Default, L("Permission:MessageUsers"));
        messageUserPermission.AddChild(WebCarpetAppPermissions.MessageUsers.Create, L("Permission:Create"));
        messageUserPermission.AddChild(WebCarpetAppPermissions.MessageUsers.Edit, L("Permission:Edit"));
        messageUserPermission.AddChild(WebCarpetAppPermissions.MessageUsers.Delete, L("Permission:Delete"));

        var messageTemplatePermission = myGroup.AddPermission(WebCarpetAppPermissions.MessageTemplates.Default, L("Permission:MessageTemplates"));
        messageTemplatePermission.AddChild(WebCarpetAppPermissions.MessageTemplates.Create, L("Permission:Create"));
        messageTemplatePermission.AddChild(WebCarpetAppPermissions.MessageTemplates.Edit, L("Permission:Edit"));
        messageTemplatePermission.AddChild(WebCarpetAppPermissions.MessageTemplates.Delete, L("Permission:Delete"));

        //Define your own permissions here. Example:
        //myGroup.AddPermission(WebCarpetAppPermissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<WebCarpetAppResource>(name);
    }
}
