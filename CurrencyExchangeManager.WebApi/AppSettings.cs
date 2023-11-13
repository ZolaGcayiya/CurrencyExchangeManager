
namespace CurrencyExchangeManager.WebApi;

public static class AppSettings
{
	public static int RedisCacheExpiryTime { get; set; }
	public static string RedisConnectionString { get; set; }
	public static string ExchangeRateApiUrl { get; set; }
	public static string ExchangeRateApiKey { get; set; }

	public static string ConnectionString { get; set; }
}

