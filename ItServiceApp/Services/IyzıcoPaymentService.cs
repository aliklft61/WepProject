using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ItServiceApp.Models;
using ItServiceApp.Models.Identity;
using ItServiceApp.Models.Payment;
using Iyzipay.Model;
using Iyzipay.Request;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using MUsefullMethods;

namespace ItServiceApp.Services
{
    public class IyzicoPaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly IyzicoPaymentOptions _options;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        public IyzicoPaymentService(IConfiguration configuration, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _configuration = configuration;
            _mapper = mapper;
            _userManager = userManager;
            var section = _configuration.GetSection(IyzicoPaymentOptions.Key);
            _options = new IyzicoPaymentOptions()
            {
                ApiKey = section["ApiKey"],
                SecretKey = section["SecretKey"],
                BaseUrl = section["BaseUrl"],
                ThreedsCallbackUrl = section["ThreedsCallbackUrl"],
            };
        }
        public PaymentResponseModel Pay(PaymentModel model)
        {
            var request = this.InitialPaymentRequest(model);
            var payment = Payment.Create(request, _options);

            return _mapper.Map<PaymentResponseModel>(payment);
        }
        private CreatePaymentRequest InitialPaymentRequest(PaymentModel model)
        {

            var paymentRequest = new CreatePaymentRequest();
            paymentRequest.Installment =(int)model.Installment;
            paymentRequest.Locale = Locale.TR.ToString();
            paymentRequest.ConversationId = GenerateConversationId();
            paymentRequest.Price =model.Price.ToString(new CultureInfo("en-US"));
            paymentRequest.PaidPrice =model.PaidPrice.ToString(new CultureInfo("en-US"));
            paymentRequest.Currency = Currency.TRY.ToString();
            paymentRequest.BasketId = StringHelpers.GenerateUniqueCode();
            paymentRequest.PaymentChannel = PaymentChannel.WEB.ToString();
            paymentRequest.PaymentGroup = PaymentGroup.SUBSCRIPTION.ToString();


            var user = _userManager.FindByIdAsync(model.UserId).Result;

            Buyer buyer = new Buyer();
            buyer.Id = user.Id;
            buyer.Name = user.Name;
            buyer.Surname = user.Surname;
            buyer.GsmNumber = user.PhoneNumber;
            buyer.Email = user.Email;
            buyer.IdentityNumber = "11111111110";
            buyer.LastLoginDate = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}";
            buyer.RegistrationDate = $"{user.CreatedDate:yyyy-MM-dd HH:mm:ss}";
            buyer.RegistrationAddress = "Wissen parti yeri beşiktaş";
            buyer.Ip = model.Ip;
            buyer.City = "Istanbul";
            buyer.Country = "Turkey";
            buyer.ZipCode = "34732";
            paymentRequest.Buyer = buyer;

            Address billingAddres = new Address();
            billingAddres.ContactName = $"{user.Name}{user.Surname}";
            billingAddres.City = "istanbul";
            billingAddres.Country = "turkey";
            billingAddres.Description = "Wissen parti yeri beşiktaş";
            billingAddres.ZipCode = "34742";
            paymentRequest.BillingAddress = billingAddres;

            var basketItems = new List<BasketItem>();
            var firstBasketItem = new BasketItem()
            {
                Id = "BI101",
                Name = "binoncular",
                Category1 = "collections",
                Category2 = "accessories",
                ItemType = BasketItemType.VIRTUAL.ToString(),
                Price = model.Price.ToString(new CultureInfo("en-US"))
            };
            basketItems.Add(firstBasketItem);
            paymentRequest.BasketItems = basketItems;

            paymentRequest.PaymentCard = _mapper.Map<PaymentCard>(model.CardModel);

            return paymentRequest;
        }

        public InstallmentModel CheckInstallment(string binNumber, decimal price)
        {
            var conversationId = GenerateConversationId();

            var request = new RetrieveInstallmentInfoRequest
            {
                Locale = Locale.TR.ToString(),
                ConversationId = conversationId,
                BinNumber = binNumber,
                Price = price.ToString(new CultureInfo("en-US"))
            };
            var result = InstallmentInfo.Retrieve(request, _options);
            if (result.Status == "failure")
            {
                
                throw new Exception(result.ErrorMessage);
            }

            if (result.ConversationId != conversationId)
            {
                throw new Exception("Hatalı istek oluştu!");

            }

            var resq = result.InstallmentDetails[0];
            InstallmentModel resultModel = _mapper.Map<InstallmentModel>(resq);
            
            
            return resultModel;
        }

        private string GenerateConversationId()
        {
            return StringHelpers.GenerateUniqueCode();
        }

        
    }
}
