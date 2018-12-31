using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using DTO;

namespace ValaisBookink.ViewModels
{
    public class ReservationValidateVM
    {
        public string HotelName { get; set; }
        public DateTime Arrival { get; set; }
        public DateTime Departure { get; set; }
        public Double TotalNight { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public ICollection<Room> Rooms { get; set; }
        public Decimal TotalPrice { get; set; }
        public int RoomNumber { get; set; }
    }
}