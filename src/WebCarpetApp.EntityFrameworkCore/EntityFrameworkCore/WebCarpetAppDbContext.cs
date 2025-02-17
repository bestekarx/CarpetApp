using Microsoft.EntityFrameworkCore;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using WebCarpetApp.Books;
using WebCarpetApp.Models;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.BlobStoring.Database.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.TenantManagement;
using Volo.Abp.TenantManagement.EntityFrameworkCore;

namespace WebCarpetApp.EntityFrameworkCore;

[ReplaceDbContext(typeof(IIdentityDbContext))]
[ReplaceDbContext(typeof(ITenantManagementDbContext))]
[ConnectionStringName("Default")]
public class WebCarpetAppDbContext :
    AbpDbContext<WebCarpetAppDbContext>,
    ITenantManagementDbContext,
    IIdentityDbContext
{
    /* Add DbSet properties for your Aggregate Roots / Entities here. */
    public DbSet<Book> Books { get; set; }
    public DbSet<Area> Areas { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<MessageLog> MessageLogs { get; set; }
    public DbSet<MessageSettings> MessageSettings { get; set; }
    public DbSet<MessageTemplate> MessageTemplates { get; set; }
    public DbSet<MessageUser> MessageUsers { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderedProduct> OrderedProducts { get; set; }
    public DbSet<OrderImage> OrderImages { get; set; }
    public DbSet<Printer> Printers { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Received> Receiveds { get; set; }
    public DbSet<UserTenantMapping> UserTenantMappings { get; set; }
    public DbSet<Vehicle> Vehicles { get; set; }

    #region Entities from the modules

    /* Notice: We only implemented IIdentityProDbContext and ISaasDbContext
     * and replaced them for this DbContext. This allows you to perform JOIN
     * queries for the entities of these modules over the repositories easily. You
     * typically don't need that for other modules. But, if you need, you can
     * implement the DbContext interface of the needed module and use ReplaceDbContext
     * attribute just like IIdentityProDbContext and ISaasDbContext.
     *
     * More info: Replacing a DbContext of a module ensures that the related module
     * uses this DbContext on runtime. Otherwise, it will use its own DbContext class.
     */

    // Identity
    public DbSet<IdentityUser> Users { get; set; }
    public DbSet<IdentityRole> Roles { get; set; }
    public DbSet<IdentityClaimType> ClaimTypes { get; set; }
    public DbSet<OrganizationUnit> OrganizationUnits { get; set; }
    public DbSet<IdentitySecurityLog> SecurityLogs { get; set; }
    public DbSet<IdentityLinkUser> LinkUsers { get; set; }
    public DbSet<IdentityUserDelegation> UserDelegations { get; set; }
    public DbSet<IdentitySession> Sessions { get; set; }

    // Tenant Management
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<TenantConnectionString> TenantConnectionStrings { get; set; }

    #endregion

    public WebCarpetAppDbContext(DbContextOptions<WebCarpetAppDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        /* Include modules to your migration db context */

        builder.ConfigurePermissionManagement();
        builder.ConfigureSettingManagement();
        builder.ConfigureBackgroundJobs();
        builder.ConfigureAuditLogging();
        builder.ConfigureFeatureManagement();
        builder.ConfigureIdentity();
        builder.ConfigureOpenIddict();
        builder.ConfigureTenantManagement();
        builder.ConfigureBlobStoring();

        builder.Entity<Book>(b =>
        {
            b.ToTable(WebCarpetAppConsts.DbTablePrefix + "Books",
                WebCarpetAppConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.Name).IsRequired().HasMaxLength(128);
        });

        builder.Entity<Area>(b =>
        {
            b.ToTable(WebCarpetAppConsts.DbTablePrefix + "Areas",
                WebCarpetAppConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.Name).IsRequired().HasMaxLength(128);
        });

        builder.Entity<Company>(b =>
        {
            b.ToTable(WebCarpetAppConsts.DbTablePrefix + "Companies",
                WebCarpetAppConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.Name).IsRequired().HasMaxLength(128);
            b.Property(x => x.Description).IsRequired().HasMaxLength(500);
            b.Property(x => x.Color).IsRequired().HasMaxLength(50);
        });

        builder.Entity<Customer>(b =>
        {
            b.ToTable(WebCarpetAppConsts.DbTablePrefix + "Customers",
                WebCarpetAppConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.FullName).IsRequired().HasMaxLength(256);
            b.Property(x => x.Phone).IsRequired().HasMaxLength(20);
            b.Property(x => x.CountryCode).IsRequired().HasMaxLength(10);
            b.Property(x => x.Gsm).IsRequired().HasMaxLength(20);
            b.Property(x => x.Address).IsRequired().HasMaxLength(500);
            b.Property(x => x.Coordinate).HasMaxLength(100);
        });

        builder.Entity<Invoice>(b =>
        {
            b.ToTable(WebCarpetAppConsts.DbTablePrefix + "Invoices",
                WebCarpetAppConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.InvoiceNote).HasMaxLength(500);
        });

        builder.Entity<MessageLog>(b =>
        {
            b.ToTable(WebCarpetAppConsts.DbTablePrefix + "MessageLogs",
                WebCarpetAppConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.MessageContent).IsRequired();
            b.Property(x => x.CustomerName).HasMaxLength(256);
            b.Property(x => x.MessagedPhone).HasMaxLength(20);
        });

        builder.Entity<MessageSettings>(b =>
        {
            b.ToTable(WebCarpetAppConsts.DbTablePrefix + "MessageSettings",
                WebCarpetAppConsts.DbSchema);
            b.ConfigureByConvention();
        });

        builder.Entity<MessageTemplate>(b =>
        {
            b.ToTable(WebCarpetAppConsts.DbTablePrefix + "MessageTemplates",
                WebCarpetAppConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.MessageTitle).IsRequired().HasMaxLength(256);
            b.Property(x => x.MessageContent).IsRequired();
        });

        builder.Entity<MessageUser>(b =>
        {
            b.ToTable(WebCarpetAppConsts.DbTablePrefix + "MessageUsers",
                WebCarpetAppConsts.DbSchema);
            b.ConfigureByConvention();
        });

        builder.Entity<Order>(b =>
        {
            b.ToTable(WebCarpetAppConsts.DbTablePrefix + "Orders",
                WebCarpetAppConsts.DbSchema);
            b.ConfigureByConvention();
        });

        builder.Entity<OrderedProduct>(b =>
        {
            b.ToTable(WebCarpetAppConsts.DbTablePrefix + "OrderedProducts",
                WebCarpetAppConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.ProductName).IsRequired().HasMaxLength(256);
        });

        builder.Entity<OrderImage>(b =>
        {
            b.ToTable(WebCarpetAppConsts.DbTablePrefix + "OrderImages",
                WebCarpetAppConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.ImagePath).IsRequired().HasMaxLength(500);
        });

        builder.Entity<Printer>(b =>
        {
            b.ToTable(WebCarpetAppConsts.DbTablePrefix + "Printers",
                WebCarpetAppConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.Name).IsRequired().HasMaxLength(256);
            b.Property(x => x.MacAddress).IsRequired().HasMaxLength(20);
        });

        builder.Entity<Product>(b =>
        {
            b.ToTable(WebCarpetAppConsts.DbTablePrefix + "Products",
                WebCarpetAppConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.Name).IsRequired().HasMaxLength(256);
        });

        builder.Entity<Received>(b =>
        {
            b.ToTable(WebCarpetAppConsts.DbTablePrefix + "Receiveds",
                WebCarpetAppConsts.DbSchema);
            b.ConfigureByConvention();
        });

        builder.Entity<UserTenantMapping>(b =>
        {
            b.ToTable(WebCarpetAppConsts.DbTablePrefix + "UserTenantMappings",
                WebCarpetAppConsts.DbSchema);
            b.ConfigureByConvention();
        });

        builder.Entity<Vehicle>(b =>
        {
            b.ToTable(WebCarpetAppConsts.DbTablePrefix + "Vehicles",
                WebCarpetAppConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.VehicleName).IsRequired().HasMaxLength(256);
            b.Property(x => x.PlateNumber).IsRequired().HasMaxLength(20);
        });
    }
}
