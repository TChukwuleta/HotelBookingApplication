using HotelBooking.Application.Interfaces;
using HotelBooking.Application.Model;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Infrastructure.Services
{
    public class ApiClientService : IApiClientService
    {
        public ApiClientService()
        {
        }

        public Task<string> Get(ApiRequestDto request)
        {
            throw new NotImplementedException();
        }

        public async Task<T> Get<T>(ApiRequestDto request)
        {
            try
            {
                RestClient client = new RestClient();
                RestRequest restRequest = new RestRequest(request.ApiUrl);
                if (!string.IsNullOrEmpty(request.ApiKey))
                {
                    restRequest.AddHeader("Accept", "application/json");
                    restRequest.AddHeader("Authorization", "Bearer " + request.ApiKey);
                }
                ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
                var response = await client.GetAsync<T>(restRequest);
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<string> Post(ApiRequestDto request)
        {
            try
            {
                var client = new RestClient(request.ApiUrl);
                RestRequest restRequest = new RestRequest(request.ApiUrl, Method.Post);
                if (!string.IsNullOrEmpty(request.ApiKey))
                {
                    restRequest.AddHeader("Accept", "application/json");
                    restRequest.AddHeader("Authorization", "Bearer " + request.ApiKey);
                }
                restRequest.AddJsonBody(request.requestObject);
                RestResponse restResponse = await client.ExecuteAsync(restRequest);
                var responseContent = restResponse.Content;
                return responseContent;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Task<T> Post<T>(ApiRequestDto request)
        {
            throw new NotImplementedException();
        }
    }
}
