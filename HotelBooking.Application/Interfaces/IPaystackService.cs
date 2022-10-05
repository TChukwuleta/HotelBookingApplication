using HotelBooking.Application.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Application.Interfaces
{
    public interface IPaystackService
    {
        Task<PaystackPaymentResponse> VerifyTransactionStatus(PaymentIntentVm paymentIntentVm);
        Task<PaystackPaymentResponse> InitializeTransaction(PaymentIntentVm paymentIntentVm);
    }
}
