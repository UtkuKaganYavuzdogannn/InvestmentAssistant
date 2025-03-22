using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class InvestmentController : ControllerBase
{
    private readonly HttpClient _httpClient;
    private const string ApiKey = "CG-VScB9m3aeevEG8SALpEZMQyV"; // API Key

    public InvestmentController(HttpClient httpClient)
    {
        _httpClient =  httpClient;
    }

    [HttpGet("prices")]
    public async Task<IActionResult> GetKriptoVeEmtialar()
    {
        try
        {




            // Çekilecek varlıklar (kripto paralar ve emtialar)
            string varliklar = "bitcoin,ethereum,tether,bnb,solana,xrp,dogecoin,cardano,tron,polkadot,gold,silver";

            string apiUrl = $"https://api.coingecko.com/api/v3/simple/price?ids={varliklar}&vs_currencies=usd";


            // API key'i header'a ekliyoruz
            _httpClient.DefaultRequestHeaders.Add("x-cg-api-key", ApiKey);

            var api_cevabı = await _httpClient.GetAsync(apiUrl);


            /*
            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, "Fiyatları alırken hata oluştu.");
            }
            */
          
            string result = await api_cevabı.Content.ReadAsStringAsync();
            return Ok(result);
        }
       
        
        
        
        
        catch (Exception ex)
        {
            return StatusCode(500, $"Sunucu hatası: {ex.Message}");
        }
    }
}
