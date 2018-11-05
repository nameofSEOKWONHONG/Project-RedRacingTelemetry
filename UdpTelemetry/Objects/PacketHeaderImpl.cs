using F12018UdpTelemetry;
using Project_RedRacingTelemetry.Abstract;
using Project_RedRacingTelemetry.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project_RedRacingTelemetry.Objects
{
    public class PacketHeaderImpl : IF1PacketObject
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

            PacketHeader packetHeader = new PacketHeader();
            packetHeader.m_packetFormat = bytesAccessor.GetShort(0);
            packetHeader.m_packetVersion = bytesAccessor.GetUnsingedByte(2);
            packetHeader.m_packetId = bytesAccessor.GetUnsingedByte(3);
            packetHeader.m_sessionUID = (long)bytesAccessor.GetDouble(4);
            packetHeader.m_sessionTime = bytesAccessor.GetFloat(12);
            packetHeader.m_frameIdentifier = bytesAccessor.GetInt(16);

            if (packetHeader.m_packetFormat != 2018)
            {
                throw new Exception("not support F1 game version. Only support F1 2018");
            }

            packetReceived?.Invoke(this, new F1PacketEventEventArgs(packetHeader: packetHeader));
        }
    }
}
