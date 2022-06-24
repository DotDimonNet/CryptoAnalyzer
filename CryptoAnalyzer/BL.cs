using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoAnalyzer
{
    public static class BL
    {
        public static Account Account { get; set; } = new Account();
        public static string FullResultPath { get; set; }

        public static double initialUSD = 0;
        public static double initialBTC = 0;
        public static double priceBTC = 0;
        public static double priceUSD = 0;
        public static List<CryptoRow> CryptoRealRows { get; set; } = new List<CryptoRow>();
        public static List<CryptoRow> CryptoPredictiveRows { get; set; } = new List<CryptoRow>();
        public static List<Log> Logs { get; set; } = new List<Log>();

        public static async Task FindBestValue()
        {
            if (CryptoPredictiveRows.Any() && CryptoRealRows.Any())
            {
                for (int index = 0; index < CryptoRealRows.Count; index++)
                {
                    var pred = CryptoPredictiveRows[index];
                    var real = CryptoRealRows[index];

                    priceBTC = ((real.MaxRate + real.MinRate) / 2);
                    priceUSD = 1 / priceBTC;

                    if (index == 0)
                    {
                        Logs.Add(new Log
                        {
                            Date = DateTime.Now,
                            Message = $"-------------START--------------------------"
                        });
                        Logs.Add(new Log
                        {
                            Date = DateTime.Now,
                            Message = $"Date::: {pred.Date.ToShortDateString()} Balance::: USD: {Math.Round(Account.BalanceUSD, 5)}$ \t\t---\t\t BTC: {Math.Round(Account.BalanceBTC, 5)} BTC || TOTAL - {Math.Round(Account.BalanceBTC * priceBTC + Account.BalanceUSD, 5)}"
                        });
                        Logs.Add(new Log
                        {
                            Date = DateTime.Now,
                            Message = $"-------------START--------------------------"
                        });
                    }

                    await CheckAndBuyUSD(real, pred);
                    await CheckAndBuyBTC(real, pred);

                    if (index == CryptoRealRows.Count - 1)
                    {
                        Logs.Add(new Log
                        {
                            Date = DateTime.Now,
                            Message = $"-------------FINAL--------------------------"
                        });
                        Logs.Add(new Log
                        {
                            Date = DateTime.Now,
                            Message = $"Date::: {pred.Date.ToShortDateString()} Balance::: USD: {Math.Round(Account.BalanceUSD, 5)}$ \t\t---\t\t BTC: {Math.Round(Account.BalanceBTC, 5)} BTC || TOTAL - {Math.Round(Account.BalanceBTC * priceBTC + Account.BalanceUSD, 5)}"
                        });
                        Logs.Add(new Log
                        {
                            Date = DateTime.Now,
                            Message = $"-------------FINAL--------------------------"
                        });
                    }
                }
                
            }
        }

        private static async Task CheckAndBuyBTC(CryptoRow pred, CryptoRow real)
        {
            if ((pred.MinRate / real.MinRate) > 1 && (pred.MinRate / real.MinRate) < 1 + Account.TreshholdUSDtoBTC && Account.BalanceUSD > 0)
            {
                var usd = Account.BalanceUSD * Account.MaxActionFromTotalinPercent / 100;
                var btc = usd * priceUSD;

                if (usd > Account.BalanceUSD)
                {
                    Account.BalanceBTC += Account.BalanceUSD * priceUSD;
                    Account.BalanceUSD = 0;
                }
                else
                {
                    Account.BalanceUSD -= usd;
                    Account.BalanceBTC += btc;
                }
                Logs.Add(new Log
                {
                    Date = pred.Date,
                    Message = $"Buy BTC --- Date::: {pred.Date.ToShortDateString()} Balance::: USD: {Math.Round(Account.BalanceUSD, 5)}$ \t\t---\t\t BTC: {Math.Round(Account.BalanceBTC, 5)} BTC || TOTAL - {Math.Round(Account.BalanceBTC * priceBTC + Account.BalanceUSD, 5)}"
                });

            }
        }

        private static async Task CheckAndBuyUSD(CryptoRow pred, CryptoRow real)
        {
            if ((real.MaxRate / pred.MaxRate) > 1 && (real.MaxRate / pred.MaxRate) < 1 + Account.TreshholdBTCtoUSD && Account.BalanceBTC > 0)
            {
                var btc = Account.BalanceBTC * Account.MaxActionFromTotalinPercent / 100;
                var usd = btc * priceBTC;

                if (btc > Account.BalanceBTC)
                {
                    Account.BalanceUSD += Account.BalanceBTC * priceBTC;
                    Account.BalanceBTC = 0;
                }
                else
                {
                    Account.BalanceUSD += usd;
                    Account.BalanceBTC -= btc;
                }
                Logs.Add(new Log
                {
                    Date = pred.Date,
                    Message = $"Buy USD --- Date::: {pred.Date.ToShortDateString()} Balance::: USD: {Math.Round(Account.BalanceUSD, 5)}$ \t\t---\t\t BTC: {Math.Round(Account.BalanceBTC, 5)} BTC || TOTAL - {Math.Round(Account.BalanceBTC * priceBTC + Account.BalanceUSD, 5)}"
                });

            }
        }

        public static async Task UploadCsvFile(string path)
        {
            var strings = await File.ReadAllLinesAsync(path, Encoding.UTF8);
            
            foreach (var row in strings)
            {
                var data = row.Split(',');
                DateTime.TryParse(data[0], out DateTime date);
                double.TryParse(data[1], out double maxPredictive);
                double.TryParse(data[2], out double minPredictive);
                double.TryParse(data[3], out double closePredictive);
                double.TryParse(data[4], out double maxReal);
                double.TryParse(data[5], out double minReal);
                double.TryParse(data[6], out double closeReal);
                
                var cryptoPredictiveRow = new CryptoRow
                {
                    Date = date,
                    MaxRate = maxPredictive,
                    MinRate = minPredictive,
                    EndRate = closePredictive,
                };

                CryptoPredictiveRows.Add(cryptoPredictiveRow);

                var cryptoRealRow = new CryptoRow
                {
                    Date = date,
                    MaxRate = maxReal,
                    MinRate = minReal,
                    EndRate = closeReal,
                };

                CryptoRealRows.Add(cryptoRealRow);
            }
        }
    }
}
