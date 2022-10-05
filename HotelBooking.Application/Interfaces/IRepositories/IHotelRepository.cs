using HotelBooking.Application.Interfaces.IRepositories.Base;
using HotelBooking.Domain.Entities;
using HotelBooking.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Application.Interfaces.IRepositories
{
    public interface IHotelRepository : IRepository<Domain.Entities.Hotel>
    {
        Task<Domain.Entities.Hotel> UpdateHotelStatus(Status status);
    }
}
