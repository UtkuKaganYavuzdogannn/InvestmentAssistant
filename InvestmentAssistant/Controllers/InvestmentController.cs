using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Net.Http;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class InvestmentController : ControllerBase
{
    private readonly HttpClient _httpClient;
    private readonly IMemoryCache _cache;
    private const string ApiKey = "CG-VScB9m3aeevEG8SALpEZMQyV"; // API Key
    private const string CacheKey = "KriptoVeEmtialar"; // Cache anahtarı

    public InvestmentController(HttpClient httpClient, IMemoryCache cache)
    {
        _httpClient = httpClient;
        _cache = cache;
    }

    [HttpGet("prices")]
    public async Task<IActionResult> GetKriptoVeEmtialar()
    {
        try
        {
            // Eğer cache'de veri varsa onu döndür
            if (_cache.TryGetValue(CacheKey, out string cachedData))
            {
                return Ok(cachedData);
            }

            // Çekilecek varlıklar
            string varliklar = "bitcoin,ethereum,tether,bnb,solana,xrp,dogecoin,cardano,tron,polkadot,gold,silver";
            string apiUrl = $"https://api.coingecko.com/api/v3/simple/price?ids={varliklar}&vs_currencies=usd";

            // API key'i header'a ekle
            _httpClient.DefaultRequestHeaders.Add("x-cg-api-key", ApiKey);

            var apiResponse = await _httpClient.GetAsync(apiUrl);

            if (!apiResponse.IsSuccessStatusCode)
            {
                return StatusCode((int)apiResponse.StatusCode, "Fiyatları alırken hata oluştu.");
            }

            string result = await apiResponse.Content.ReadAsStringAsync();

            // Veriyi cache'e ekle, 30 saniye boyunca sakla
            _cache.Set(CacheKey, result, TimeSpan.FromSeconds(30));

            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Sunucu hatası: {ex.Message}");
        }
    }
}
