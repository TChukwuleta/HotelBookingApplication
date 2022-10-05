using HotelBooking.Application.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Application.Interfaces
{
    public interface IApiClientService
    {
        Task<string> Get(ApiRequestDto request);
        Task<T> Get<T>(ApiRequestDto request);
        Task<string> Post(ApiRequestDto request);
        Task<T> Post<T>(ApiRequestDto request);
    }
}
