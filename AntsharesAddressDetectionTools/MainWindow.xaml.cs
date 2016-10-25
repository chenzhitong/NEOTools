using System;
using System.Collections.Generic;
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
using AntShares.Wallets;
using System.Collections.ObjectModel;

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
