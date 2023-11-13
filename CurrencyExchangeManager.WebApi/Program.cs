using CurrencyExchangeManager.WebApi;
using CurrencyExchangeManager.WebApi.Service;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
AppSettings.ConnectionString = configuration.GetConnectionString("DefaultConnection").ToString();
AppSettings.ExchangeRateApiUrl = configuration.GetSection("AppSettings").GetSection("ExchangeRateApiUrl").Value;
AppSettings.RedisConnectionString = configuration.GetSection("AppSettings").GetSection("RedisConnectionString").Value;
AppSettings.ExchangeRateApiKey = configuration.GetSection("AppSettings").GetSection("ExchangeRateApiKey").Value;
AppSettings.RedisCacheExpiryTime = Convert.ToInt32(configuration.GetSection("AppSettings").GetSection("RedisCacheExpiryTime").Value);

// Add services to the container.
builder.Services.AddScoped<ICurrencyExchangeService, CurrencyExchangeService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

