using Deposito.DB.Enums;
using Deposito.Web.Services.Dtos;

namespace Deposito.Web.Services;

public interface IDepositService
{
    Task<IEnumerable<DepositDto>> GetDeposits(double amount, Currency currency, int months,
        PayoutType payoutType);

    Task<CalculatedDepositDto> CalculateDeposit(string interestId, double amount);
}