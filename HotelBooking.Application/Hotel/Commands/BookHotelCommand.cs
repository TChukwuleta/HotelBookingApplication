using HotelBooking.Application.Interfaces;
using HotelBooking.Application.Interfaces.IRepositories;
using HotelBooking.Application.Model;
using HotelBooking.Domain.Entities;
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
    public class BookHotelCommand : IRequest<Result>
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public decimal Amount { get; set; }
        public int HotelId { get; set; }
    }
    public class BookHotelCommandHandler : IRequestHandler<BookHotelCommand, Result>
    {
        private readonly IHotelRepository _hotelRepository;
        private readonly IBookingService _bookingService;
        public BookHotelCommandHandler(IHotelRepository hotelRepository, IBookingService bookingService)
        {
            _hotelRepository = hotelRepository;
            _bookingService = bookingService;
        }
        public async Task<Result> Handle(BookHotelCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var hotel = await _hotelRepository.GetByIdAsync(request.HotelId);
                if(hotel == null)
                {
                    return Result.Failure("Invalid hotel selected");
                }
                if(hotel.Status != Status.Available)
                {
                    return Result.Failure("Hotel is currently not avaiable");
                }
                if(request.Amount != hotel.Price)
                {
                    return Result.Failure("Please enter the valid amount");
                }
                var transactionRequest = new BookingTransactionRequest
                {
                    Status = Status.Available,
                    StatusDesc = Status.Available.ToString(),
                    CreatedDate = DateTime.Now,
                    TransactionDate = DateTime.Now,
                    TransactionStatus = TransactionStatus.Success,
                    TransactionStatusDesc = TransactionStatus.Processing.ToString(),
                    Amount = request.Amount,
                    TransactionReference = string.Concat("Hotel_Booking", Guid.NewGuid()),
                    CurrencyCode = "NGN",
                    Email = request.Email,
                    TransactionResponse = "Booking successful",
                    FullName = request.FullName,
                    HotelId = hotel.Id
                };
                await _bookingService.AddAsync(transactionRequest);
                hotel.LastModifiedDate = DateTime.Now;
                hotel.Status = Status.NotAvailable;
                hotel.StatusDesc = Status.NotAvailable.ToString();
                await _hotelRepository.UpdateAsync(hotel);
                return Result.Success("Hotel booking was successful", transactionRequest);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}