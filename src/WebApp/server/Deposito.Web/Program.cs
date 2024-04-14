using Deposito.DB;
using Deposito.Web.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddDbContext<AppDbContext>();

builder.Services.AddTransient<IDepositService, DepositService>();

var app = builder.Build();


using (var scoped = app.Services.CreateScope())
{
    var appDbContext = scoped.ServiceProvider.GetService<AppDbContext>();

    await appDbContext.Database.EnsureCreatedAsync();
}


app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthorization();

app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();