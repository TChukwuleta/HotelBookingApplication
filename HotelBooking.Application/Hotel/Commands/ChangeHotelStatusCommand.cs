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

namespace HotelBooking.Application.Hotel.Commands
{
    public class ChangeHotelStatusCommand : IRequest<Result>
    {
        public int HotelId { get; set; }
    }

    public class ChangeHotelStatusCommandHandler : IRequestHandler<ChangeHotelStatusCommand, Result>
    {
        private readonly IAppDbContext _context;
        public ChangeHotelStatusCommandHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<Result> Handle(ChangeHotelStatusCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var hotel = await _context.Hotels.FirstOrDefaultAsync(c => c.Id == request.HotelId);
                if(hotel == null)
                {
                    return Result.Failure("Invalid hotel selected");
                }
                string message = default;
                switch (hotel.Status)
                {
                    case Domain.Enums.Status.Available:
                        hotel.Status = Status.NotAvailable;
                        hotel.StatusDesc = Status.NotAvailable.ToString();
                        message = "Hotel is now not available";
                        break;
                    case Domain.Enums.Status.UnderReview:
                        hotel.Status = Status.Available;
                        hotel.StatusDesc = Status.Available.ToString();
                        message = "Hotel is now available";
                        break;
                    case Domain.Enums.Status.NotAvailable:
                        hotel.Status = Status.Available;
                        hotel.StatusDesc = Status.Available.ToString();
                        message = "Hotel is now available";
                        break;
                    default:
                        break;
                }
                await _context.Hotels.AddAsync(hotel);
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success(message);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
