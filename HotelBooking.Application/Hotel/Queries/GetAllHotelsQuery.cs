using HotelBooking.Application.Interfaces;
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
    }

    public class GetAllHotelsQueryHandler : IRequestHandler<GetAllHotelsQuery, Result>
    {
        private readonly IAppDbContext _context;
        public GetAllHotelsQueryHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<Result> Handle(GetAllHotelsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var hotels = await _context.Hotels.ToListAsync();
                if(hotels.Count <= 0)
                {
                    return Result.Failure("No hotel available");
                }
                if(request.Skip == 0 && request.Take == 0)
                {
                    var allHotels = new
                    {
                        Hotels = hotels,
                        Count = hotels.Count
                    };
                    return Result.Success("Hotels retrieval was successful", allHotels);
                }
                var prunedHotels = new
                {
                    Hotels = hotels.Skip(request.Skip).Take(request.Take).ToList(),
                    Count = hotels.Count
                };
                return Result.Success("Hotels retrieval was successful", prunedHotels); 
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
