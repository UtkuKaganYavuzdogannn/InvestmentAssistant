using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using InvestmentAssistant.Model; // Varliklar ve context buradan gelecek
/*

public class CoinUpdateService : BackgroundService   //Backround service, arka planda çalışan bir servistir.Bu yüzden bu sınıfı miras aldım.
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {


        while (!stoppingToken.IsCancellationRequested)
        {
            Console.WriteLine("Servis çalışıyor... " + DateTime.Now);

            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken); // 10 saniye beklesin

        }



    }
}
*/