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
    internal class MockHotelRepository
    {
        public static Mock<IHotelRepository> GetHotelRepository()
        {
            var hotelRepository = new Mock<IHotelRepository>();
            var facilityies = new List<Facility>()
            {
                new Facility()
                {
                    Id = 1,
                    CreatedDate = DateTime.Now,
                    Status = Status.Available,
                    StatusDesc = Status.Available.ToString(),
                    FacilityType = FacilityType.Wifi,
                    FacilityTypeDesc = FacilityType.Wifi.ToString()
                }
            };
            List<Hotel> hotels = new List<Hotel>
            {
                new Hotel()
                {
                    Id = 1,
                    CreatedDate = DateTime.Now,
                    Status = Status.Available,
                    StatusDesc = Status.Available.ToString(),
                    Name = "Test Hotel",
                    Image = "testhotelimage.png",
                    Address = "Qatar 2022",
                    Description = "Nothing to say here",
                    Rating = 3,
                    Price = 1000,
                    Facility = facilityies
                },

                new Hotel()
                {
                    Id = 2,
                    CreatedDate = DateTime.Now,
                    Status = Status.UnderReview,
                    StatusDesc = Status.UnderReview.ToString(),
                    Name = "Test Two Hotel",
                    Image = "testtwohotelimage.png",
                    Address = "Qatar World cup 2022",
                    Description = "Nothing to say here",
                    Rating = 3,
                    Price = 1000,
                    Facility = facilityies
                }
            };

            hotelRepository.Setup(c => c.GetAllAsync().Result).Returns(hotels);
            hotelRepository.Setup(c => c.GetByIdAsync(It.IsAny<int>()).Result).Returns((int id) => hotels.FirstOrDefault(c => c.Id == id));
            hotelRepository.Setup(c => c.AddAsync(It.IsAny<Hotel>()).Result).Returns((Hotel hotel) =>
            {
                hotels.Add(hotel);
                return hotel;
            });
            hotelRepository.Setup(c => c.UpdateAsync(It.IsAny<Hotel>())).Callback(() => { return; });
            return hotelRepository;
        }
    }
}
