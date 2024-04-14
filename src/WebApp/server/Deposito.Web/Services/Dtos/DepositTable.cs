namespace Deposito.Web.Services.Dtos;

public class DepositTable
{
    public IList<double> Amounts { get; set; }
    public IList<double> Interests { get; set; }
    public IList<double> InterestIncomes { get; set; }
    public IList<double> NetIncomes { get; set; }
}