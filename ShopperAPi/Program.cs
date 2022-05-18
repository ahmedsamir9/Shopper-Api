using Core.Entities;
using Core.Interfaces;
using Inferastructure.DB;
using Inferastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopperAPi.Errors;
using ShopperAPi.Helpers;
using ShopperAPi.Middlewares;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddScoped<IBaseRepository<Category>,CategoryRepository>();
builder.Services.AddScoped<IBaseRepository<Product>, ProductRepository>();
builder.Services.AddScoped<IImageHandler, ImageHandler>();
string txt = "";
builder.Services.AddCors(options =>
{
    options.AddPolicy(txt,
    builder =>
    {
        builder.AllowAnyOrigin();
        builder.AllowAnyMethod();
        builder.AllowAnyHeader();
    });
});
// add Redis DB
builder.Services.AddSingleton<IConnectionMultiplexer>(op => {

    var redisConfiguration = ConfigurationOptions.Parse(builder.Configuration
        .GetConnectionString("Redis"), true);
    return ConnectionMultiplexer.Connect(redisConfiguration);
}
);
builder.Services.AddScoped<IBasketRepository, BasketRepository>();
//add SQL DB 
builder.Services.AddDbContext<AppDbContext>(op =>
op.UseSqlServer(builder.Configuration.GetConnectionString("ShopperDb")));
builder.Services.Configure<ApiBehaviorOptions> (options =>
{
    options.InvalidModelStateResponseFactory = actionContext =>
    {
        var errors = actionContext.ModelState
            .Where(e => e.Value.Errors.Count > 0)
            .SelectMany(x => x.Value.Errors)
            .Select(x => x.ErrorMessage).ToArray();

        var errorResponse = new ApiValidationErrorResponse
        {
            Errors = errors
        };
        return new BadRequestObjectResult(errorResponse);
    };
});
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies()); 
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<CustomExceptionMiddleware>();
app.UseStatusCodePagesWithReExecute("errors/{0}");
app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseRouting();
app.UseCors(txt);
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
