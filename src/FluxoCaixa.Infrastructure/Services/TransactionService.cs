using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FluxoCaixa.Core.Models;
using FluxoCaixa.Core.DTOs;
using FluxoCaixa.Core.Enums;
using FluxoCaixa.Infrastructure.Data;

namespace FluxoCaixa.Infrastructure.Services
{

    public class DailyConsolidated
    {
        public DateTime Date { get; set; }
        public decimal DayTotal { get; set; }
        public decimal CumulativeBalance { get; set; }
    }

    public interface ITransactionService
    {
        Task<Transaction> AddAsync(TransactionDto dto);
        Task<IEnumerable<Transaction>> GetAsync(DateTime? from, DateTime? to);
        Task<IEnumerable<DailyConsolidated>> GetConsolidatedAsync(DateTime from, DateTime to, decimal initialBalance);
    }

    public class TransactionService : ITransactionService
    {
        private readonly IConsolidatedHealthService _healthService;

        public TransactionService(AppDbContext db, IConsolidatedHealthService healthService)
        {
            _db = db;
            _healthService = healthService;
        }

        public async Task<Transaction> AddAsync(TransactionDto dto)
        {
            var healthy = await _healthService.IsAvailableAsync();
            if (!healthy)
                throw new InvalidOperationException("Serviço de consolidado indisponível. Tente novamente mais tarde.");

            var entity = new Transaction
            {
                Id = Guid.NewGuid(),
                Date = dto.Date,
                Amount = dto.Amount,
                Type = dto.Type,
                Description = dto.Description
            };
            _db.Transactions.Add(entity);
            await _db.SaveChangesAsync();
            return entity;
        }


        private readonly AppDbContext _db;

        public TransactionService(AppDbContext db)
        {
            _db = db;
        }

       
        public async Task<IEnumerable<Transaction>> GetAsync(DateTime? from, DateTime? to)
        {
            var q = _db.Transactions.AsQueryable();
            if (from.HasValue) q = q.Where(t => t.Date >= from.Value.Date);
            if (to.HasValue) q = q.Where(t => t.Date <= to.Value.Date.AddDays(1).AddTicks(-1));
            return await q.OrderBy(t => t.Date).ToListAsync();
        }

        public async Task<IEnumerable<DailyConsolidated>> GetConsolidatedAsync(DateTime from, DateTime to, decimal initialBalance)
        {
            DateTime start = from.Date;
            DateTime end = to.Date;
            var txns = await _db.Transactions
                .Where(t => t.Date.Date >= start && t.Date.Date <= end)
                .ToListAsync();

            var grouped = txns
                .GroupBy(t => t.Date.Date)
                .Select(g => new {
                    Date = g.Key,
                    DayTotal = g.Sum(t => t.Type == TransactionType.Credit ? t.Amount : -t.Amount)
                })
                .ToDictionary(x => x.Date, x => x.DayTotal);

            var result = new List<DailyConsolidated>();
            decimal cumulative = initialBalance;
            for (var date = start; date <= end; date = date.AddDays(1))
            {
                decimal dayTotal = grouped.ContainsKey(date) ? grouped[date] : 0m;
                cumulative += dayTotal;
                result.Add(new DailyConsolidated { Date = date, DayTotal = dayTotal, CumulativeBalance = cumulative });
            }

            return result;
        }
    }
}