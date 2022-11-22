using DeliveryWebApi.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DeliveryAPIDbContext>(option =>
{ option.UseSqlServer(builder.Configuration.GetConnectionString("SQLServer")); }
);
builder.Services.AddHttpClient("Catalogue", options => options.BaseAddress = new Uri("http://localhost:12763"));
builder.Services.AddHttpClient("Orders", options => options.BaseAddress = new Uri("http://localhost:13411"));
var app = builder.Build();

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
