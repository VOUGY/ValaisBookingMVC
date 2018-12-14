using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class Reservation
    {
        public int IdReservation { get; set; }
        public DateTime Arrival { get; set; }
        public DateTime Departure { get; set; }
        public List<Room> Rooms { get; set; }
        public Client Client { get; set; }
    }
}
