using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace UDPSENDER.Models
{
    public class StaticClass
    {



      
        /*
         her sayfadan erişilmesi gereken bilgiler burada tutulur.
         */
        public static ValueModel Values=new ValueModel();
        public static int packetindeks=0;
        public static int valueindeks;
        public static String url;
        public static List<UdpModel> UdpList = new List<UdpModel>();
        public static List<PeriodicModel> PeriodicModels = new List<PeriodicModel>();
        public static IPEndPoint e;
        public static UdpClient u;


    }
  

}
