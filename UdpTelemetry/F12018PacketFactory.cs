using F12018UdpTelemetry;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project_RedRacingTelemetry
{
    public class F12018PacketFactory
    {
        private F12018PacketFactory() { }

        public static IF12018Packet CreateInstance(PacketHeader packetHeader)
        {
            switch(packetHeader.m_packetId)
            {
                case 0:
                    return null;
                default:
                    return null;
            }
        }
    }
}
