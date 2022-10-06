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
    public class GetHotelByIdQuery : IRequest<Result>
    {
        public int HotelId { get; set; }
    } 
    public class GetHotelByIdQueryHandler : IRequestHandler<GetHotelByIdQuery, Result>
    {
        private readonly IHotelRepository _hotelRepository;
        public GetHotelByIdQueryHandler(IHotelRepository hotelRepository)
        {
            _hotelRepository = hotelRepository;
        }

        public async Task<Result> Handle(GetHotelByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var hotel = await _hotelRepository.GetByIdAsync(request.HotelId);
                if(hotel == null)
                {
                    return Result.Failure("Invalid Hotel selected");
                }
                return Result.Success("Hotel retrieval was successful", hotel);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Getting hotel by Id was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}  
