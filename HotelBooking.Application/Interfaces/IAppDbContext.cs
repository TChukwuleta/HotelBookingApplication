using HotelBooking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Application.Interfaces
{
    public interface IAppDbContext
    {
        DbSet<Domain.Entities.Facility> Facilities { get; set; }
        DbSet<Domain.Entities.Hotel> Hotels { get; set; }
        DbSet<Domain.Entities.BookingTransactionRequest> BookingTransactionRequests { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
