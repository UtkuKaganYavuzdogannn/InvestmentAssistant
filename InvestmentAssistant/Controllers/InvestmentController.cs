using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class InvestmentController : ControllerBase
{
    private readonly HttpClient _httpClient;
    private const string ApiKey = "99ed4fc6765e8e20541a472ea4039933"; // API Key

    public InvestmentController(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    [HttpGet("prices")]
    public async Task<IActionResult> GetCryptoAndCommodityPrices()
    {
        try
        {
            // Çekilecek varlıklar (kripto paralar ve emtialar)
            string assets = "bitcoin,ethereum,tether,bnb,solana,xrp,dogecoin,cardano,tron,polkadot,gold,silver";

            // API URL'si
            string apiUrl = $"https://api.coingecko.com/api/v3/simple/price?ids={assets}&vs_currencies=usd";

            // API key'i header'a ekliyoruz
            _httpClient.DefaultRequestHeaders.Add("x-cg-api-key", ApiKey);

            HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, "Fiyatları alırken hata oluştu.");
            }

            string result = await response.Content.ReadAsStringAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Sunucu hatası: {ex.Message}");
        }
    }
}
