using System;
using System.Collections.Generic;
using System.Text;

namespace Project_RedRacingTelemetry.Abstract
{
    public interface IF1PacketObject
    {
        event EventHandler OnPacketReceived;
        void Parse(byte[] buffer);
    }
}
