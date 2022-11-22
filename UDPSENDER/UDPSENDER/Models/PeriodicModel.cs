using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UDPSENDER.Models
{
    public class PeriodicModel
    {
        public int id { get; set; }
        public System.Timers.Timer timer { get; set; }
        public string packetName { get; set; }

        
    }
}
