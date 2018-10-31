using System;

namespace F12018UdpTelemetry {
    public class F12018UDPMgr {
        private static F12018UDPMgr _instance;
        private static object _syncObj = new object();
        
        public static F12018UDPMgr Instance {
            get{
                if(_instance == null) {
                    lock(_syncObj) {
                        if(_instance == null) {
                            _instance = new F12018UDPMgr();

                        }
                    }
                }

                return _instance;
            }
        }

        #region packetheader
        private PacketHeader _packetHeader;
        public PacketHeader PacketHeader {
            get
            {
                return _packetHeader;
            }
            set
            {
                _packetHeader = value;
                OnF1PacketHeaderDataReceive(this, new F1PacketHeaderDataEventArgs(0, _packetHeader));
            }
        }
        public event EventHandler<F1PacketHeaderDataEventArgs> F1PacketHeaderDataReceived;
        protected virtual void OnF1PacketHeaderDataReceive(object sender, F1PacketHeaderDataEventArgs e)
        {
            if (F1PacketHeaderDataReceived != null)
            {
                F1PacketHeaderDataReceived(this, e);
            }
        }
        #endregion

        #region carmotiondata
        public CarMotionData _carMotionData;
        public CarMotionData CarMotionData
        {
            get
            {
                return _carMotionData;
            }
            set
            {
                _carMotionData = value;
            }
        }
        public event EventHandler<F1CarMotionDataEventArgs> F1CarMotionDataReceived;
        protected virtual void OnF1CarMotionDataReceive(object sender, F1CarMotionDataEventArgs e)
        {
            if (F1CarMotionDataReceived != null)
            {
                F1CarMotionDataReceived(this, e);
            }
        }
        #endregion

        public PacketMotionData PacketMotionData {get;set;}
        public MarshalZone MarshalZone {get;set;}
        public PacketSessionData PacketSessionData {get;set;}
        public LapData LapData {get;set;}
        public PacketLapData PacketLapData {get;set;}
        public PacketEventData PacketEventData {get;set;}
        public ParticipantData ParticipantData {get;set;}
        public PacketParticipantsData PacketParticipantsData {get;set;}
        public CarSetupData CarSetupData {get;set;}
        public PacketCarSetupData PacketCarSetupData {get;set;}

        #region cartelemetrydata
        private CarTelemetryData _carTelemetryData;
        public CarTelemetryData CarTelemetryData {
            get
            {
                return _carTelemetryData;
            }
            set
            {
                _carTelemetryData = value;
                OnF1CarTelemetryDataReceive(this, new F1CarTelemetryDataEventArgs(6, _carTelemetryData));
            }
        }
        public event EventHandler<F1CarTelemetryDataEventArgs> F1F1CarTelemetryDataReceived;
        protected virtual void OnF1CarTelemetryDataReceive(object sender, F1CarTelemetryDataEventArgs e)
        {
            if (F1F1CarTelemetryDataReceived != null)
            {
                F1F1CarTelemetryDataReceived(this, e);
            }
        }
        #endregion

        public PacketCarTelemetryData PacketCarTelemetryData {get;set;}

        #region carstatusdata
        public CarStatusData _carStatusData;
        public CarStatusData CarStatusData {
            get{
                return _carStatusData;
            }
            set{
                _carStatusData = value;
            }
        }

        private PacketCarStatusData _packetCarStatusData;
        public PacketCarStatusData PacketCarStatusData {
            get{
                return _packetCarStatusData;
            }
            set{
                _packetCarStatusData = value;
                OnF1PacketCarStatusDataReceive(this, new F1PacketCarStatusDataEventArgs(7, _packetCarStatusData));
            }
        }
        public event EventHandler<F1PacketCarStatusDataEventArgs> F1PacketCarStatusDataReceived;
        protected virtual void OnF1PacketCarStatusDataReceive(object sender, F1PacketCarStatusDataEventArgs e)
        {
            if (F1PacketCarStatusDataReceived != null)
            {
                F1PacketCarStatusDataReceived(this, e);
            }
        }
        #endregion

        private F12018UDPMgr() {
            
        }
    }

    #region eventargs
    public class F1PacketHeaderDataEventArgs : EventArgs
    {
        public byte packetCode { get; set; }
        public PacketHeader packetStruct { get; set; }

        public F1PacketHeaderDataEventArgs(byte packetCode, PacketHeader packetStruct)
        {
            this.packetCode = packetCode;
            this.packetStruct = packetStruct;
        }
    }

    public class F1CarMotionDataEventArgs : EventArgs
    {
        public byte packetCode { get; set; }
        public CarMotionData packetStruct { get; set; }

        public F1CarMotionDataEventArgs(byte packetCode, CarMotionData packetStruct)
        {
            this.packetCode = packetCode;
            this.packetStruct = packetStruct;
        }
    }

    public class F1PacketCarStatusDataEventArgs : EventArgs
    {
        public byte packetCode {get;set;}
        public PacketCarStatusData packetStruct { get; set; }

        public F1PacketCarStatusDataEventArgs(byte packetCode, PacketCarStatusData packetStruct)
        {
            this.packetCode = packetCode;
            this.packetStruct = packetStruct;
        }
    }

    public class F1CarTelemetryDataEventArgs : EventArgs
    {
        public byte packetCode { get; set; }
        public CarTelemetryData packetStruct { get; set; }

        public F1CarTelemetryDataEventArgs(byte packetCode, CarTelemetryData packetStruct)
        {
            this.packetCode = packetCode;
            this.packetStruct = packetStruct;
        }
    }
    #endregion
}