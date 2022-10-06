using HotelBooking.Application.Interfaces;
using HotelBooking.Application.Interfaces.IRepositories;
using HotelBooking.Application.Model;
using HotelBooking.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Application.Hotel.Queries
{
    public class GetAvailableHotelsQuery : IRequest<Result>
    {

    }
    public class GetAvailableHotelsQueryHandler : IRequestHandler<GetAvailableHotelsQuery, Result>
    {
        private readonly IHotelRepository _hotelRepository;
        public GetAvailableHotelsQueryHandler(IAppDbContext context, IHotelRepository hotelRepository)
        {
            _hotelRepository = hotelRepository;
        }
        public async Task<Result> Handle(GetAvailableHotelsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var allHotels = await _hotelRepository.GetAllAsync();
                if(allHotels.Count <= 0 || allHotels == null)
                {
                    return Result.Failure("No hotel available");
                }
                var avaialbleHotels = allHotels.Where(c => c.Status == Status.Available).ToList();
                if(avaialbleHotels.Count <= 0 || avaialbleHotels == null)
                {
                    return Result.Failure("No available hotel at the moment. Please, try again later");
                }
                return Result.Success("Retrieval of available hotels was successful", avaialbleHotels);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
