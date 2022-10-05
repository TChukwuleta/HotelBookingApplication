using HotelBooking.Application.Interfaces;
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
        public decimal Price { get; set; }
        public int Rating { get; set; }
    }
    public class CreateHotelCommandHandler : IRequestHandler<CreateHotelCommand, Result>
    {
        private readonly IAppDbContext _context;
        private readonly IUploadService _uploadService;
        public CreateHotelCommandHandler(IAppDbContext context, IUploadService uploadService)
        {
            _context = context;
            _uploadService = uploadService;
        }

        public async Task<Result> Handle(CreateHotelCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if(request.Rating < 0 || request.Rating > 5)
                {
                    return Result.Failure("Rating should be between 0 and 5");
                }
                var fileUrl = await _uploadService.UploadImage(request.Name, request.Address);
                if (string.IsNullOrEmpty(fileUrl))
                {
                    return Result.Failure("An error occured");
                }
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
                            Status = Status.Available
                        });
                    }
                    await _context.Facilities.AddRangeAsync(facilities);
                }
                var newHotel = new Domain.Entities.Hotel
                {
                    Name = request.Name,
                    Address = request.Address,
                    Description = request.Description,
                    Rating = request.Rating,
                    Price = request.Price,
                    Status = Status.UnderReview,
                    Facility = facilities,
                    CreatedDate = DateTime.Now
                };
                await _context.Hotels.AddAsync(newHotel);
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success("New Hotel added successfully", newHotel);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
