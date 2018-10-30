using MiniBinaryParser;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace F12018UdpTelemetry
{
    public class F12018UdpSocket
    {
        private Socket _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        private const int bufSize = 8 * 1024;
        private State state = new State();
        private EndPoint epFrom = new IPEndPoint(IPAddress.Any, 0);
        private AsyncCallback recv = null;

        public class State
        {
            public byte[] buffer = new byte[bufSize];
        }

        public void Server(string address, int port)
        {
            _socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.ReuseAddress, true);
            _socket.Bind(new IPEndPoint(IPAddress.Parse(address), port));
            Receive();            
        }

        public void Client(string address, int port)
        {
            _socket.Connect(IPAddress.Parse(address), port);
            Receive();            
        }

        public void Send(string text)
        {
            byte[] data = Encoding.ASCII.GetBytes(text);
            _socket.BeginSend(data, 0, data.Length, SocketFlags.None, (ar) =>
            {
                State so = (State)ar.AsyncState;
                int bytes = _socket.EndSend(ar);
                Console.WriteLine("SEND: {0}, {1}", bytes, text);
            }, state);
        }

        private void Receive()
        {
            _socket.BeginReceiveFrom(state.buffer, 0, bufSize, SocketFlags.None, ref epFrom, recv = (ar) =>
            {
                State so = (State)ar.AsyncState;
                int bytes = _socket.EndReceiveFrom(ar, ref epFrom);
                _socket.BeginReceiveFrom(so.buffer, 0, bufSize, SocketFlags.None, ref epFrom, recv, so);
                byte[] data = so.buffer;
                //Array.Reverse(so.buffer);

                PacketHeader packetHeader = new PacketHeader();
                so.buffer.Parse(Endian.Little,
                    From.UInt16((m_packetFormat) => packetHeader.m_packetFormat = m_packetFormat),
                    From.Byte((m_packetVersion) => packetHeader.m_packetVersion = m_packetVersion),
                    From.Byte((m_packetId) => packetHeader.m_packetId = m_packetId),
                    From.UInt32((m_sessionUID) => packetHeader.m_sessionUID = m_sessionUID),
                    From.Int32((m_sessionTime) => packetHeader.m_sessionTime = m_sessionTime),
                    From.UInt16((m_frameIdentifier) => packetHeader.m_frameIdentifier = m_frameIdentifier),
                    From.Byte((m_playerCarIndex) => packetHeader.m_playerCarIndex = m_playerCarIndex)
                );

                Console.WriteLine($"m_packetId:{packetHeader.m_packetId}");

                switch (packetHeader.m_packetId)
                {
                    case 0:
                        Console.WriteLine("Motion");
                        break;
                    case 1:
                        Console.WriteLine("Session");
                        PacketSessionData packetSessionData = new PacketSessionData();
                        so.buffer.Parse(Endian.Little,
                            From.((m_packetFormat) => packetHeader.m_packetFormat = m_packetFormat),
                            From.Byte((m_packetVersion) => packetHeader.m_packetVersion = m_packetVersion),
                            From.Byte((m_packetId) => packetHeader.m_packetId = m_packetId),
                            From.UInt32((m_sessionUID) => packetHeader.m_sessionUID = m_sessionUID),
                            From.Int32((m_sessionTime) => packetHeader.m_sessionTime = m_sessionTime),
                            From.UInt16((m_frameIdentifier) => packetHeader.m_frameIdentifier = m_frameIdentifier),
                            From.Byte((m_playerCarIndex) => packetHeader.m_playerCarIndex = m_playerCarIndex),
                            From.Byte((m_weather) => packetSessionData.m_weather = m_weather)
                        );
                        Console.WriteLine($"m_weather:{packetSessionData.m_weather}");
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


                //using (var stream = new MemoryStream(so.buffer))
                //{
                //    using (BinaryReader rdr = new BinaryReader(stream))
                //    {
                //        var packetHeader = (PacketHeader)F1PacketConvertUtils.FromBinaryReader<PacketHeader>(rdr);

                //        switch(packetHeader.m_packetId)
                //        {
                //            case 0:
                //                Console.WriteLine("Motion");
                //                F12018PacketInstance.Instance.CarMotionData = F1PacketConvertUtils.FromBinaryReader<CarMotionData>(rdr);
                //                break;
                //            case 1:
                //                Console.WriteLine("Session");
                //                F12018PacketInstance.Instance.PacketSessionData = F1PacketConvertUtils.FromBinaryReader<PacketSessionData>(rdr);
                //                break;
                //            case 2:
                //                Console.WriteLine("Lap Data");
                //                F12018PacketInstance.Instance.LapData = F1PacketConvertUtils.FromBinaryReader<LapData>(rdr);
                //                break;
                //            //case 3:
                //            //    Console.WriteLine("Event");
                //            //    F12018PacketInstance.Instance.PacketEventData = (PacketEventData)F1PacketConvertUtils.FromBinaryReader<PacketEventData>(rdr);
                //            //    break;
                //            case 4:
                //                Console.WriteLine("Participants");
                //                F12018PacketInstance.Instance.ParticipantData = F1PacketConvertUtils.FromBinaryReader<ParticipantData>(rdr);
                //                break;
                //            case 5:
                //                Console.WriteLine("Car Setups");
                //                F12018PacketInstance.Instance.CarSetupData = F1PacketConvertUtils.FromBinaryReader<CarSetupData>(rdr);
                //                break;
                //            //case 6:
                //            //    Console.WriteLine("Car Telemetry");
                //            //    F12018PacketInstance.Instance.CarTelemetryData = F1PacketConvertUtils.FromBinaryReader<CarTelemetryData>(rdr);
                //            //    break;
                //            case 7:
                //                Console.WriteLine("Car Status");
                //                F12018PacketInstance.Instance.CarStatusData = F1PacketConvertUtils.FromBinaryReader<CarStatusData>(rdr);
                //                break;
                //            default:
                //                break;
                //        }
                //        rdr.Close();
                //    }

                //    stream.Flush();
                //    stream.Close();
                //}
            }, state);
        }
    }
}
