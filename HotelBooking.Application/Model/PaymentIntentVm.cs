using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Application.Model
{
    public class PaymentIntentVm
    {
        public decimal Amount { get; set; }
        public string Email { get; set; }
        public string ClientReferenceId { get; set; }
        public string CurrencyCode { get; set; }
    }
}
