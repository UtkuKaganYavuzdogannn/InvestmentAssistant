using InvestmentAssistant.Model;
using Microsoft.EntityFrameworkCore;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// CORS Ayarlar�n� Ekle
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy => policy.WithOrigins("http://localhost:3000") // React'�n �al��t��� port
                        .AllowAnyHeader()
                        .AllowAnyMethod());
});

// Swagger Deste�i
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// HttpClient ve Cache Servislerini Ekle
builder.Services.AddHttpClient();
builder.Services.AddMemoryCache(); // IMemoryCache i�in ekleme


builder.Services.AddDbContext<Varl�klarDbContext>
    (options =>
    options.UseMySql(builder.Configuration.GetConnectionString("MyDatabaseConnection"),
    new MySqlServerVersion(new Version(8, 0, 36))));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// CORS Middleware
app.UseCors("AllowReactApp");

app.UseAuthorization();

app.MapControllers();

app.Run();
