using CurrencyExchangeManager.WebApi.Service;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CurrencyExchangeManager.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ExchangeController : ControllerBase
{
    private readonly ILogger<ExchangeController> _logger;
    private readonly ICurrencyExchangeService _exchangeService;
    public ExchangeController(ILogger<ExchangeController> logger, ICurrencyExchangeService exchangeService)
    {
        _logger = logger;
        _exchangeService = exchangeService;
    }

    [HttpGet]
    public async Task<IActionResult> ConvertCurrency(string sourceCurrency, string targetCurrency, decimal amount)
    {
        var result = await _exchangeService.ConvertCurrency(sourceCurrency, targetCurrency, amount);
        return Ok(new
        {
            Amount = amount,
            SourceCurrency = sourceCurrency,
            TargetCurrency = targetCurrency,
            Result = result
        });
    }
}

