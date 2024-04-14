using Deposito.DB.Enums;

namespace Deposito.Web.Services.Dtos;

public class CalculatedDepositDto
{
    public string Id { get; set; }

    public double Percent { get; set; }

    public int Period { get; set; }

    public string Currency { get; set; }

    public double DepositedAmount { get; set; }

    public double AmountAfterDeposit { get; set; }

    public PayoutType PayoutType { get; set; }
}