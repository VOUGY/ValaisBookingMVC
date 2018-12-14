using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class HotelWithInfo
    {
        public Hotel Hotel { get; set; }
        public string UrlPicture { get; set; }
        public HotelAvailability HotelAvailability { get; set; }
        public HotelCapacity HotelCapacity { get; set; }
        
    }
}
