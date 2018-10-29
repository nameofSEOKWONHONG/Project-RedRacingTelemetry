using System;
using System.IO;

namespace F12018UdpTelemetry
{
    public class F1PacketConverterFactory {
        public static T CreatePacketConverter<T>(byte packetId, BinaryReader br) {
            var structObj = default(T);

            switch(packetId)
            {
                case 0:
                    Console.WriteLine("Motion");
                    structObj = F1PacketConvertUtils.FromBinaryReader<T>(br);
                    break;
                case 1:
                    Console.WriteLine("Session");
                    structObj = F1PacketConvertUtils.FromBinaryReader<T>(br);
                    break;
                case 2:
                    Console.WriteLine("Lap Data");
                    
                    break;
                case 3:
                    Console.WriteLine("Event");
                    break;
                case 4:
                    Console.WriteLine("Participants");
                    break;
                case 5:
                    Console.WriteLine("Car Setups");
                    break;
                case 6:
                    Console.WriteLine("Car Telemetry");
                    break;
                case 7:
                    Console.WriteLine("Car Status");
                    break;
                default:
                    break;
            }

            return structObj;
        }

       
    }
}