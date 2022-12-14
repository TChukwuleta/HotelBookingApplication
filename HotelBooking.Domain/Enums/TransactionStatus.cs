using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Domain.Enums
{
    public enum TransactionStatus
    {
        Initiated = 1,
        Processing = 2,
        Success = 3,
        Failed = 4,
        Reversed = 5,
        Cancelled = 6,
        RequiresAction = 7,
        RequiresCapture = 8,
        RequiresConfirmation = 9,
        RequiresPaymentMethod = 10
    }
}
