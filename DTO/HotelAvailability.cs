using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class HotelAvailability
    {
        public int NumberOfAvailableRoom { get; set; }
        public int NumberOfAvailablePerson { get; set; }
        public double OcucupancyRate { get; set; }
        public bool HasAvailableRoomWithTv { get; set; }
        public bool HasAvailableRoomWithHairDryer { get; set; }
        public decimal MinPrice { get; set; }
    }
}
