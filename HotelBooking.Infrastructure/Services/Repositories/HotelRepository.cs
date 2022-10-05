using HotelBooking.Application.Interfaces.IRepositories;
using HotelBooking.Domain.Entities;
using HotelBooking.Domain.Enums;
using HotelBooking.Infrastructure.Data;
using HotelBooking.Infrastructure.Services.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Infrastructure.Services.Repositories
{
    public class HotelRepository : Repository<Hotel>, IHotelRepository
    {
        public HotelRepository(AppDbContext context) : base(context)
        {

        }
        public async Task<Hotel> UpdateHotelStatus(Status status)
        {
            try
            {
                var existingHotel = await _context.Hotels.FirstOrDefaultAsync(c => c.Status == status);
                return existingHotel;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
