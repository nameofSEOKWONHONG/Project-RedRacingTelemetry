using F12018UdpTelemetry;
using Newtonsoft.Json;
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
            F12018UdpSocket c = new F12018UdpSocket();
            c.Server("192.168.0.11", 20777);
            //c.Send("TEST!");
            F12018UDPMgr.Instance.F1CarMotionDataReceived += Instance_F1CarMotionDataReceived;
            F12018UDPMgr.Instance.F1F1CarTelemetryDataReceived += Instance_F1F1CarTelemetryDataReceived;
            F12018UDPMgr.Instance.F1PacketCarStatusDataReceived += Instance_F1PacketCarStatusDataReceived;
            Console.ReadKey();
        }

        private static void Instance_F1CarMotionDataReceived(object sender, F1CarMotionDataEventArgs e)
        {
            Console.WriteLine(e.packetCode);
            var json = JsonConvert.SerializeObject(e.packetStruct);
            Console.WriteLine(json);
        }

        private static void Instance_F1PacketCarStatusDataReceived(object sender, F1PacketCarStatusDataEventArgs e)
        {
            Console.WriteLine(e.packetCode);
        }

        private static void Instance_F1F1CarTelemetryDataReceived(object sender, F1CarTelemetryDataEventArgs e)
        {
            Console.WriteLine(e.packetCode);
        }
    }
}
