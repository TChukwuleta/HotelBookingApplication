using HotelBooking.Application.Interfaces;
using HotelBooking.Application.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Application.Hotel.Commands
{
    public class UpdateHotelCommand : IRequest<Result>
    {
        public int HotelId { get; set; }
        public string Name { get; set; }
        public string Base64Image { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public int Rating { get; set; }
        public decimal Price { get; set; }
    }

    public class UpdateHotelCommandHandler : IRequestHandler<UpdateHotelCommand, Result>
    {
        private readonly IAppDbContext _context;
        private readonly IUploadService _uploadService;
        public UpdateHotelCommandHandler(IAppDbContext context, IUploadService uploadService)
        {
            _context = context;
            _uploadService = uploadService;
        }

        public async Task<Result> Handle(UpdateHotelCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var hotel = await _context.Hotels.FirstOrDefaultAsync(c => c.Id == request.HotelId);
                if(hotel == null)
                {
                    return Result.Failure("Invalid hotel selected for update");
                }
                if (request.Rating < 0 || request.Rating > 5)
                {
                    return Result.Failure("Rating should be between 0 and 5");
                }
                hotel.Name = request.Name;
                hotel.Address = request.Address;
                hotel.Price = request.Price;
                hotel.Description = request.Description;
                hotel.LastModifiedDate = DateTime.Now;

                _context.Hotels.Update(hotel);
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success("Hotel details have been successfully updated");
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
