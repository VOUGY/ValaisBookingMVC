using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public partial class Picture
    {
        public int IdPicture { get; set; }
        public string Url { get; set; }
        public int IdRoom { get; set; }
    
        public virtual Room Room { get; set; }
    }
}
