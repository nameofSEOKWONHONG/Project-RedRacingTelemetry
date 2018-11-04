using Project_RedRacingTelemetry.Abstract;
using Project_RedRacingTelemetry.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project_RedRacingTelemetry.Objects
{
    /// <summary>
    ///  2018의 주요 변경 사항은 여러 패킷 유형의 도입입니다. 
    /// 이제 각 패킷은 모든 것을 포함하는 하나의 패킷을 가지지 않고 다른 유형의 데이터를 전달할 수 있습니다. 
    /// 각 패킷에도 헤더가 추가되어 버전 관리를 추적 할 수 있으며 응용 프로그램이 들어오는 데이터를 올바른 방식으로 해석하고 있는지 쉽게 확인할 수 있습니다.
    /// ref code : https://us.v-cdn.net/5021484/uploads/editor/i2/fj958zeqdhf8.png
    /// </summary>
    public class PacketHeader : IF1PacketObject
    {
        public short m_packetFormat;         // 2018
        public byte m_packetVersion;        // Version of this packet type, all start from 1
        public byte m_packetId;             // Identifier for the packet type, see below
        public long m_sessionUID;           // Unique identifier for the session
        public float m_sessionTime;          // Session timestamp
        public int m_frameIdentifier;      // Identifier for the frame the data was retrieved on
        public byte m_playerCarIndex;       // Index of player's car in the array
        
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
