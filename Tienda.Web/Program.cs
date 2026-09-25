using Libreria.Web.Middleware;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Events;
using System.Globalization;
using System.Text;
using Tienda.Application.Profiles;
using Tienda.Application.Services.Implementations;
using Tienda.Application.Services.Interfaces;
using Tienda.Infraestructure.Data;
using Tienda.Infraestructure.Repository.Implementations;
using Tienda.Infraestructure.Repository.Interfaces;
using Tienda.Web.Services;

var builder = WebApplication.CreateBuilder(args);
var connString = builder.Configuration.GetConnectionString("SqlServerDataBase");

// Add services to the container.
builder.Services.AddControllersWithViews();
// Imágenes de productos de hasta 5 MB cada una
builder.Services.Configure<Microsoft.AspNetCore.Http.Features.FormOptions>(o => o.MultipartBodyLengthLimit = 50 * 1024 * 1024);
builder.WebHost.ConfigureKestrel(o => o.Limits.MaxRequestBodySize = 50 * 1024 * 1024);

// Detrás del proxy del hosting (HTTPS termina en el proxy)
builder.Services.Configure<ForwardedHeadersOptions>(o =>
{
    o.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    o.KnownNetworks.Clear();
    o.KnownProxies.Clear();
});

// Configurar Inyección de Dependencias (D.I.)
// Repositorios
builder.Services.AddTransient<IRepositoryCategoria, RepositoryCategoria>();
builder.Services.AddTransient<IRepositoryProducto, RepositoryProducto>();
builder.Services.AddTransient<IRepositoryImagenProducto, RepositoryImagenProducto>();
builder.Services.AddTransient<IRepositoryEtiqueta, RepositoryEtiqueta>();
builder.Services.AddTransient<IRepositoryEtiquetaProducto, RepositoryEtiquetaProducto>();
builder.Services.AddTransient<IRepositoryResena, RepositoryResena>();
builder.Services.AddTransient<IRepositoryUsuario, RepositoryUsuario>();
builder.Services.AddTransient<IRepositoryPromocion, RepositoryPromoción>();
builder.Services.AddTransient<IRepositoryPromocionCategoria, RepositoryPromociónCategoria>();
builder.Services.AddTransient<IRepositoryPromocionProducto, RepositoryPromocionProducto>();
builder.Services.AddTransient<IRepositoryTipoPromocion, RepositoryTipoPromoción>();

// Servicios
builder.Services.AddTransient<IServiceCategoria, ServiceCategoria>();
builder.Services.AddTransient<IServiceProducto, ServiceProducto>();
builder.Services.AddTransient<IServiceImagenProducto, ServiceImagenProducto>();
builder.Services.AddTransient<IServiceEtiqueta, ServiceEtiqueta>();
builder.Services.AddTransient<IServiceEtiquetaProducto, ServiceEtiquetaProducto>();
builder.Services.AddTransient<IServiceResena, ServiceResena>();
builder.Services.AddTransient<IServiceUsuario, ServiceUsuario>();
builder.Services.AddTransient<IServicePromocion, ServicePromocion>();
builder.Services.AddTransient<IServicePromocionCategoria, ServicePromocionCategoria>();
builder.Services.AddTransient<IServicePromocionProducto, ServicePromocionProducto>();
builder.Services.AddTransient<IServiceTipoPromocion, ServiceTipoPromocion>();

// Servicios propios de la capa web
builder.Services.AddScoped<ICatalogoOfertas, CatalogoOfertas>();

// Configurar AutoMapper
builder.Services.AddAutoMapper(config =>
{
    config.AddProfile<CategoriaProfile>();
    config.AddProfile<ProductoProfile>();
    config.AddProfile<ImagenProductoProfile>();
    config.AddProfile<ResenaProfile>();
    config.AddProfile<EtiquetaProfile>();
    config.AddProfile<EtiquetaProductoProfile>();
    config.AddProfile<UsuarioProfile>();
    config.AddProfile<PromocionProfile>();
    config.AddProfile<PromocionCategoriaProfile>();
    config.AddProfile<PromocionProductoProfile>();
    config.AddProfile<TipoPromocionProfile>();
});


builder.Services.AddDbContext<VideoGameContext>(options =>
{
    // it read appsettings.json file
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerDataBase"));
    if (builder.Environment.IsDevelopment())
        options.EnableSensitiveDataLogging();
});




// Configuración de Serilog
var logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
    .Enrich.FromLogContext()
    .WriteTo.Console(LogEventLevel.Information)
    .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Information).WriteTo.File(Path.Combine("Logs", "Info-.log"), shared: true, encoding: Encoding.ASCII, rollingInterval: RollingInterval.Day))
    .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Debug).WriteTo.File(Path.Combine("Logs", "Debug-.log"), shared: true, encoding: Encoding.ASCII, rollingInterval: RollingInterval.Day))
    .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Warning).WriteTo.File(Path.Combine("Logs", "Warning-.log"), shared: true, encoding: Encoding.ASCII, rollingInterval: RollingInterval.Day))
    .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Error).WriteTo.File(Path.Combine("Logs", "Error-.log"), shared: true, encoding: Encoding.ASCII, rollingInterval: RollingInterval.Day))
    .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Fatal).WriteTo.File(Path.Combine("Logs", "Fatal-.log"), shared: true, encoding: Encoding.ASCII, rollingInterval: RollingInterval.Day))
    .CreateLogger();
builder.Host.UseSerilog(logger);

var app = builder.Build();
app.UseForwardedHeaders();

// Cultura es-CR con punto decimal para los montos de los formularios
var cultura = (CultureInfo)CultureInfo.GetCultureInfo("es-CR").Clone();
cultura.NumberFormat.NumberDecimalSeparator = ".";
cultura.NumberFormat.CurrencyDecimalSeparator = ".";
CultureInfo.DefaultThreadCurrentCulture = cultura;
CultureInfo.DefaultThreadCurrentUICulture = cultura;
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(cultura),
    SupportedCultures = new List<CultureInfo> { cultura },
    SupportedUICultures = new List<CultureInfo> { cultura },
    RequestCultureProviders = new List<IRequestCultureProvider>()
});

// Configurar el pipeline de solicitudes HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    app.UseMiddleware<ErrorHandlingMiddleware>();
}

app.UseStatusCodePagesWithReExecute("/no-encontrado");
app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.UseStaticFiles();

// Azure SQL serverless se pausa cuando no se usa; la primera conexión espera a que se reanude
var ultimaConexion = DateTime.MinValue;
app.Use(async (context, next) =>
{
    if (DateTime.UtcNow - ultimaConexion > TimeSpan.FromMinutes(30))
    {
        var db = context.RequestServices.GetRequiredService<VideoGameContext>();
        for (var intento = 1; intento <= 12; intento++)
        {
            try
            {
                await db.Database.OpenConnectionAsync();
                await db.Database.CloseConnectionAsync();
                break;
            }
            catch (Microsoft.Data.SqlClient.SqlException) when (intento < 12)
            {
                await Task.Delay(TimeSpan.FromSeconds(5));
            }
        }
    }
    ultimaConexion = DateTime.UtcNow;
    await next();
});
app.UseRouting();
app.UseAuthorization();
app.UseAntiforgery();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
