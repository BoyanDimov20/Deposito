namespace Deposito.Web.Services.Dtos;

public class DepositDto
{
    public string Id { get; set; }
    
    public string BankLogo { get; set; }

    public string Description { get; set; }

    public double PercentInterest { get; set; }

    public double MinimalAmount { get; set; }

    public string Currency { get; set; }
}