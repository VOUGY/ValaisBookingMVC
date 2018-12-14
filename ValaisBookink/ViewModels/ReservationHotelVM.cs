using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DTO;

namespace ValaisBookink.ViewModels
{
    public class ReservationHotelVM
    {
        public Room Room { get; set; }
        public List<Picture> Pictures { get; set; }
    }
}