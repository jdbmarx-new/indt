using AutoFixture;
using AutoMapper;
using Common.Domain.Interfaces;
using Moq;
using PropostaApi.Application.Interfaces;
using PropostaApi.Domain.Entities;
using PropostaApi.Domain.Services;
using PropostaApi.Dto;

namespace PropostaApi.Tests
{
    public class PropostaServiceTests
    {
        private readonly Mock<IPropostaRepository> _repoMock = new();
        private readonly Mock<IUnitOfWork> _unitMock = new();
        private readonly PropostaService _service;
        private readonly Fixture _fixture = new();

        public PropostaServiceTests()
        {
            IMapper mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddMaps(typeof(PropostaService).Assembly);
            }).CreateMapper();
            _service = new PropostaService(_repoMock.Object, _unitMock.Object, mapper);
        }

        [Fact]
        public async Task CriarAsync_DeveAdicionarECommitar()
        {
            var propostaDto = _fixture.Create<CadastroPropostaDto>();

            var proposta = await _service.CriarAsync(propostaDto);

            _repoMock.Verify(r => r.AddAsync(proposta), Times.Once);
            _unitMock.Verify(u => u.Commit(), Times.Once);
        }

        [Fact]
        public async Task AlterarStatusAsync_DeveAlterarStatus_QuandoPropostaExiste()
        {
            var proposta = _fixture.Create<Proposta>();
            proposta.Status = PropostaStatus.EmAnalise;

            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(proposta);

            var result = await _service.AlterarStatusAsync(1, PropostaStatus.Aprovada);

            Assert.Equal(PropostaStatus.Aprovada, result?.Status);
            _unitMock.Verify(u => u.Commit(), Times.Once);
        }

        [Fact]
        public async Task AlterarStatusAsync_DeveRetornarNull_QuandoPropostaNaoExiste()
        {
            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Proposta?)null);

            var result = await _service.AlterarStatusAsync(1, PropostaStatus.Aprovada);

            Assert.Null(result);
            _unitMock.Verify(u => u.Commit(), Times.Never);
        }
    }
}