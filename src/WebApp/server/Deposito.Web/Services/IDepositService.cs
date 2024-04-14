using Deposito.DB.Enums;
using Deposito.Web.Services.Dtos;

namespace Deposito.Web.Services;

public interface IDepositService
{
    Task<IEnumerable<DepositDto>> GetDeposits(double amount, Currency currency, int months,
        PayoutType payoutType);

    Task<CalculatedDepositDto> CalculateDeposit(string interestId, double amount);

    Task<DepositTable> FindMonthlyPaidDeposit(double amount, double interest, int period);
    Task<DepositTable> FindYearlyPaidDeposit(double amount, double interest, int period);

    Task<Stream> GenerateExcel(double amount, double interest, int period, PayoutType payoutType);
}