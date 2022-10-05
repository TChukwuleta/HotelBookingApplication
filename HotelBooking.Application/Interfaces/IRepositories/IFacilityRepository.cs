using HotelBooking.Application.Interfaces.IRepositories.Base;
using HotelBooking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Application.Interfaces.IRepositories
{
    public interface IFacilityRepository : IRepository<Domain.Entities.Facility>
    {
    }
}
