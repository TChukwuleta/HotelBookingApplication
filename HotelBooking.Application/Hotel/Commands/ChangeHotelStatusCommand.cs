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

namespace HotelBooking.Application.Hotel.Commands
{
    public class ChangeHotelStatusCommand : IRequest<Result>
    {
        public int HotelId { get; set; }
    }

    public class ChangeHotelStatusCommandHandler : IRequestHandler<ChangeHotelStatusCommand, Result>
    {
        private readonly IHotelRepository _hotelRepository;
        public ChangeHotelStatusCommandHandler(IHotelRepository hotelRepository)
        {
            _hotelRepository = hotelRepository;
        }
        public async Task<Result> Handle(ChangeHotelStatusCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var hotel = await _hotelRepository.GetByIdAsync(request.HotelId);
                if(hotel == null)
                {
                    return Result.Failure("Invalid hotel selected");
                }
                string message = default;
                switch (hotel.Status)
                {
                    case Status.Available:
                        hotel.Status = Status.NotAvailable;
                        hotel.StatusDesc = Status.NotAvailable.ToString();
                        message = "Hotel is now not available";
                        break;
                    case Status.UnderReview:
                        hotel.Status = Status.Available;
                        hotel.StatusDesc = Status.Available.ToString();
                        message = "Hotel is now available";
                        break;
                    case Status.NotAvailable:
                        hotel.Status = Status.Available;
                        hotel.StatusDesc = Status.Available.ToString();
                        message = "Hotel is now available";
                        break;
                    default:
                        break;
                }
                await _hotelRepository.UpdateAsync(hotel);
                return Result.Success(message, hotel);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Changing hotel status was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
