using HotelBooking.Application.Interfaces.IRepositories;
using HotelBooking.Domain.Entities;
using HotelBooking.Domain.Enums;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Tests.Mocks
{
    internal class MockBookingRepository
    {
        public static Mock<IBookingService> GetBookingService()
        {
            var bookingService = new Mock<IBookingService>();
            List<BookingTransactionRequest> bookings = new List<BookingTransactionRequest>
            {
                new BookingTransactionRequest()
                {
                    Id = 1,
                    CreatedDate = DateTime.Now,
                    Status = Status.Available,
                    StatusDesc = Status.Available.ToString(),
                    TransactionStatus = TransactionStatus.Success,
                    TransactionStatusDesc = TransactionStatus.Success.ToString(),
                    TransactionReference = "Book001",
                    TransactionDate = DateTime.Now,
                    FullName = "User One",
                    Email = "userone@yopmail.com",
                    CurrencyCode = ":NGN",
                    TransactionResponse = "success",
                    HotelId = 2
                },

                new BookingTransactionRequest()
                {
                    Id = 1,
                    CreatedDate = DateTime.Now,
                    Status = Status.Available,
                    StatusDesc = Status.Available.ToString(),
                    TransactionStatus = TransactionStatus.Success,
                    TransactionStatusDesc = TransactionStatus.Success.ToString(),
                    TransactionReference = "Book002",
                    TransactionDate = DateTime.Now,
                    FullName = "User Two",
                    Email = "usertwo@yopmail.com",
                    CurrencyCode = ":NGN",
                    TransactionResponse = "success",
                    HotelId = 2
                }
            };

            bookingService.Setup(c => c.GetAllAsync().Result).Returns(bookings);
            bookingService.Setup(c => c.GetByIdAsync(It.IsAny<int>()).Result).Returns((int id) => bookings.FirstOrDefault(c => c.Id == id));
            bookingService.Setup(c => c.AddAsync(It.IsAny<BookingTransactionRequest>()).Result).Returns((BookingTransactionRequest booking) =>
            {
                bookings.Add(booking);
                return booking;
            });
            bookingService.Setup(c => c.UpdateAsync(It.IsAny<BookingTransactionRequest>())).Callback(() => { return; });
            return bookingService;
        }
    }
}
