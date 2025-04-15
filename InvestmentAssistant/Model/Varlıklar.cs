namespace InvestmentAssistant.Model
{
    public class Varlıklar
    {
        public int Id { get; set; }
        public string VarlikKodu { get; set; }
        public string PiyasaAdi { get; set; }
        public decimal FiyatUsd { get; set; }
        public DateTime GuncellenmeZamani { get; set; } // Coingecko'dan çekilme zamanı

        public decimal Onceki_fiyati { get; set; } // Önceki fiyatı
    }
}
