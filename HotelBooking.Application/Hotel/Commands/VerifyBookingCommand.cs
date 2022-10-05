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
    public class VerifyBookingCommand : IRequest<Result>
    {
        public string Reference { get; set; }
    }
    public class VerifyBookingCommandHandler : IRequestHandler<VerifyBookingCommand, Result>
    {
        private readonly IAppDbContext _context;
        private readonly IPaystackService _paystackService;
        public VerifyBookingCommandHandler(IAppDbContext context, IPaystackService paystackService)
        {
            _context = context;
            _paystackService = paystackService;
        }

        public async Task<Result> Handle(VerifyBookingCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var existingBookingTransaction = await _context.BookingTransactionRequests.FirstOrDefaultAsync(c => c.TransactionReference == request.Reference);
                if(existingBookingTransaction == null)
                {
                    return Result.Failure("No existing transaction exists for this reference");
                }
                if(existingBookingTransaction.TransactionStatus == TransactionStatus.Success)
                {
                    return Result.Failure($"{existingBookingTransaction.TransactionResponse} has already been processed successfully.");
                }
                PaymentIntentVm paymentIntentVm = new PaymentIntentVm
                {
                    Amount = existingBookingTransaction.Amount * 100,//kobo equivalent
                    CurrencyCode = existingBookingTransaction.CurrencyCode,
                    Email = existingBookingTransaction.Email,
                    ClientReferenceId = existingBookingTransaction.TransactionReference
                };
                var verifyResponse = await _paystackService.VerifyTransactionStatus(paymentIntentVm);
                if (!verifyResponse.status)

                {
                    return Result.Failure("Verifying Paystack Payment with reference number " + existingBookingTransaction.TransactionReference + "is " + verifyResponse.status.ToString() + " with message " + verifyResponse.message);
                }
                if (Convert.ToDecimal(verifyResponse.data.requested_amount) != existingBookingTransaction.Amount)
                {
                    return Result.Failure("Error with transaction reference " + existingBookingTransaction.TransactionReference + "Amount from Paystack is not equal to paid amount. Please contact support");
                }

                var message = "";
                switch (verifyResponse.data.status)
                {
                    case "success":
                        existingBookingTransaction.TransactionStatus = TransactionStatus.Success;
                        existingBookingTransaction.TransactionStatusDesc = TransactionStatus.Success.ToString();
                        message = "Your payment has been fulfilled successfully";
                        break;
                    case "abandoned":
                        existingBookingTransaction.TransactionStatus = TransactionStatus.Cancelled;
                        existingBookingTransaction.TransactionStatusDesc = TransactionStatus.Cancelled.ToString();
                        message = "Oops! You abandoned your payment";
                        break;
                    case "cancelled":
                        existingBookingTransaction.TransactionStatus = TransactionStatus.Cancelled;
                        existingBookingTransaction.TransactionStatusDesc = TransactionStatus.Cancelled.ToString();
                        message = "Oops! You cancelled your payment";
                        break;
                    case "failed":
                        existingBookingTransaction.TransactionStatus = TransactionStatus.Failed;
                        existingBookingTransaction.TransactionStatusDesc = TransactionStatus.Failed.ToString();
                        message = "Oops! Your  payment failed";
                        break;
                    case "failure":
                        existingBookingTransaction.TransactionStatus = TransactionStatus.Failed;
                        existingBookingTransaction.TransactionStatusDesc = TransactionStatus.Failed.ToString();
                        message = "Oops! Your payment failed";
                        break;
                    case "processing":
                        existingBookingTransaction.TransactionStatus = TransactionStatus.Processing;
                        existingBookingTransaction.TransactionStatusDesc = TransactionStatus.Processing.ToString();
                        message = "Your payment is processing";
                        break;
                    default:
                        break;
                }
                _context.BookingTransactionRequests.Update(existingBookingTransaction);
                var hotel = await _context.Hotels.FirstOrDefaultAsync(c => c.Id == existingBookingTransaction.HotelId);
                hotel.LastModifiedDate = DateTime.Now;
                hotel.Status = Status.NotAvailable;
                hotel.StatusDesc = Status.NotAvailable.ToString();
                _context.Hotels.Update(hotel);
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
