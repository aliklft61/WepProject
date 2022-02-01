using AutoMapper;
using ItServiceApp.Models.Payment;
using Iyzipay.Model;

namespace ItServiceApp.MapperProfiles
{
    public class PaymentProfile : Profile
    {
        public PaymentProfile()
        {
            CreateMap<CardModel, PaymentCard>().ReverseMap();
            CreateMap<BasketModel, BasketItem>().ReverseMap();
            CreateMap<AddressModel, Address>().ReverseMap();
            CreateMap<CustomerModel, Buyer>().ReverseMap();
            CreateMap<InstallmentPriceModel, InstallmentPrice>().ReverseMap();
            //CreateMap<InstallmentModel,InstallmentInfo >().ForMember(dest => dest.InstallmentDetails,opt => opt.MapFrom(src => src.InstallmentPrices)).ReverseMap();
            CreateMap<InstallmentModel, InstallmentDetail>().ReverseMap();
            CreateMap<PaymentResponseModel, Payment>().ReverseMap();
        }
    }
}