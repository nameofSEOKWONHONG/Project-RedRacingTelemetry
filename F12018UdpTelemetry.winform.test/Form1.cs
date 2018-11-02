using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace F12018UdpTelemetry.winform.test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            F12018UdpSocket c = new F12018UdpSocket();
            c.Server("192.168.0.11", 20777);
            F12018UDPMgr.Instance.F1PacketHeaderDataReceived += Instance_F1PacketHeaderDataReceived;
            //F12018UDPMgr.Instance.F1CarMotionDataReceived += Instance_F1CarMotionDataReceived;
            //F12018UDPMgr.Instance.F1PacketMotionDataReceived += Instance_F1PacketMotionDataReceived;
            //F12018UDPMgr.Instance.F1PacketSessionDataReceived += Instance_F1PacketSessionDataReceived;
            //F12018UDPMgr.Instance.F1LapDataReceived += Instance_F1LapDataReceived;
            //F12018UDPMgr.Instance.F1ParticipantDataReceived += Instance_F1ParticipantDataReceived;
            //F12018UDPMgr.Instance.F1CarSetupDataReceived += Instance_F1CarSetupDataReceived;
            //F12018UDPMgr.Instance.F1CarStatusDataReceived += Instance_F1CarStatusDataReceived;

            base.OnLoad(e);
        }

        private void Instance_F1PacketHeaderDataReceived(object sender, F1PacketHeaderDataEventArgs e)
        {
            this.Invoke(new Action(() => {
                this.richTextBox1.Clear();
                var packetStr = $"m_packetFormat:{e.packetStruct.m_packetFormat}" + Environment.NewLine;
                packetStr += $"m_packetId:{e.packetStruct.m_packetId}" + Environment.NewLine;
                packetStr += $"m_playerCarIndex:{e.packetStruct.m_playerCarIndex}" + Environment.NewLine;
                this.richTextBox1.Text = packetStr;
            }));
        }

        private void Instance_F1LapDataReceived(object sender, F1LapDataEventArgs e)
        {
            this.Invoke(new Action(() => {
                this.richTextBox1.Clear();
                var packetStr = $"m_carPosition:{e.packetStruct.m_carPosition}" + Environment.NewLine;
                this.richTextBox1.Text = packetStr;
            }));
        }

        private void Instance_F1PacketSessionDataReceived(object sender, F1PacketSessionDataEventArgs e)
        {
            this.Invoke(new Action(() => {
                this.richTextBox1.Clear();
                var packetStr = $"m_weather:{e.packetStruct.m_weather}" + Environment.NewLine;
                this.richTextBox1.Text = packetStr;
            }));
        }

        private void Instance_F1PacketMotionDataReceived(object sender, F1PacketMotionDataEventArgs e)
        {
            this.Invoke(new Action(() => {
                this.richTextBox1.Clear();
                //var packetStr = $"RL:{e.packetStruct.m_suspensionAcceleration[0]}" + Environment.NewLine;
                //packetStr += $"RR:{e.packetStruct.m_suspensionAcceleration[1]}" + Environment.NewLine;
                //packetStr += $"FL:{e.packetStruct.m_suspensionAcceleration[2]}" + Environment.NewLine;
                //packetStr += $"FR:{e.packetStruct.m_suspensionAcceleration[3]}" + Environment.NewLine;
                var packetStr = $"m_worldForwardDirX:{e.packetStruct.m_carMotionData[F12018UDPMgr.Instance.PacketHeader.m_packetId].m_worldForwardDirX}" + Environment.NewLine;
                packetStr += $"m_worldForwardDirY:{e.packetStruct.m_carMotionData[F12018UDPMgr.Instance.PacketHeader.m_packetId].m_worldForwardDirY}" + Environment.NewLine;
                packetStr += $"m_worldForwardDirZ:{e.packetStruct.m_carMotionData[F12018UDPMgr.Instance.PacketHeader.m_packetId].m_worldForwardDirZ}" + Environment.NewLine;
                packetStr += $"m_worldPositionX:{e.packetStruct.m_carMotionData[F12018UDPMgr.Instance.PacketHeader.m_packetId].m_worldPositionX}" + Environment.NewLine;
                packetStr += $"m_worldPositionY:{e.packetStruct.m_carMotionData[F12018UDPMgr.Instance.PacketHeader.m_packetId].m_worldPositionY}" + Environment.NewLine;
                packetStr += $"m_worldPositionZ:{e.packetStruct.m_carMotionData[F12018UDPMgr.Instance.PacketHeader.m_packetId].m_worldPositionZ}" + Environment.NewLine;
                this.richTextBox1.Text = packetStr;
            }));
        }
    }
}
