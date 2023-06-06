using Deposito.DB;
using Deposito.DB.Enums;
using Deposito.Web.Services.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Deposito.Web.Services;

public class DepositService : IDepositService
{
    private readonly AppDbContext _appDbContext;

    public DepositService(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<IEnumerable<DepositDto>> GetDeposits(double amount, Currency currency, int months,
        PayoutType payoutType)
    {
        var deposits = await _appDbContext.Interests
            .AsNoTracking()
            .Where(x => x.PeriodInMonths == months
                        && x.Deposit.MinimalAmount <= amount
                        && x.Deposit.MaximumAmount >= amount
                        && x.Deposit.Currency == currency
                        && x.Deposit.Type == payoutType)
            .Select(x => new DepositDto
            {
                Id = x.Id,
                Description = x.Deposit.Description,
                BankLogo = x.Deposit.Bank.IconUrl,
                MinimalAmount = x.Deposit.MinimalAmount,
                PercentInterest = x.Percent,
                Currency = x.Deposit.Currency.ToString()
            })
            .ToListAsync();


        return deposits;
    }

    public async Task<CalculatedDepositDto> CalculateDeposit(string interestId, double amount)
    {
        var interest = await this._appDbContext.Interests
            .AsNoTracking()
            .Where(x => x.Id == interestId
                        && amount >= x.Deposit.MinimalAmount
                        && amount <= x.Deposit.MaximumAmount)
            .Select(x => new CalculatedDepositDto
            {
                Id = x.Id,
                Percent = x.Percent,
                Currency = x.Deposit.Currency.ToString(),
                Period = x.PeriodInMonths,
                DepositedAmount = amount,
                AmountAfterDeposit = amount + (amount * (x.Percent / 100))
            })
            .FirstOrDefaultAsync();

        if (interest is null)
        {
            throw new ArgumentNullException(nameof(interest));
        }


        return interest;
    }
   
}