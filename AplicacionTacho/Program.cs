using AplicacionTacho.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// -------------------------------
// 1. Agregar MVC
// -------------------------------
builder.Services.AddControllersWithViews();

// -------------------------------
// 2. Registrar DbContext con SQLite
// -------------------------------
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=appstore.db"));
// Render creará appstore.db dentro del contenedor

var app = builder.Build();

// -------------------------------
// 3. APLICAR MIGRACIONES AUTOMÁTICAMENTE
//    (esto soluciona el error "no such table")
// -------------------------------
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();  // CREA TABLAS SI NO EXISTEN
}

// -------------------------------
// 4. Middleware HTTP
// -------------------------------
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();   // NECESARIO PARA QUE SIRVA EL APK

app.UseRouting();

app.UseAuthorization();

// -------------------------------
// 5. Ruta por defecto
// -------------------------------
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
