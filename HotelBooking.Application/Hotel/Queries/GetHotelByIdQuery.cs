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
    public class GetHotelByIdQuery : IRequest<Result>
    {
        public int HotelId { get; set; }
    } 
    public class GetHotelByIdQueryHandler : IRequestHandler<GetHotelByIdQuery, Result>
    {
        private readonly IAppDbContext _context;
        public GetHotelByIdQueryHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(GetHotelByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var hotel = await _context.Hotels.Include(c => c.Facility).FirstOrDefaultAsync(c => c.Id == request.HotelId);
                if(hotel == null)
                {
                    return Result.Failure("Invalid Hotel selected");
                }
                return Result.Success("Hotel retrieval was successful", hotel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}  
