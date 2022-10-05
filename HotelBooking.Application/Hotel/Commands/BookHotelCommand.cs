using HotelBooking.Application.Interfaces;
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
        private readonly IAppDbContext _context;
        private readonly IPaystackService _paystackService;
        public BookHotelCommandHandler(IAppDbContext context, IPaystackService paystackService)
        {
            _context = context;
            _paystackService = paystackService;
        }
        public async Task<Result> Handle(BookHotelCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var hotel = await _context.Hotels.FirstOrDefaultAsync(c => c.Id == request.HotelId);
                if(hotel == null)
                {
                    return Result.Failure("Invalid hotel selected");
                }
                if(hotel.Status != Status.Available)
                {
                    return Result.Failure("Hotel is currently not avaiable");
                }
                PaymentIntentVm paymentIntentVm = new PaymentIntentVm
                {
                    Amount = request.Amount * 100,
                    Email = request.Email,
                    CurrencyCode = "NGN",
                    ClientReferenceId = string.Concat("Hotel_Booking", Guid.NewGuid())
                };
                var response = await _paystackService.InitializeTransaction(paymentIntentVm);
                if (!response.status)
                {
                    return Result.Failure("Unable to book hotel. Could nor process transaction request");
                }
                var transactionRequest = new BookingTransactionRequest
                {
                    Status = Status.Available,
                    StatusDesc = Status.Available.ToString(),
                    CreatedDate = DateTime.Now,
                    TransactionDate = DateTime.Now,
                    TransactionStatus = TransactionStatus.Processing,
                    TransactionStatusDesc = TransactionStatus.Processing.ToString(),
                    Amount = request.Amount,
                    TransactionReference = response.data.reference == null ? response.data.reference : response.data.reference,
                    CurrencyCode = "NGN",
                    Email = request.Email,
                    TransactionResponse = response.message,
                    FullName = request.FullName,
                    HotelId = hotel.Id
                };
                await _context.BookingTransactionRequests.AddAsync(transactionRequest);
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success("Payment initiation was successful", response);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}