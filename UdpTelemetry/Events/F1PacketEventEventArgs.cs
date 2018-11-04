using F12018UdpTelemetry;
using Project_RedRacingTelemetry.Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project_RedRacingTelemetry.Events
{
    public class F1PacketEventEventArgs : EventArgs {
        public PacketHeader packetHeader;
        public CarMotionData carMotionData;

        public PacketSessionData packetSessionData;

        public F1PacketEventEventArgs(PacketHeader packetHeader = null,
            CarMotionData carMotionData = null,
            PacketSessionData packetSessionData = null)
        {
            this.packetHeader = packetHeader;
            this.carMotionData = carMotionData;

            this.packetSessionData = packetSessionData;
        }
    }
}
