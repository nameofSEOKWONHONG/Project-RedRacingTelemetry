using Project_RedRacingTelemetry.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project_RedRacingTelemetry.Objects
{
    public class CarMotionImpl : IF1PacketObject
    {
        public event EventHandler OnPacketReceived;

        public void Parse(byte[] buffer)
        {
            F1BytesAccessor bytesAccessor = new F1BytesAccessor(buffer, true);
        }
    }
}
