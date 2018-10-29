using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace F12018UdpTelemetry
{
    public class UDPSocket
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

                Array.Reverse(so.buffer);
                using (var stream = new MemoryStream(so.buffer))
                {
                    using (BinaryReader rdr = new BinaryReader(stream))
                    {
                        var packetHeader = F1PacketConvertUtils.FromBinaryReader<PacketHeader>(rdr);

                        Console.WriteLine($"m_packetId:{packetHeader.m_packetId}");

                        switch(packetHeader.m_packetId)
                        {
                            case 0:
                                Console.WriteLine("Motion");
                                break;
                            case 1:
                                Console.WriteLine("Session");
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
                        rdr.Close();
                    }

                    stream.Flush();
                    stream.Close();
                }
            }, state);
        }
    }
}
