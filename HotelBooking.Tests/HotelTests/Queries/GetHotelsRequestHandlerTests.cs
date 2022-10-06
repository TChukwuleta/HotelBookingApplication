using HotelBooking.Application.Hotel.Queries;
using HotelBooking.Application.Interfaces;
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

namespace HotelBooking.Tests.HotelTests.Queries
{
    public class GetHotelsRequestHandlerTests
    {
        private readonly Mock<IHotelRepository> _hotelRepo; 
        public GetHotelsRequestHandlerTests()
        {
            _hotelRepo = MockHotelRepository.GetHotelRepository();
        }

        [Fact]
        public async void GetAllHotelsTests()
        {
            var handler = new GetAllHotelsQueryHandler(_hotelRepo.Object);
            var result = await handler.Handle(new GetAllHotelsQuery(), CancellationToken.None);
            result.ShouldBeOfType<Result>();
            Assert.NotNull(result.Entity);
            Assert.Equal(true, result.Succeeded);
        }

        [Fact]
        public async void GetAllAvailableHotelsTests()
        {
            var handler = new GetAvailableHotelsQueryHandler(_hotelRepo.Object);
            var result = await handler.Handle(new GetAvailableHotelsQuery(), CancellationToken.None);
            result.ShouldBeOfType<Result>();
            Assert.NotNull(result.Entity);
            Assert.Equal(true, result.Succeeded);
        }

        [Fact]
        public async void GetSingleHotelTest()
        {
            var handler = new GetHotelByIdQueryHandler(_hotelRepo.Object);
            var result = await handler.Handle(new GetHotelByIdQuery() {  HotelId = 2 }, CancellationToken.None);
            result.ShouldBeOfType<Result>();
            Assert.Equal(true, result.Succeeded);
        }

        [Fact]
        public async void GetSingleHotelTest_InvalidId_ReturnsFalse()
        {
            var handler = new GetHotelByIdQueryHandler(_hotelRepo.Object);
            var result = await handler.Handle(new GetHotelByIdQuery() { HotelId = 7 }, CancellationToken.None);
            result.ShouldBeOfType<Result>();
            Assert.Equal(false, result.Succeeded);
        }
    }
}