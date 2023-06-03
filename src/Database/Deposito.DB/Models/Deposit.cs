using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Deposito.DB.Enums;

namespace Deposito.DB.Models;

public class Deposit
{
    [Key]
    public string Id { get; set; }

    [ForeignKey("Bank")]
    public string BankId { get; set; }
    
    public Bank Bank { get; set; }

    public string Code { get; set; }

    public string Description { get; set; }

    public double MinimalAmount { get; set; }

    public double MaximumAmount { get; set; }

    public Currency Currency { get; set; }

    public ICollection<Interest> Interests { get; set; }

    public PayoutType Type { get; set; }

    public DateOnly? PayoutDay { get; set; }
}