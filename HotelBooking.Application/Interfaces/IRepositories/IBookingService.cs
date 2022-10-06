using HotelBooking.Application.Interfaces.IRepositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Application.Interfaces.IRepositories
{
    public interface IBookingService : IRepository<Domain.Entities.BookingTransactionRequest>
    {
    }
}
