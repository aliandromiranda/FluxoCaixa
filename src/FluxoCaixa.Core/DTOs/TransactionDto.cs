using System;
using FluxoCaixa.Core.Enums;

namespace FluxoCaixa.Core.DTOs
{
    public class TransactionDto
    {
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public TransactionType Type { get; set; }
        public string? Description { get; set; }
    }
}