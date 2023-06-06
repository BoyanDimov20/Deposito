using Deposito.DB;
using Deposito.Web.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseSqlite(AppDbContext.DbConnection);
});

builder.Services.AddScoped<IDepositService, DepositService>();

var app = builder.Build();


using (var scoped = app.Services.CreateScope())
{
    var appDbContext = scoped.ServiceProvider.GetService<AppDbContext>();

    await appDbContext.Database.EnsureCreatedAsync();
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();