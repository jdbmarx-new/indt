using AutoMapper;
using PropostaApi.Domain.Entities;
using PropostaApi.Dto;

namespace PropostaApi.Infrastructure
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<CadastroPropostaDto, Proposta>();
        }
    }
}