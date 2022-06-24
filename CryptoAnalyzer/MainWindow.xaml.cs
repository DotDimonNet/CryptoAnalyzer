using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CryptoAnalyzer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Upload_Click(object sender, RoutedEventArgs e)
        {
            await BL.UploadCsvFile(pathUpload.Text);
            Real.ItemsSource = BL.CryptoRealRows;
            Pred.ItemsSource = BL.CryptoPredictiveRows;
        }

        private async void Start_Click(object sender, RoutedEventArgs e)
        {
            ClearValues();

            await BL.FindBestValue();

            await WriteLogToFile();
            
            MessageBox.Show($"Finish!!! Please, check your folder to see the results: {pathResult.Text}", "Finish", MessageBoxButton.OK, MessageBoxImage.Information);
            Process.Start("notepad.exe", BL.FullResultPath);
        }

        private void ClearValues()
        {
            BL.Account.BalanceBTC = double.Parse(balanceBTC.Text);
            BL.Account.BalanceUSD = double.Parse(balanceUSD.Text);
            BL.Account.TreshholdBTCtoUSD = double.Parse(tresholdBTC.Text);
            BL.Account.TreshholdUSDtoBTC = double.Parse(tresholdUSD.Text);
            BL.Account.MaxActionFromTotalinPercent = double.Parse(limitAction.Text);
            BL.Logs.Clear();
        }

        private async Task WriteLogToFile()
        {
            BL.FullResultPath = @$"{pathResult.Text}\result-${DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss")}.txt";
            using (StreamWriter sw = new StreamWriter(BL.FullResultPath, false))
            {
                foreach (var log in BL.Logs)
                {
                    await sw.WriteLineAsync(log.Message);
                }
            }
        }
    }
}
