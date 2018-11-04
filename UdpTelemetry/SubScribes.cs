using Project_RedRacingTelemetry.Abstract;
using Project_RedRacingTelemetry.Events;
using Project_RedRacingTelemetry.Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project_RedRacingTelemetry
{
    public class SubScribes
    {
        public PacketHeader PacketHeader { get; set; } = new PacketHeader();
        public CarMotionData CarMotionData { get; set; } = new CarMotionData();

        public SubScribes(byte[] buffer)
        {
            IF1PacketObject f1PacketObject = new PacketHeader();
            f1PacketObject.Parse(buffer);

            
            switch(((PacketHeader)f1PacketObject).m_packetId) {
                case 0:
                    IF1PacketObject f1PacketObject_ = CarMotionData;
                    f1PacketObject_.Parse(buffer);
                    break;
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    break;
                case 5:
                    break;
                case 6:
                    break;
                case 7:
                    break;
                default:
                    break;
            }
        }

        private void F1PacketObject_OnPacketReceived(object sender, EventArgs e)
        {
            PacketHeader = ((F1PacketEventEventArgs)e).packetHeader;
        }
    }
}
