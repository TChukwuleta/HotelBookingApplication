using HotelBooking.Application.Interfaces;
using HotelBooking.Application.Interfaces.IRepositories;
using HotelBooking.Application.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Application.Hotel.Queries
{
    public class GetAllHotelsQuery : IRequest<Result>
    {
        public int Skip { get; set; }
        public int Take { get; set; }
        public string? SearchValue { get; set; }
    }

    public class GetAllHotelsQueryHandler : IRequestHandler<GetAllHotelsQuery, Result>
    {
        private readonly IHotelRepository _hotelRepository;
        public GetAllHotelsQueryHandler(IHotelRepository hotelRepository)
        {
            _hotelRepository = hotelRepository;
        }
        public async Task<Result> Handle(GetAllHotelsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var hotels = await _hotelRepository.GetAllAsync();
                if(hotels == null)
                {
                    return Result.Failure("No hotel available");
                }
                if (!string.IsNullOrEmpty(request.SearchValue))
                {
                    hotels = hotels.Where(c => c.Name.ToLower().Contains(request.SearchValue) || c.Description.ToLower().Contains(request.SearchValue) || c.Address.ToLower().Contains(request.SearchValue)).ToList();
                    if(hotels.Count <= 0 | hotels == null)
                    {
                        return Result.Failure("No hotel found based on the search");
                    }
                    return Result.Success("Hotel retrieval based on search was successful", hotels);
                }
                if(request.Skip == 0 && request.Take == 0)
                {
                    return Result.Success("Hotels retrieval was successful", hotels);
                }
                return Result.Success("Hotels retrieval was successful", hotels.Skip(request.Skip).Take(request.Take).ToList()); 
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "All hotels retrieval was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
