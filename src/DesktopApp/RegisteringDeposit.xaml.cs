using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Deposito.DB;
using Deposito.DB.Enums;
using Deposito.DB.Models;
using Deposito.Desktop.Models;
using Microsoft.EntityFrameworkCore;

namespace Deposito.Desktop;

public partial class RegisteringDeposit : UserControl
{
    private AppDbContext dbContext;
    private List<InterestInfo> interests = new List<InterestInfo>();

    public RegisteringDeposit()
    {
        InitializeComponent();
        try
        {
            this.dbContext = new AppDbContext();

            GetBanksAsync();
            
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    private async Task GetBanksAsync()
    {
        
        var banks = await this.dbContext.Banks.Select(x => new BankInfo
        {
            Id = x.Id,
            Name = x.Name
        }).ToListAsync();
        banksComboBox.ItemsSource = banks;
    }
    private void CurrencyComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        // Get the selected items
        var selectedItems = currencyTextBox.Text.Split(Environment.NewLine).ToList();
        if (string.IsNullOrEmpty(currencyTextBox.Text))
            selectedItems.Clear();
        foreach (ComboBoxItem item in currencyComboBox.Items)
        {
            if (item.IsSelected)
            {
                selectedItems.Add(item.Content.ToString());
            }
        }

        currencyTextBox.Text = string.Join(Environment.NewLine, selectedItems);
    }

    private void numericTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        e.Handled = !int.TryParse(e.Text, out _);
    }

    private void OnSubmit(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(banksComboBox.Text) || string.IsNullOrEmpty(depositTypeTextBox.Text) ||
            string.IsNullOrEmpty(currencyTextBox.Text) || string.IsNullOrEmpty(descriptionTextBox.Text)
            || string.IsNullOrEmpty(minAmountTextBox.Text) || string.IsNullOrEmpty(maxAmountTextBox.Text)
            || string.IsNullOrEmpty(payoutTypeTextBox.Text))
        {
            MessageBox.Show("Всички полета са задължителни");
            return;
        }

        var currencies = currencyTextBox.Text.Split(Environment.NewLine).Select(x => Enum.Parse<Currency>(x));
        foreach (var currency in currencies)
        {
            var depositId = Guid.NewGuid().ToString();
            this.dbContext.Add(new Deposit
            {
                Id = depositId,
                Code = Guid.NewGuid().ToString(),
                BankId = ((BankInfo)banksComboBox.SelectionBoxItem).Id,
                Currency = currency,
                Description = descriptionTextBox.Text,
                Type = Enum.Parse<PayoutType>((string)((ComboBoxItem)payoutTypeTextBox.SelectedItem).Tag),
                MinimalAmount = double.Parse(minAmountTextBox.Text),
                MaximumAmount = double.Parse(maxAmountTextBox.Text)
            });

            foreach (var interest in this.interests)
            {
                this.dbContext.Add(new Interest
                {
                    Id = Guid.NewGuid().ToString(),
                    DepositId = depositId,
                    Description = ".",
                    Percent = interest.Percent,
                    PeriodInMonths = interest.Period
                });
            }
        }

        this.dbContext.SaveChanges();

        MessageBox.Show("Депозитът беше създаден успешно");
        ClearValues();
    }

    private void ClearValues()
    {
        banksComboBox.Text = string.Empty;
        descriptionTextBox.Text = string.Empty;
        depositTypeTextBox.Text = string.Empty;
        currencyTextBox.Text = string.Empty;
        minAmountTextBox.Text = string.Empty;
        maxAmountTextBox.Text = string.Empty;
        payoutTypeTextBox.Text = string.Empty;
        interestsTextBox.Text = string.Empty;
    }

    private void OpenInterestDialog(object sender, RoutedEventArgs e)
    {
        var dialog = new InterestDialog();
        var submitInterestBtn = dialog.FindName("SubmitInterest") as Button;
        submitInterestBtn.Click += (o, args) =>
        {
            var period = int.Parse((string)((dialog.FindName("period") as ComboBox).SelectedItem as ComboBoxItem).Tag);
            var percent = double.Parse((dialog.FindName("interestPercent") as TextBox).Text,
                CultureInfo.InvariantCulture);
            this.interests.Add(new InterestInfo
            {
                Percent = percent,
                Period = period
            });
            interestsTextBox.Text = string.Join(", ", interests.Select(x => $"{x.Period} месеца"));
            dialog.Close();
        };
        dialog.ShowDialog();
    }
}