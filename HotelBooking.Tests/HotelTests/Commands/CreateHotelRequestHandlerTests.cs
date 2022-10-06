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
    public class CreateHotelRequestHandlerTests
    {
        private readonly Mock<IHotelRepository> _hotelRepo;
        private readonly Mock<IUploadService> _uploadRepo;
        public CreateHotelRequestHandlerTests()
        {
            _hotelRepo = MockHotelRepository.GetHotelRepository();
            _uploadRepo = new Mock<IUploadService>();
        }

        [Fact]
        public async void CreateHotelTests()
        {

            var handler = new CreateHotelCommandHandler(_uploadRepo.Object, _hotelRepo.Object);
            var result = await handler.Handle(new CreateHotelCommand() {
                Name = "Test",
                Address = "string",
                FacilityType = new List<int> { 1, 2 },
                Description = "string",
                Base64Image = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAUAAAAFCAYAAACNbyblAAAAHElEQVQI12P4//8/w38GIAXDIBKE0DHxgljNBAAO9TXL0Y4OHwAAAABJRU5ErkJggg==",
                Extension = "png",
                Price = 100,
                Rating = 2
            }, CancellationToken.None);
            result.ShouldBeOfType<Result>();
            Assert.NotNull(result.Entity);
            Assert.Equal(true, result.Succeeded);
            var hotels = await _hotelRepo.Object.GetAllAsync();
            hotels.Count.ShouldBe(3);
            Assert.Equal(true, result.Succeeded);
        }
    }
}
