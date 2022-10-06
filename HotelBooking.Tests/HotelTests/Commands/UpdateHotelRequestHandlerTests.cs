using HotelBooking.Application.Hotel.Commands;
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

namespace HotelBooking.Tests.HotelTests.Commands
{
    public class UpdateHotelRequestHandlerTests
    {
        private readonly Mock<IHotelRepository> _hotelRepo;
        private readonly Mock<IUploadService> _uploadService;
        public UpdateHotelRequestHandlerTests()
        {
            _hotelRepo = MockHotelRepository.GetHotelRepository();
            _uploadService = new Mock<IUploadService>();
        }

        [Fact]
        public async void ChangeHotelStatusTests()
        {

            var handler = new ChangeHotelStatusCommandHandler(_hotelRepo.Object);
            var result = await handler.Handle(new ChangeHotelStatusCommand() { HotelId = 2 }, CancellationToken.None);
            result.ShouldBeOfType<Result>();
            Assert.NotNull(result.Entity);
            Assert.Equal(true, result.Succeeded);
        }

        [Fact]
        public async void UpdateHotelTests()
        {

            var handler = new UpdateHotelCommandHandler(_uploadService.Object, _hotelRepo.Object);
            var result = await handler.Handle(new UpdateHotelCommand() { 
                HotelId = 2,
                Name = "Updated Hotel",
                Address = "Updated Address",
                Description = "Updated Description",
                Rating = 4,
                Price = 100
            }, CancellationToken.None);
            result.ShouldBeOfType<Result>();
            Assert.NotNull(result.Entity);
            Assert.Equal(true, result.Succeeded);
        }

        [Fact]
        public async void UpdateHotelTests_InvalidRating_Fails()
        {

            var handler = new UpdateHotelCommandHandler(_uploadService.Object, _hotelRepo.Object);
            var result = await handler.Handle(new UpdateHotelCommand() {
                HotelId = 2,
                Name = "Updated Hotel",
                Address = "Updated Address",
                Description = "Updated Description",
                Rating = 7,
                Price = 100
            }, CancellationToken.None);
            result.ShouldBeOfType<Result>();
            Assert.Equal(false, result.Succeeded);
        }

        [Fact]
        public async void UpdateHotelTests_InvalidHotelId_Fails()
        {

            var handler = new UpdateHotelCommandHandler(_uploadService.Object, _hotelRepo.Object);
            var result = await handler.Handle(new UpdateHotelCommand() {
                HotelId = 9,
                Name = "Updated Hotel",
                Address = "Updated Address",
                Description = "Updated Description",
                Rating = 4,
                Price = 100
            }, CancellationToken.None);
            result.ShouldBeOfType<Result>();
            Assert.Equal(false, result.Succeeded);
        }
    }
}