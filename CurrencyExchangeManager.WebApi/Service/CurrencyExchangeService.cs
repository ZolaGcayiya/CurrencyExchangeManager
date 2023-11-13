using System;
using System.Net.Http;
using CurrencyExchangeManager.WebApi.Models;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace CurrencyExchangeManager.WebApi.Service;

public class CurrencyExchangeService : ICurrencyExchangeService
{
    public async Task<decimal> ConvertCurrency(string sourceCurrency, string targetCurrency, decimal amount)
    {
        var rate = 0d;
        using (var redis = ConnectionMultiplexer.Connect(AppSettings.RedisConnectionString))
        {
            var db = redis.GetDatabase();
            var cacheKey = $"{sourceCurrency}_{targetCurrency}";
            var cachedRate = await db.StringGetAsync(cacheKey);
            if (cachedRate.HasValue)
                rate = Convert.ToDouble(cachedRate);
            else
            {
                rate = await GetExchangeRate(sourceCurrency, targetCurrency);

                // Store the rate in Redis
                await db.StringSetAsync(cacheKey, rate, TimeSpan.FromMinutes(AppSettings.RedisCacheExpiryTime));
            }
        }

        // Store the exchange rate in MySQL
        using (var connection = new MySqlConnection(AppSettings.ConnectionString))
        {
            await connection.OpenAsync();
            var query = $"INSERT INTO exchange_rate_history (source_currency, target_currency, rate, created_date) VALUES ('{sourceCurrency.ToString().Trim()}', '{targetCurrency.ToString().Trim()}', {rate.ToString().Trim()}, NOW())";
            using (var command = new MySqlCommand(query, connection))
            {
                await command.ExecuteNonQueryAsync();
            }
        }

        return amount * (decimal)rate;
    }

    private static async Task<double> GetExchangeRate(string sourceCurrency, string targetCurrency)//, decimal amount)
    {
        try
        {
            using (var httpClient = new HttpClient())
            {
                string url = AppSettings.ExchangeRateApiUrl + "&from=" + sourceCurrency.ToString().Trim() + "&to=" + targetCurrency.ToString().Trim();// + "&amount=" + amount.ToString().Trim();
                var response = await httpClient.GetStringAsync(url);
                var exchangeRates = JsonConvert.DeserializeObject<CurrencyExchangeModel>(response);
                
                if (exchangeRates is not null && exchangeRates.rates is not null)
                {
                    var rate = exchangeRates.rates[targetCurrency];
                    return rate;
                }
                else
                    throw new Exception($"Exchange rate not found for {sourceCurrency} to {targetCurrency}");
            }
        }
        catch(Exception ex)
        {
            throw;
        }
    }
}