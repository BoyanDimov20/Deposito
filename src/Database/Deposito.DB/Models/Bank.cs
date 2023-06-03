using System.ComponentModel.DataAnnotations;

namespace Deposito.DB.Models;

public class Bank
{
    [Key]
    public string Id { get; set; }

    [Required]
    public string Name { get; set; }

    public string IconUrl { get; set; }

    public ICollection<Deposit> Deposits { get; set; }
}