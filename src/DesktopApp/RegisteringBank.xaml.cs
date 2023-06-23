using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Deposito.DB;
using Deposito.DB.Models;

namespace Deposito.Desktop;

public partial class RegisteringBank : UserControl
{
    private AppDbContext dbContext;
    public RegisteringBank()
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
    }
    
    private void SubmitButton_Click(object sender, RoutedEventArgs e)
            {
                if (string.IsNullOrEmpty(bankIcon.Text) || string.IsNullOrEmpty(bankName.Text))
                {
                    MessageBox.Show($"Всички полета са задължителни.");
                    return;
                }
    
                this.dbContext.Banks.Add(new Bank
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = bankName.Text,
                    IconUrl = bankIcon.Text
                });
                this.dbContext.SaveChanges();
                
                bankName.Text = "";
                bankIcon.Text = "";
                MessageBox.Show($"Банката беше регистрирана успешно.");
            }
    
            private void BankIconUrlChanged(object sender, TextChangedEventArgs e)
            {
                if (!string.IsNullOrEmpty(bankIcon.Text))
                {
                    SetImageSource(bankIcon.Text);
                }
            }
    
            private bool IsBase64String(string value)
            {
                return value.StartsWith("data");
            }
    
            private void SetImageSource(string imageUrl)
            {
                try
                {
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
    
                    if (IsBase64String(imageUrl))
                    {
                        var data = imageUrl.Split(',')[1];
                        byte[] imageBytes = Convert.FromBase64String(data);
                        bitmap.StreamSource = new MemoryStream(imageBytes);
                    }
                    else
                    {
                        bitmap.UriSource = new Uri(imageUrl);
                    }
    
                    bitmap.EndInit();
                    img.Source = bitmap;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading image: {ex.Message}");
                }
            }
}