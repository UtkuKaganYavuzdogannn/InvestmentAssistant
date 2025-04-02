using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Memory;

[Route("api/[controller]")]
[ApiController]
public class InvestmentController : Controller
{
    private readonly HttpClient _httpClient;
    private readonly IMemoryCache _cache;

    public InvestmentController(HttpClient httpClient, IMemoryCache cache)
    {
        _httpClient = httpClient;
        _cache = cache;
    }

    [HttpGet("prices")]   //https://localhost:7215/api/investment/prices 
    public async Task<IActionResult> GetKriptoVeEmtialar()
    {
        try
        {
            string varliklar = "bitcoin,ethereum,tether,bnb,solana,xrp,dogecoin,cardano,tron,polkadot,tether-gold,tether-eurt,kinesis-silver,monerium-gbp-emoney";
            string apiUrl = $"https://api.coingecko.com/api/v3/simple/price?ids={varliklar}&vs_currencies=usd";

            var apiCevabi = await _httpClient.GetAsync(apiUrl);
            string jsonApiCevabi = await apiCevabi.Content.ReadAsStringAsync();

            var price = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, decimal>>>(jsonApiCevabi);

            #region Json olarak gelen veriyi C# nesnelerine çevirip düzeltme.
            /*Coingecko borsasından bazı varlık isimleri diğer borsalarda olandan farklı geliyor.
              Bunu düzeltmek için Mapping sözlüğü oluşturuyorum. */

            var varlikAdiMap = new Dictionary<string, string>
            {
                { "bitcoin", "Bitcoin (BTC)" },
                { "ethereum", "Ethereum (ETH)" },
                { "tether", "Tether (USDT)" },
                { "cardano", "Cardano (ADA)" },
                { "dogecoin", "Dogecoin (DOGE)" },
                { "polkadot", "Polkadot (DOT)" },
                { "solana", "Solana (SOL)" },
                { "tether-gold", "Gold (XAU)" },
                { "tron", "Tron (TRX)" },
                { "tether-eurt", "EUR (EURT)" },
                { "kinesis-silver" , "Silver (XAG)" },
                { "monerium-gbp-emoney", "Sterling £ (GBP)" }
            };

            var guncellenmisVarliklar = new Dictionary<string, Dictionary<string, decimal>>();

            foreach (var item in price)
            {
                string piyasaAdlari;

                if (varlikAdiMap.ContainsKey(item.Key))
                {
                    piyasaAdlari = varlikAdiMap[item.Key];
                }
                else
                {
                    piyasaAdlari = item.Key; // Eğer eşleşme yoksa olduğu gibi bırak
                }

                guncellenmisVarliklar[piyasaAdlari] = item.Value;
            }
            #endregion

            return Ok(guncellenmisVarliklar);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
