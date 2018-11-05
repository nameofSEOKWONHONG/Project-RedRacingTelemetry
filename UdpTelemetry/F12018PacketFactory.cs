using F12018UdpTelemetry;
using Project_RedRacingTelemetry.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project_RedRacingTelemetry
{
    public class F12018PacketFactory
    {
        private F12018PacketFactory() { }

        public static IF1PacketObject CreateInstance(PacketHeader packetHeader)
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
