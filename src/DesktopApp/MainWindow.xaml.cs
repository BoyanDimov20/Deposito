using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using Deposito.DB;
using Deposito.DB.Models;

namespace Deposito.Desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private AppDbContext dbContext;

        public MainWindow()
        {
            InitializeComponent();
            try
            {
                this.dbContext = new AppDbContext();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            contentControl.Content = new RegisteringBank();
        }
        
        private void Page1_Click(object sender, RoutedEventArgs e)
        {
            contentControl.Content = new RegisteringBank();
        }

        private void Page2_Click(object sender, RoutedEventArgs e)
        {
            contentControl.Content = new RegisteringDeposit();
        }

        
    }
}