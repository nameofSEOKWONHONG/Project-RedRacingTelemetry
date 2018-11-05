using F12018UdpTelemetry;
using Project_RedRacingTelemetry.Abstract;
using Project_RedRacingTelemetry.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project_RedRacingTelemetry.Objects
{
    public class PacketSessionImpl : IF1PacketObject
    {
        event EventHandler packetReceived;

        object objectLock = new Object();

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

            PacketSessionData packetSessionData = new PacketSessionData();
            packetSessionData.m_totalLaps = bytesAccessor.GetUnsingedByte(24);
            packetSessionData.m_trackLength = (float)bytesAccessor.GetShort(25);
            packetSessionData.m_sessionType = bytesAccessor.GetSignedByte(27);

            // Raise IDrawingObject's event before the object is drawn.
            packetReceived?.Invoke(this, new F1PacketEventEventArgs(packetSessionData: packetSessionData));
        }
    }
}
