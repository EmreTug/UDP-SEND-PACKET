using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace UDPSENDER.Models
{
    public class UdpModel
    {

        public int port { get; set; }
        public UdpClient client { get; set; }

    }

}
