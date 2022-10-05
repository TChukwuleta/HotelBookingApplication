using HotelBooking.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Domain.Entities
{
    public class Facility : AuditableEntity
    {
        public FacilityType FacilityType { get; set; }
        public string FacilityTypeDesc { get; set; }
    }
}
