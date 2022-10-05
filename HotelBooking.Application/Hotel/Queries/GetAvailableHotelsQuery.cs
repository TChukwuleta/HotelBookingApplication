using HotelBooking.Application.Interfaces;
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
        private readonly IAppDbContext _context;
        public GetAvailableHotelsQueryHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<Result> Handle(GetAvailableHotelsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var avaialbleHotels = await _context.Hotels.Where(c => c.Status == Status.Available).ToListAsync();
                if(avaialbleHotels.Count <= 0)
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
