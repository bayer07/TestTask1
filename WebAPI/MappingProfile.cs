using AutoMapper;
using Domain.Interfaces;
using Domain.Transactions;
using WebAPI.Models;

namespace WebAPI
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreditTransactionModel, CreditTransaction>();
            CreateMap<DebitTransactionModel, DebitTransaction>()
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => -src.Amount));
            CreateMap<ITransactionResult, TransactionModel>()
                .ForMember(dest => dest.InsertDateTime, opt => opt.MapFrom(src => src.DateTime))
                .ForMember(dest => dest.ClientBalance, opt => opt.MapFrom(src => src.Balance));
            CreateMap<ITransactionResult, RevertTransactionModel>()
                .ForMember(dest => dest.RevertDateTime, opt => opt.MapFrom(src => src.DateTime))
                .ForMember(dest => dest.ClientBalance, opt => opt.MapFrom(src => src.Balance));
            CreateMap<ITransactionResult, BalanceModel>()
                .ForMember(dest => dest.BalanceDateTime, opt => opt.MapFrom(src => src.DateTime))
                .ForMember(dest => dest.ClientBalance, opt => opt.MapFrom(src => src.Balance));
        }
    }
}
