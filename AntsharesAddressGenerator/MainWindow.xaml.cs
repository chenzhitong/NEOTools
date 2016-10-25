using System.Windows;
using AntShares.Wallets;
using System.Threading;
using System.Windows.Threading;
using System;
using System.Linq;
using System.IO;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Security.Cryptography;

namespace AddressGenerator
{
    public partial class MainWindow : Window
    {
        bool pause = true;
        string[] startWith;
        ObservableCollection<GoodAddress> goodAddresses = new ObservableCollection<GoodAddress>();
        Thread[] threads = new Thread[8];

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            textProcessorCount.Text = Environment.ProcessorCount.ToString();
            startWith = File.ReadAllLines("StartWith.txt");
            dataGrid1.ItemsSource = goodAddresses;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var count = 0;
            if (pause)
            {
                pause = false;
                for (int i = 0; i < Math.Min(Environment.ProcessorCount, 8); i++)
                {
                    threads[i] = new Thread(Run);
                    threads[i].Start();
                    count++;
                }
            }
            else
            {
                pause = true;
            }

            textThreadCount.Text = count.ToString();
            (sender as Button).Content = pause ? "开始生成" : "停止生成";
        }

        public void Run()
        {
            byte[] privateKey = new byte[32];
            while (!pause)
            {
                using (CngKey key = CngKey.Create(CngAlgorithm.ECDsaP256, null, new CngKeyCreationParameters { ExportPolicy = CngExportPolicies.AllowPlaintextArchiving }))
                {
                    privateKey = key.Export(CngKeyBlobFormat.EccPrivateBlob);
                }
                Generate(privateKey);
            }
        }

        public void Generate(byte[] privateKey)
        {
            var account = new Account(privateKey);
            var contract = Contract.CreateSignatureContract(account.PublicKey);
            if (startWith.Any(p => contract.Address.ToLower().StartsWith(p)) && !goodAddresses.Any(p => p.Address == contract.Address))
            {
                Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                {
                    goodAddresses.Add(new GoodAddress()
                    {
                        Address = contract.Address,
                        Privatekey = account.Export()
                    });
                });
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
