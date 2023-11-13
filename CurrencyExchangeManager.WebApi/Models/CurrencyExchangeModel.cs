using System;
namespace CurrencyExchangeManager.WebApi.Models;

public class CurrencyExchangeModel
{
    public bool success { get; set; }
    public int timestamp { get; set; }
    public string @base { get; set; }
    public string date { get; set; }
    public Dictionary<string, double> rates { get; set; }
}

