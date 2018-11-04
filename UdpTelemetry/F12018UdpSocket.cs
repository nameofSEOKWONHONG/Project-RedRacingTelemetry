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
                
                //set endian little
                Array.Reverse(so.buffer);

                using (var stream = new MemoryStream(so.buffer))
                {
                    using (BinaryReader rdr = new BinaryReader(stream))
                    {
                        F12018UDPMgr.Instance.PacketHeader = F1PacketConvertUtils.FromBinaryReader<PacketHeader>(rdr);

                        switch (F12018UDPMgr.Instance.PacketHeader.m_packetId)
                        {
                            case 0:
                                //Console.WriteLine("Motion");
                                //F12018UDPMgr.Instance.CarMotionData = F1PacketConvertUtils.FromBinaryReader<CarMotionData>(rdr);
                                F12018UDPMgr.Instance.PacketMotionData = F1PacketConvertUtils.FromBinaryReader<PacketMotionData>(rdr);
                                break;
                            case 1:
                                //Console.WriteLine("Session");
                                F12018UDPMgr.Instance.PacketSessionData = F1PacketConvertUtils.FromBinaryReader<PacketSessionData>(rdr);
                                break;
                            case 2:
                                //Console.WriteLine("Lap Data");
                                F12018UDPMgr.Instance.LapData = F1PacketConvertUtils.FromBinaryReader<LapData>(rdr);
                                break;
                            case 3:
                                //array memeory protected error
                                Console.WriteLine("Event");
                                //F12018PacketInstance.Instance.PacketEventData = (PacketEventData)F1PacketConvertUtils.FromBinaryReader<PacketEventData>(rdr);
                                break;
                            case 4:
                                //Console.WriteLine("Participants");
                                F12018UDPMgr.Instance.ParticipantData = F1PacketConvertUtils.FromBinaryReader<ParticipantData>(rdr);
                                break;
                            case 5:
                                //Console.WriteLine("Car Setups");
                                F12018UDPMgr.Instance.CarSetupData = F1PacketConvertUtils.FromBinaryReader<CarSetupData>(rdr);
                                break;
                            case 6:
                                //array memory protected error
                                //Console.WriteLine("Car Telemetry");
                                //F12018PacketInstance.Instance.CarTelemetryData = F1PacketConvertUtils.FromBinaryReader<CarTelemetryData>(rdr);
                                break;
                            case 7:
                                //Console.WriteLine("Car Status");
                                F12018UDPMgr.Instance.CarStatusData = F1PacketConvertUtils.FromBinaryReader<CarStatusData>(rdr);
                                break;
                            default:
                                break;
                        }
                        rdr.Close();
                    }

                    stream.Flush();
                    stream.Close();
                }
            }, state);
        }
    }
}
