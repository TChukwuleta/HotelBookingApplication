using HotelBooking.Application.Hotel.Commands;
using HotelBooking.Application.Hotel.Queries;
using HotelBooking.Application.Interfaces;
using HotelBooking.Application.Interfaces.IRepositories;
using HotelBooking.Application.Interfaces.IRepositories.Base;
using HotelBooking.Infrastructure.Data;
using HotelBooking.Infrastructure.Services;
using HotelBooking.Infrastructure.Services.Repositories;
using HotelBooking.Infrastructure.Services.Repositories.Base;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
builder.Services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddMediatR(typeof(CreateHotelCommandHandler).Assembly);
builder.Services.AddMediatR(typeof(GetAllHotelsQueryHandler).Assembly);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Hotel.API",
        Version = "v1"
    });
});
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddTransient<IHotelRepository, HotelRepository>();
builder.Services.AddTransient<IUploadService, UploadService>();
builder.Services.AddTransient<IPaystackService, PaystackService>();
builder.Services.AddTransient<IApiClientService, ApiClientService>();
builder.Services.AddScoped<IAppDbContext>(provider => provider.GetService<AppDbContext>());

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
