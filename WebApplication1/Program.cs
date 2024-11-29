using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

var builder = WebApplication.CreateBuilder(args);

// Configurar la cadena de conexi�n
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Agregar el contexto de la base de datos al contenedor de servicios
builder.Services.AddDbContext<PlataformaCursosContext>(options =>
    options.UseSqlServer(connectionString));

// Habilitar el uso de sesiones
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Tiempo de expiraci�n de la sesi�n
    options.Cookie.HttpOnly = true; // Seguridad de la cookie
    options.Cookie.IsEssential = true; // Esencial para la funcionalidad del sitio
});

// Agregar servicios para controladores con vistas (MVC)
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<DashboardService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

// Middleware de sesi�n
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
