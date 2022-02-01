using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ItServiceApp.Models.Payment;

namespace ItServiceApp.Services
{
    public interface IPaymentService
    {
        public PaymentResponseModel Pay(PaymentModel model);
        public InstallmentModel CheckInstallment(string binNumber,decimal price);
    }
}
