using AutoFixture;
using Common.Domain.Interfaces;
using ContratacaoApi.Application.Interfaces;
using ContratacaoApi.Application.Interfaces.Dto;
using ContratacaoApi.Domain.Entities;
using ContratacaoApi.Domain.Services;
using Moq;

namespace ContratacaoApi.Tests
{
    public class ContratacaoServiceTests
    {
        private readonly Mock<IContratacaoRepository> _repoMock;
        private readonly Mock<IPropostaServiceAgent> _agentMock;
        private readonly Mock<IUnitOfWork> _unitMock;
        private readonly ContratacaoService _service;
        private readonly Fixture _fixture;

        public ContratacaoServiceTests()
        {
            _repoMock = new();
            _agentMock = new();
            _unitMock = new();
            _service = new ContratacaoService(_repoMock.Object, _agentMock.Object, _unitMock.Object);
            _fixture = new();
        }

        [Fact]
        public async Task ContratarPropostaAsync_DeveCriarContratacao_QuandoPropostaAprovada()
        {
            // Arrange
            var propostaId = 1;
            var proposta = new PropostaDto { Id = propostaId, Status = PropostaStatus.Aprovada };
            _agentMock.Setup(a => a.GetPropostaAsync(propostaId)).ReturnsAsync(proposta);

            // Act
            var result = await _service.ContratarPropostaAsync(propostaId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(propostaId, result.PropostaId);

            _unitMock.Verify(u => u.Commit(), Times.Once);
            _repoMock.Verify(r => r.AddAsync(It.IsAny<Contratacao>()), Times.Once);
        }

        [Fact]
        public async Task ContratarPropostaAsync_DeveLancarExcecao_QuandoPropostaNaoExiste()
        {
            // Arrange
            var propostaId = 2;
            _agentMock.Setup(a => a.GetPropostaAsync(propostaId)).ReturnsAsync((PropostaDto?)null);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.ContratarPropostaAsync(propostaId));
            Assert.Contains($"Proposta com ID {propostaId} não encontrada.", ex.Message);
            _unitMock.Verify(u => u.Commit(), Times.Never);
        }

        [Fact]
        public async Task ContratarPropostaAsync_DeveLancarExcecao_QuandoPropostaNaoAprovada()
        {
            // Arrange
            var propostaId = 3;
            var proposta = new PropostaDto { Id = propostaId, Status = PropostaStatus.EmAnalise };
            _agentMock.Setup(a => a.GetPropostaAsync(propostaId)).ReturnsAsync(proposta);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => _service.ContratarPropostaAsync(propostaId));
            Assert.Equal("Proposta não está aprovada.", ex.Message);
            _unitMock.Verify(u => u.Commit(), Times.Never);
        }

        [Fact]
        public async Task GetContratacaoByIdAsync_DeveRetornarContratacao_QuandoExiste()
        {
            // Arrange
            var contratacaoId = 10;
            var contratacao = _fixture.Create<Contratacao>();
            _repoMock.Setup(r => r.GetByIdAsync(contratacaoId)).ReturnsAsync(contratacao);

            // Act
            var result = await _service.ObterPorIdAsync(contratacaoId);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetContratacaoByIdAsync_DeveRetornarNull_QuandoNaoExiste()
        {
            // Arrange
            var contratacaoId = 99;
            _repoMock.Setup(r => r.GetByIdAsync(contratacaoId)).ReturnsAsync((Contratacao?)null);

            // Act
            var result = await _service.ObterPorIdAsync(contratacaoId);

            // Assert
            Assert.Null(result);
        }
    }
}