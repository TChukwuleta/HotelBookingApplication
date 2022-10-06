using HotelBooking.Application.Interfaces;
using HotelBooking.Application.Interfaces.IRepositories;
using HotelBooking.Application.Model;
using HotelBooking.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Application.Hotel.Commands
{
    public class CreateHotelCommand : IRequest<Result>
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public List<int> FacilityType { get; set; }
        public string Description { get; set; }
        public string Base64Image { get; set; }
        public string Extension { get; set; }
        public decimal Price { get; set; }
        public int Rating { get; set; }
    }
    public class CreateHotelCommandHandler : IRequestHandler<CreateHotelCommand, Result>
    {
        private readonly IUploadService _uploadService;
        private readonly IHotelRepository _hotelRepository;
        public CreateHotelCommandHandler(IUploadService uploadService, IHotelRepository hotelRepository)
        {
            _uploadService = uploadService;
            _hotelRepository = hotelRepository;
        }

        public async Task<Result> Handle(CreateHotelCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if(request.Rating < 0 || request.Rating > 5)
                {
                    return Result.Failure("Rating should be between 0 and 5");
                }
                var fileUrl = string.IsNullOrEmpty(request.Base64Image) ? $"{request.Name}.jpeg" : await _uploadService.UploadImage(request.Base64Image);
                var facilities = new List<Domain.Entities.Facility>();
                if(request.FacilityType.Count > 0)
                {
                    foreach (var item in request.FacilityType)
                    {
                        FacilityType facilityType = (FacilityType)item;
                        facilities.Add(new Domain.Entities.Facility
                        {
                            FacilityType = facilityType,
                            FacilityTypeDesc = facilityType.ToString(),
                            CreatedDate = DateTime.Now,
                            Status = Status.Available,
                            StatusDesc = Status.Available.ToString()
                        });
                    }
                }
                var newHotel = new Domain.Entities.Hotel
                {
                    Name = request.Name,
                    Address = request.Address,
                    Image = fileUrl,
                    Description = request.Description,
                    Rating = request.Rating,
                    Price = request.Price,
                    Status = Status.UnderReview,
                    StatusDesc = Status.UnderReview.ToString(),
                    Facility = facilities,
                    CreatedDate = DateTime.Now
                };
                var result = await _hotelRepository.AddAsync(newHotel);
                return Result.Success("New Hotel added successfully", result);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Adding new hotel was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
