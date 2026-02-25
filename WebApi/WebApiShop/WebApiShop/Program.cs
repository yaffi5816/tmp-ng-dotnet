using Microsoft.EntityFrameworkCore;
using Repositories;
using Services;
using DTO;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Gemini Configuration
builder.Services.Configure<GeminiSettings>(builder.Configuration.GetSection("GeminiSettings"));
builder.Services.Configure<AdminCredentials>(builder.Configuration.GetSection("AdminCredentials"));
builder.Services.AddHttpClient<IGeminiRepository, GeminiRepository>();
builder.Services.AddScoped<IDashboardService, DashboardService>();

// CORS Policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});





builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddTransient<ICategoryRepository, CategoryRepository>();
builder.Services.AddTransient<ICategoryService, CategoryService>();

builder.Services.AddTransient<IProductRepository, ProductRepository>();
builder.Services.AddTransient<IProductService, ProductService>();

builder.Services.AddTransient<IOrderRepository, OrderRepository>();
builder.Services.AddTransient<IOrderService, OrderService>();

builder.Services.AddDbContext<DashGen2026Context>(
    options=>options.UseSqlServer(
        builder.Configuration.GetConnectionString("EfratHome"),
        x => x.MigrationsAssembly("WebApiShop") ));

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddOpenApi();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "My API V1");
    });
}
// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseCors("AllowAngular");

app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.Run();
