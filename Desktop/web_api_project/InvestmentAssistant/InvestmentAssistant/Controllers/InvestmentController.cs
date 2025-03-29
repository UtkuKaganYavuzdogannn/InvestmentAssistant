using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

[Route("api/[controller]")]
[ApiController]
public class InvestmentController : Controller
{

    //Tek bir istekte aynı httpClient kullanacağımızdan tekrar tekrar kullanmak için HttpClient nesnesi oluşturuyoruz.
    private readonly HttpClient _httpClient;

    /* Coingecko ücretsiz apisi kullandığımız için buna şuan gerek yok.
    private const string ApiKey = "CG-VScB9m3aeevEG8SALpEZMQyV0000";  API Key */


    //Dependency Injection ile HttpClient nesnesini yani "httpClient"ı kendi yerel değişkenimiz olan _httpClient'a enjekte ediyoruz.
    public InvestmentController(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }







    [HttpGet("prices")]   //https://localhost:5001/api/investment/prices 
    public async Task<IActionResult> GetKriptoVeEmtialar()
    {
        try
        {

            // Çekilecek varlıklar ve API URL'si (kag:gümüş, gbp:sterlin) Varlıklar arasında virgülle ayırarak yazıyoruz. Bu formatta yazmamızın sebebi API'nin bu formatta istek yapılmasını istemesi.
            string varliklar = "bitcoin,ethereum,tether,bnb,solana,xrp,dogecoin,cardano,tron,polkadot,tether-gold,tether-eurt,kinesis-silver,monerium-gbp-emoney";
            string apiUrl = $"https://api.coingecko.com/api/v3/simple/price?ids={varliklar}&vs_currencies=usd";


            /* _httpClient.DefaultRequestHeaders.Add("x-cg-api-key", ApiKey);
             * Eğer request'i api key ile atacak olsaydım burada headera apikeyi eklerdim.*/

            var apiCevabi = await _httpClient.GetAsync(apiUrl);

            string jsonApiCevabi = await apiCevabi.Content.ReadAsStringAsync();


            /*Json veriyi parçalara ayırmak ve daha sonra daha iyi manipüle edebilmek için Dictionary kullanıyoruz.
             *  "bitcoin": { "usd": 86998 } formatında bir json veri yani string=string,decimal.*/
            var price = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, decimal>>>(jsonApiCevabi);

            return Ok(price);


        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
   




