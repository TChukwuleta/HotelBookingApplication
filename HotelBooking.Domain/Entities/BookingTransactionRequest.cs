using HotelBooking.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Domain.Entities
{
    public class BookingTransactionRequest : AuditableEntity
    {
        public DateTime TransactionDate { get; set; }
        public string FullName { get; set; }
        public decimal Amount { get; set; }
        public TransactionStatus TransactionStatus { get; set; }
        public string TransactionStatusDesc { get; set; }
        public string TransactionReference { get; set; }
        public string CurrencyCode { get; set; }
        public string Email { get; set; }
        public string TransactionResponse { get; set; }
        public int HotelId { get; set; }
    }
}
