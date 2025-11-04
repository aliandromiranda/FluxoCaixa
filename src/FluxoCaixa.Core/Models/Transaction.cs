using System;
using FluxoCaixa.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace FluxoCaixa.Core.Models
{
    public class Transaction
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public TransactionType Type { get; set; }
        public string? Description { get; set; }
    }
}