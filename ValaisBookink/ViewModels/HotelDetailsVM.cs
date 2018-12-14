using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DTO;

namespace ValaisBookink.ViewModels
{
    public class HotelDetailsVM
    {
        public Hotel Hotel { get; set; }
        public List<Picture> Pictures { get; set; }
        public HotelCapacity HotelCapacity { get; set; }
    }
}