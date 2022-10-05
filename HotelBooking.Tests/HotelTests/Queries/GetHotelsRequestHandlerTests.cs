using HotelBooking.Application.Hotel.Queries;
using HotelBooking.Application.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Tests.HotelTests.Queries
{
    public class GetHotelsRequestHandlerTests
    {
        private readonly Mock<IAppDbContext> _context; 
        public GetHotelsRequestHandlerTests()
        {
            
        }

        /*[Fact]
        public async void GetAllHotelsTests()
        {
            var handler = new GetAllHotelsQueryHandler(_orderRepo.Object);
            var result = await handler.Handle(new GetAllOrdersQuery(), CancellationToken.None);
            result.ShouldBeOfType<Result>();
            Assert.NotNull(result.Entity);
            Assert.Equal(true, result.Succeeded);
        }

        [Fact]
        public async void GetSingleHotelTest()
        {
            var handler = new GetOrderByIdQueryHandler(_orderRepo.Object);
            var result = await handler.Handle(new GetOrderByIdQuery(), CancellationToken.None);
            result.ShouldBeOfType<Result>();
            Assert.Equal(true, result.Succeeded);
        }*/
    }
}
