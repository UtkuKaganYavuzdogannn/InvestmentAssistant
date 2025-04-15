using InvestmentAssistant.Model;
using System.Net.Http;
using System.Text.Json;

namespace InvestmentAssistant.Services
{
    public class SimpleDataFetcherService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly HttpClient _httpClient;

        public SimpleDataFetcherService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
            _httpClient = new HttpClient();
        }

        public async Task StartFetchingAsync()
        {
            while (true)
            {
                try
                {
                    Console.WriteLine("⏳ Veri çekiliyor...");

                    string varliklar = "bitcoin,ethereum,tether,bnb,solana,xrp,dogecoin,cardano,tron,polkadot,tether-gold,tether-eurt,kinesis-silver,monerium-gbp-emoney";
                    string url = $"https://api.coingecko.com/api/v3/simple/price?ids={varliklar}&vs_currencies=usd";

                    var response = await _httpClient.GetAsync(url);
                    string json = await response.Content.ReadAsStringAsync();

                    var parsed = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, decimal>>>(json);

                    using (var scope = _scopeFactory.CreateScope())
                    {
                        var db = scope.ServiceProvider.GetRequiredService<VarlıklarDbContext>();

                        foreach (var item in parsed)
                        {
                            var entity = db.varliklar.FirstOrDefault(v => v.VarlikKodu == item.Key);

                            if (entity != null)
                            {
                                entity.Onceki_fiyati = entity.FiyatUsd;
                                entity.FiyatUsd = item.Value["usd"];
                                entity.GuncellenmeZamani = DateTime.Now;
                            }
                        }

                        await db.SaveChangesAsync();
                    }

                    Console.WriteLine("✅ Veri güncellendi.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("🚨 Hata oluştu: " + ex.Message);
                }

                await Task.Delay(TimeSpan.FromMinutes(1)); // 1 dakika bekle
            }
        }
    }
}
