using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Domain.Interfaces;

namespace Domain.Transactions
{
    [Index(nameof(Id), IsUnique = true)]
    [Index(nameof(ClientId), IsUnique = false)]
    public class Transaction : ITransaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public Guid ClientId { get; set; }

        public DateTime DateTime { get; set; }

        public decimal Amount { get; set; }

        public DateTime? RevertDateTime { get; set; }
    }
}
