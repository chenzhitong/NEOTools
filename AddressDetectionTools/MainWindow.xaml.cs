using Neo.Wallets;
using System;
using System.Collections.Generic;
using System.Windows;

namespace AntsharesAddressDetectionTools
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            textBoxError.Clear();
            var errorList = new List<string>();
            var inputs = textBoxInput.Text.Replace("\r", "").Split('\n');
            foreach (var item in inputs)
            {
                if(!string.IsNullOrEmpty(item))
                try
                {
                    Wallet.ToScriptHash(item);
                }
                catch (FormatException)
                {
                    errorList.Add(item);
                }
            }
            errorList.Add("检测完毕");
            textBoxError.Text = string.Join("\r\n", errorList);
        }
    }
}
