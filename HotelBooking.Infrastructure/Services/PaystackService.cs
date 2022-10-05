using HotelBooking.Application.Interfaces;
using HotelBooking.Application.Model;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Infrastructure.Services
{
    public class PaystackService : IPaystackService
    {
        private readonly IConfiguration _config;
        private readonly IApiClientService _apiClientService;
        public PaystackService(IConfiguration config, IApiClientService apiClientService)
        {
            _config = config;
            _apiClientService = apiClientService;
        }

        public async Task<PaystackPaymentResponse> InitializeTransaction(PaymentIntentVm paymentIntentVm)
        {
            try
            {
                string key = _config["Paystack:Key"];
                string url = _config["Paystack:Url"] + "transaction/initialize";
                var payObject = new
                {
                    email = paymentIntentVm.Email,
                    amount = paymentIntentVm.Amount.ToString(),
                    currency = paymentIntentVm.CurrencyCode,
                    reference = paymentIntentVm.ClientReferenceId,
                };
                var aPIRequestDto = new ApiRequestDto
                {
                    ApiKey = key,
                    ApiUrl = url,
                    requestObject = payObject
                };

                var apiResult = await _apiClientService.Post(aPIRequestDto);
                var result = JsonConvert.DeserializeObject<PaystackPaymentResponse>(apiResult);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<PaystackPaymentResponse> VerifyTransactionStatus(PaymentIntentVm paymentIntentVm)
        {
            try
            {
                string key = _config["Paystack:Key"];
                if (paymentIntentVm.ClientReferenceId != null)
                {
                    var referenceId = paymentIntentVm.ClientReferenceId;
                    string apiUrl = _config["Paystack:Url"] + "transaction/verify/" + referenceId;
                    var aPIRequestDto = new ApiRequestDto
                    {
                        ApiKey = key,
                        ApiUrl = apiUrl
                    };
                    var response = await _apiClientService.Get<PaystackPaymentResponse>(aPIRequestDto);
                    return response;
                }
                throw new Exception("Invalid Client Reference Id");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
