using System;
namespace CurrencyExchangeManager.WebApi.Service;

public interface ICurrencyExchangeService
{
    Task<decimal> ConvertCurrency(string sourceCurrency, string targetCurrency, decimal amount);
}

