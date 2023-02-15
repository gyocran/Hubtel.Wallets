using AutoMapper;
using Hubtel.Wallets.Api.DTOs;
using Hubtel.Wallets.Api.Models;

namespace Hubtel.Wallets.Api.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Wallet, WalletDto>()
                .ForMember(d => d.Type, opt => opt.MapFrom(src => src.AccountType.Type))
                .ForMember(d => d.AccountScheme, opt => opt.MapFrom(src => src.AccountScheme.Scheme))
                .ReverseMap()
                .ForPath(s => s.AccountScheme.Scheme, opt => opt.MapFrom(src => src.AccountScheme));
        }
    }
}
