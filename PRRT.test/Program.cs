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

            //F12018UDPMgr.Instance.F1CarMotionDataReceived += Instance_F1CarMotionDataReceived;
            F12018UDPMgr.Instance.F1PacketMotionDataReceived += Instance_F1PacketMotionDataReceived;
            //F12018UDPMgr.Instance.F1PacketSessionDataReceived += Instance_F1PacketSessionDataReceived;
            //F12018UDPMgr.Instance.F1LapDataReceived += Instance_F1LapDataReceived;
            //F12018UDPMgr.Instance.F1ParticipantDataReceived += Instance_F1ParticipantDataReceived;
            //F12018UDPMgr.Instance.F1CarSetupDataReceived += Instance_F1CarSetupDataReceived;
            //F12018UDPMgr.Instance.F1CarStatusDataReceived += Instance_F1CarStatusDataReceived;
            
            Console.ReadKey();
        }

        private static void Instance_F1PacketMotionDataReceived(object sender, F1PacketMotionDataEventArgs e)
        {
            Console.Clear();
            //var packetStr = $"RL:{e.packetStruct.m_suspensionAcceleration[0]}" + Environment.NewLine;
            //packetStr += $"RR:{e.packetStruct.m_suspensionAcceleration[1]}" + Environment.NewLine;
            //packetStr += $"FL:{e.packetStruct.m_suspensionAcceleration[2]}" + Environment.NewLine;
            //packetStr += $"FR:{e.packetStruct.m_suspensionAcceleration[3]}" + Environment.NewLine;
            var packetStr = $"m_worldForwardDirX:{e.packetStruct.m_carMotionData[F12018UDPMgr.Instance.PacketHeader.m_packetId].m_worldForwardDirX}" + Environment.NewLine;
            packetStr += $"m_worldForwardDirY:{e.packetStruct.m_carMotionData[F12018UDPMgr.Instance.PacketHeader.m_packetId].m_worldForwardDirY}" + Environment.NewLine;
            packetStr += $"m_worldForwardDirZ:{e.packetStruct.m_carMotionData[F12018UDPMgr.Instance.PacketHeader.m_packetId].m_worldForwardDirZ}" + Environment.NewLine;
            packetStr += $"m_worldPositionX:{e.packetStruct.m_carMotionData[F12018UDPMgr.Instance.PacketHeader.m_packetId].m_worldPositionX}" + Environment.NewLine;
            packetStr += $"m_worldPositionY:{e.packetStruct.m_carMotionData[F12018UDPMgr.Instance.PacketHeader.m_packetId].m_worldPositionY}" + Environment.NewLine;
            packetStr += $"m_worldPositionZ:{e.packetStruct.m_carMotionData[F12018UDPMgr.Instance.PacketHeader.m_packetId].m_worldPositionZ}" + Environment.NewLine;
            Console.WriteLine(packetStr);
            
        }

        private static void Instance_F1CarStatusDataReceived(object sender, F1CarStatusDataEventArgs e)
        {
            Console.WriteLine(JsonConvert.SerializeObject(e.packetStruct, Formatting.Indented));
        }

        private static void Instance_F1CarSetupDataReceived(object sender, F1CarSetupEventArgs e)
        {
            Console.WriteLine(JsonConvert.SerializeObject(e.packetStruct, Formatting.Indented));
        }

        private static void Instance_F1ParticipantDataReceived(object sender, F1ParticipantDataEventArgs e)
        {
            Console.WriteLine(JsonConvert.SerializeObject(e.packetStruct, Formatting.Indented));
        }

        private static void Instance_F1LapDataReceived(object sender, F1LapDataEventArgs e)
        {
            Console.WriteLine(JsonConvert.SerializeObject(e.packetStruct, Formatting.Indented));
        }

        private static void Instance_F1PacketSessionDataReceived(object sender, F1PacketSessionDataEventArgs e)
        {
            Console.WriteLine(JsonConvert.SerializeObject(e.packetStruct, Formatting.Indented));
        }

        private static void Instance_F1CarMotionDataReceived(object sender, F1CarMotionDataEventArgs e)
        {
            Console.WriteLine(JsonConvert.SerializeObject(e.packetStruct, Formatting.Indented));
        }
    }
}
