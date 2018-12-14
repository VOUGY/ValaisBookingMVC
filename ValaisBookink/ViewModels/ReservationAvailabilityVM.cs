using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DTO;

namespace ValaisBookink.ViewModels
{
    public class ReservationAvailabilityVM
    {
        public string IdHotel { get; set; }
        public string HotelName { get; set; }
        public DateTime Arrival { get; set; }
        public DateTime Departure { get; set; }
        public List<Room> Rooms { get; set; }
    }
}