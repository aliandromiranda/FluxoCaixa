using Xunit;
using FluxoCaixa.Core.DTOs;
using FluxoCaixa.Core.Enums;
using FluxoCaixa.Infrastructure.Services;
using FluxoCaixa.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace FluxoCaixa.Tests
{
    public class TransactionServiceTests
    {
        [Fact(DisplayName = "Deve criar um lançamento com sucesso")]
        public async Task Deve_Criar_Lancamento_Com_Sucesso()
        {            
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("FluxoCaixaTestDB")
                .Options;

            using var db = new AppDbContext(options);

            // Mock 
            var healthMock = new Mock<IConsolidatedHealthService>();
            healthMock.Setup(h => h.IsAvailableAsync()).ReturnsAsync(true);

            var service = new TransactionService(db, healthMock.Object);

            var dto = new TransactionDto
            {
                Date = DateTime.Now,
                Amount = 100,
                Type = TransactionType.Credit,
                Description = "Venda teste"
            };

            
            var result = await service.AddAsync(dto);

           
            Assert.NotNull(result);
            Assert.Equal(100, result.Amount);
            Assert.Equal(TransactionType.Credit, result.Type);
            Assert.NotEqual(Guid.Empty, result.Id);
        }

        [Fact(DisplayName = "Deve falhar se o relatorio consolidado estiver fora do ar")]
        public async Task Deve_Falhar_Quando_Consolidado_Indisponivel()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("FluxoCaixaTestDB2")
                .Options;

            using var db = new AppDbContext(options);

            // Simulando indisponibilidade
            var healthMock = new Mock<IConsolidatedHealthService>();
            healthMock.Setup(h => h.IsAvailableAsync()).ReturnsAsync(false);

            var service = new TransactionService(db, healthMock.Object);

            var dto = new TransactionDto
            {
                Date = DateTime.Now,
                Amount = 200,
                Type = TransactionType.Debit,
                Description = "Pagamento teste"
            };

            await Assert.ThrowsAsync<InvalidOperationException>(() => service.AddAsync(dto));
        }
    }
}
