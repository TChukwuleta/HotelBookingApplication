using HotelBooking.Application.Interfaces.IRepositories;
using HotelBooking.Domain.Entities;
using HotelBooking.Infrastructure.Data;
using HotelBooking.Infrastructure.Services.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Infrastructure.Services.Repositories
{
    public class FacilityRepository : Repository<Facility>, IFacilityRepository
    { 
        public FacilityRepository(AppDbContext context) : base(context)
        {

        }
    }
}
