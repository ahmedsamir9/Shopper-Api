using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Core.Entities.Identity;
using Core.Interfaces;
using Inferastructure.DB;
using Inferastructure.Identity;
using Inferastructure.Services;
using ShopperAPi.Errors;
using ShopperAPi.Middlewares;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var  MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddCors();

builder.Services.AddDbContext<AppDbContext>(op =>
    op.UseSqlServer(builder.Configuration.GetConnectionString("ShopperDb")));

builder.Services.AddDbContext<AppIdentityDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ShopIdentityDb"));
});

builder.Services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<AppIdentityDbContext>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:Audience"],
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
        ValidateLifetime = true,
    };
});

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

builder.Services.AddScoped<ITokenService, TokenService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<CustomExceptionMiddleware>();
app.UseStaticFiles();
app.UseStatusCodePagesWithReExecute("errors/{0}");

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors(builder =>
{
    builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
});

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
