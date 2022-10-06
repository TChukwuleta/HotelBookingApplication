using HotelBooking.Application.Hotel.Commands;
using HotelBooking.Application.Interfaces.IRepositories;
using HotelBooking.Application.Model;
using HotelBooking.Tests.Mocks;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Tests.HotelTests.Commands
{
    public class BookHotelRequestHandlerTests
    {
        private readonly Mock<IHotelRepository> _hotelRepository;
        private readonly Mock<IBookingService> _bookingService;

        public BookHotelRequestHandlerTests()
        {
            _hotelRepository = MockHotelRepository.GetHotelRepository();
            _bookingService = MockBookingRepository.GetBookingService();
        }


        public string FullName { get; set; }
        public string Email { get; set; }
        public decimal Amount { get; set; }
        public int HotelId { get; set; }
        [Fact]
        public async void UpdateHotelTests()
        {
            var handler = new BookHotelCommandHandler(_hotelRepository.Object, _bookingService.Object);
            var result = await handler.Handle(new BookHotelCommand()
            {
                HotelId = 1,
                FullName = "User Three",
                Email = "userthree@yopmail.com",
                Amount = 1000
            }, CancellationToken.None);
            result.ShouldBeOfType<Result>();
            Assert.NotNull(result.Entity);
            Assert.Equal(true, result.Succeeded);
        }

        [Fact]
        public async void UpdateHotelTests_InvalidHotelId_Fails()
        {
            var handler = new BookHotelCommandHandler(_hotelRepository.Object, _bookingService.Object);
            var result = await handler.Handle(new BookHotelCommand()
            {
                HotelId = 7,
                FullName = "User Three",
                Email = "userthree@yopmail.com",
                Amount = 1000
            }, CancellationToken.None);
            result.ShouldBeOfType<Result>();
            Assert.Equal(false, result.Succeeded);
        }

        [Fact]
        public async void UpdateHotelTests_UnavailableHotel_Fails()
        {
            var handler = new BookHotelCommandHandler(_hotelRepository.Object, _bookingService.Object);
            var result = await handler.Handle(new BookHotelCommand()
            {
                HotelId = 2,
                FullName = "User Three",
                Email = "userthree@yopmail.com",
                Amount = 1000
            }, CancellationToken.None);
            result.ShouldBeOfType<Result>();
            Assert.Equal(false, result.Succeeded);
        }

        [Fact]
        public async void UpdateHotelTests_WrongBookingPrice_Fails()
        {
            var handler = new BookHotelCommandHandler(_hotelRepository.Object, _bookingService.Object);
            var result = await handler.Handle(new BookHotelCommand()
            {
                HotelId = 1,
                FullName = "User Three",
                Email = "userthree@yopmail.com",
                Amount = 10000
            }, CancellationToken.None);
            result.ShouldBeOfType<Result>();
            Assert.Equal(false, result.Succeeded);
        }
    }
}
