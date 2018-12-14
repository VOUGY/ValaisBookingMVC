using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class Client
    {
        public int IdClient { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public Reservation Reservation { get; set; }
    }
}
