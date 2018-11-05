using F12018UdpTelemetry;
using Project_RedRacingTelemetry.Abstract;
using Project_RedRacingTelemetry.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project_RedRacingTelemetry.Objects
{
    public class PacketMotionImpl : IF1PacketObject
    {
        object objectLock = new Object();

        event EventHandler packetReceived;

        public event EventHandler OnPacketReceived
        {
            add
            {
                lock (objectLock)
                {
                    packetReceived += value;
                }
            }
            remove
            {
                lock (objectLock)
                {
                    packetReceived -= value;
                }
            }
        }

        public void Parse(byte[] buffer)
        {
            F1BytesAccessor bytesAccessor = new F1BytesAccessor(buffer, true);

            PacketMotionData packetMotion = new PacketMotionData();
            //packetMotion.
           

            packetReceived?.Invoke(this, new F1PacketEventEventArgs(packetMotionData: packetMotion));

        }
    }
}
