using System.ComponentModel.DataAnnotations.Schema;

namespace Deposito.DB.Models;

public class Interest
{
    public string Id { get; set; }

    public string Description { get; set; }

    public double Percent { get; set; }

    public int PeriodInMonths { get; set; }

    public Deposit Deposit { get; set; }
    
    [ForeignKey("Deposit")]
    public string DepositId { get; set; }
}