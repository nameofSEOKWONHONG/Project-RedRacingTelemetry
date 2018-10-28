using F12018UdpTelemetry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F12018UdpTelemetry.test
{
    class Program
    {
        static void Main(string[] args)
        {
            UDPSocket c = new UDPSocket();
            c.Server("192.168.0.11", 20777);
            //c.Send("TEST!");

            Console.ReadKey();
        }
    }
}
