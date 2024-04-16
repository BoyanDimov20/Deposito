using Deposito.DB;
using Deposito.DB.Enums;
using Deposito.Web.Services.Dtos;
using ExcelLibrary.SpreadSheet;
using Microsoft.EntityFrameworkCore;

namespace Deposito.Web.Services;

public class DepositService : IDepositService, IDisposable, IAsyncDisposable
{
    private readonly AppDbContext _appDbContext;
    private readonly MemoryStream _memoryStream;

    public DepositService(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
        _memoryStream = new MemoryStream();
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
                        && (x.Deposit.Type == payoutType))
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
            .Include(x => x.Deposit)
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
                AmountAfterDeposit = amount + (amount * (x.Percent / 100)),
                PayoutType = x.Deposit.Type
            })
            .FirstOrDefaultAsync();

        
        if (interest is null)
        {
            throw new ArgumentNullException(nameof(interest));
        }
        
        if (interest.PayoutType == PayoutType.Monthly)
        {
            interest.AmountAfterDeposit = amount + (amount * (interest.Percent / 100));
        }
        else if(interest.PayoutType == PayoutType.Yearly)
        {
            interest.AmountAfterDeposit = amount * Math.Pow(1 + (interest.Percent / 100), ((double)interest.Period/12));
        }
        else
        {
            //TODO: In the end payout
        }

        return interest;
    }

    public async Task<DepositTable> FindMonthlyPaidDeposit(double amount, double interest, int period)
    {
        double sumInterest = interest * period;
        double firstInterest = sumInterest / (10 * (double)period);
        double lastInterest = (interest * 2) - firstInterest;
        double delimeter = (lastInterest - firstInterest) / (period - 1);

        var interests = new List<double>();
        var monthlyDeposits = new List<double>();
        var totalIncome = new List<double>();

       
        for (int i = 1; i < period + 1; i++)
        {
            var currentInterest = firstInterest + ((i - 1) * delimeter);
            interests.Add(currentInterest);

            var monthDeposit = ((currentInterest / 100) * amount) / period;
            monthlyDeposits.Add(monthDeposit);
            totalIncome.Add(monthlyDeposits.Sum() + amount);
        }

        return new DepositTable
        {
            Interests = interests,
            InterestIncomes = monthlyDeposits,
            NetIncomes = totalIncome,
            Amounts = Enumerable.Repeat(amount, period).ToList()
        };
    }


    public async Task<DepositTable> FindYearlyPaidDeposit(double amount, double interest, int period)
    {
        var interests = new List<double>();
        var monthlyDeposits = new List<double>();
        var totalIncome = new List<double>();

        var currentAmount = amount;
        for (int i = 1; i < period + 1; i++)
        {
            if (i % 12 == 0)
            {
                interests.Add(interest);
                var monthDeposit = (interest / 100) * (currentAmount);
                monthlyDeposits.Add(monthDeposit);
                
                currentAmount = amount + monthlyDeposits.Sum();
                totalIncome.Add(currentAmount);
            }
            else
            {
                interests.Add(0);
                monthlyDeposits.Add(0);
                totalIncome.Add(currentAmount);
            }
            

           
        }

        return new DepositTable
        {
            Interests = interests,
            InterestIncomes = monthlyDeposits,
            NetIncomes = totalIncome,
            Amounts = Enumerable.Repeat(amount, period).ToList()
        };
    }

    public async Task<DepositTable> FindInTheEndPaidDeposit(double amount, double interest, int period)
    {
        return await FindYearlyPaidDeposit(amount, interest, period);
    }

    public async Task<DepositTable> FindDepositByType(double amount, double interest, int period, PayoutType payoutType)
    {
        switch (payoutType)
        {
            case PayoutType.Yearly:
                return await FindYearlyPaidDeposit(amount, interest, period);
            case PayoutType.InEnd:
                return await FindInTheEndPaidDeposit(amount, interest, period);
            case PayoutType.Monthly:
            default:
                return await FindMonthlyPaidDeposit(amount, interest, period);

        }
    }
    public async Task<Stream> GenerateExcel(double amount, double interest, int period, PayoutType payoutType)
    {
        var depositTable = await FindDepositByType(amount, interest, period, payoutType);

        var workbook = new Workbook();
        
        Worksheet worksheet = new Worksheet("Deposit Plan");
        worksheet.Cells[0, 1] = new Cell("Депозирана сума");
        worksheet.Cells[0, 2] = new Cell("Лихва");
        worksheet.Cells[0, 3] = new Cell("Вноска лихва");
        worksheet.Cells[0, 4] = new Cell("Нетно изплатени");
        workbook.Worksheets.Add(worksheet);

        worksheet.Cells.ColumnWidth[0, 1] = 5000;
        worksheet.Cells.ColumnWidth[0, 3] = 5000;
        worksheet.Cells.ColumnWidth[0, 4] = 5000;
        for (int i = 0; i < depositTable.Amounts.Count; i++)
        {
            worksheet.Cells[i + 1, 1] = new Cell(Math.Round(depositTable.Amounts[i], 2));
        }

        for (int i = 0; i < depositTable.Interests.Count; i++)
        {
            worksheet.Cells[i + 1, 2] = new Cell(Math.Round(depositTable.Interests[i], 2));
        }

        for (int i = 0; i < depositTable.InterestIncomes.Count; i++)
        {
            worksheet.Cells[i + 1, 3] = new Cell(Math.Round(depositTable.InterestIncomes[i], 2));
        }

        for (int i = 0; i < depositTable.NetIncomes.Count; i++)
        {
            worksheet.Cells[i + 1, 4] = new Cell(Math.Round(depositTable.NetIncomes[i], 2));
        }
        //workbook.SaveToStream(_memoryStream);
        workbook.Save(_memoryStream);
        _memoryStream.Position = 0;
        
        return _memoryStream;
    }

    public void Dispose()
    {
        _appDbContext.Dispose();
        _memoryStream.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await _appDbContext.DisposeAsync();
        await _memoryStream.DisposeAsync();
    }
}