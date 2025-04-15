using InvestmentAssistant.Model;
using InvestmentAssistant.Services;
using Microsoft.EntityFrameworkCore;



var builder = WebApplication.CreateBuilder(args);

//Uygulamanýn baþlamasý ile baþalayacak olan CoinUpdateService'ni ekliyoruz.
//builder.Services.AddHostedService<CoinUpdateService>();
builder.Services.AddSingleton<SimpleDataFetcherService>();




// Add services to the container.
builder.Services.AddControllers();

// CORS Ayarlarýný Ekle
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy => policy.WithOrigins("http://localhost:3000") // React'ýn çalýþtýðý port
                        .AllowAnyHeader()
                        .AllowAnyMethod());
});

// Swagger Desteði
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// HttpClient ve Cache Servislerini Ekle
builder.Services.AddHttpClient();
builder.Services.AddMemoryCache(); // IMemoryCache için ekleme


builder.Services.AddDbContext<VarlýklarDbContext>
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


var fetcher = app.Services.GetRequiredService<SimpleDataFetcherService>();
_ = fetcher.StartFetchingAsync(); // async olduðu için _ = kullanýyoruz

app.Run();
