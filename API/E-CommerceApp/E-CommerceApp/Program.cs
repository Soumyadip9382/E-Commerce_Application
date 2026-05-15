using E_CommerceApp.Helper;
using E_CommerceApp.Infrastructure;
using E_CommerceApp.Interface;
using E_CommerceApp.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDBContext>(option => 
            option.UseSqlServer(builder.Configuration.GetConnectionString("Default")));


//Custom Dependency Injections
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ResolveCategoryPathHelper>();
builder.Services.AddScoped<GenerateSKUCodeHelper>();
builder.Services.AddScoped<CategoryPathHelper>();
builder.Services.AddScoped<BannerService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductVariantService, ProductVariantService>();


// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<UserService>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
