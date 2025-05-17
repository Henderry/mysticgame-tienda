using Libreria.Web.Middleware;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Events;
using System.Text;
using Tienda.Application.Profiles;
using Tienda.Application.Services.Implementations;
using Tienda.Application.Services.Interfaces;
using Tienda.Infraestructure.Data;
using Tienda.Infraestructure.Repository.Implementations;
using Tienda.Infraestructure.Repository.Interfaces;

var builder = WebApplication.CreateBuilder(args);
var connString = builder.Configuration.GetConnectionString("SqlServerDataBase");

// Add services to the container.
builder.Services.AddControllersWithViews();

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
    .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Information).WriteTo.File(@"Logs\Info-.log", shared: true, encoding: Encoding.ASCII, rollingInterval: RollingInterval.Day))
    .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Debug).WriteTo.File(@"Logs\Debug-.log", shared: true, encoding: Encoding.ASCII, rollingInterval: RollingInterval.Day))
    .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Warning).WriteTo.File(@"Logs\Warning-.log", shared: true, encoding: Encoding.ASCII, rollingInterval: RollingInterval.Day))
    .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Error).WriteTo.File(@"Logs\Error-.log", shared: true, encoding: Encoding.ASCII, rollingInterval: RollingInterval.Day))
    .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Fatal).WriteTo.File(@"Logs\Fatal-.log", shared: true, encoding: Encoding.ASCII, rollingInterval: RollingInterval.Day))
    .CreateLogger();
builder.Host.UseSerilog(logger);

var app = builder.Build();

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

app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.UseAntiforgery();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
