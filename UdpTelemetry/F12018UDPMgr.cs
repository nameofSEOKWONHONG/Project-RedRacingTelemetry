using System;

namespace F12018UdpTelemetry {
    public class F12018UDPMgr {
        private static F12018UDPMgr _instance;
        private static object _syncObj = new object();

        public static F12018UDPMgr Instance {
            get {
                if (_instance == null) {
                    lock (_syncObj) {
                        if (_instance == null) {
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
                F1CarMotionDataReceived(this, new F1CarMotionDataEventArgs(1, _carMotionData));
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

        public PacketMotionData PacketMotionData { get; set; }
        public MarshalZone MarshalZone { get; set; }

        #region packetsessiondata
        private PacketSessionData _packetSessionData;
        public PacketSessionData PacketSessionData {
            get
            {
                return _packetSessionData;
            }
            set
            {
                _packetSessionData = value;
                OnF1PacketSessionDataReceive(this, new F1PacketSessionDataEventArgs(1, _packetSessionData));
            }
        }
        public event EventHandler<F1PacketSessionDataEventArgs> F1PacketSessionDataReceived;
        protected virtual void OnF1PacketSessionDataReceive(object sender, F1PacketSessionDataEventArgs e)
        {
            if (F1PacketSessionDataReceived != null)
            {
                OnF1PacketSessionDataReceive(this, e);
            }
        }
        #endregion

        #region lapdata
        private LapData _lapData;
        public LapData LapData {
            get
            {
                return _lapData;
            }
            set
            {
                _lapData = value;
                OnF1PacketLapDataReceive(this, new F1PacketLapDataEventArgs(2, _lapData));
            }
        }
        public event EventHandler<F1PacketLapDataEventArgs> F1PacketLapDataReceived;
        protected virtual void OnF1PacketLapDataReceive(object sender, F1PacketLapDataEventArgs e)
        {
            if (F1PacketLapDataReceived != null)
            {
                OnF1PacketLapDataReceive(this, e);
            }
        }
        #endregion

        public PacketLapData PacketLapData { get; set; }
        public PacketEventData PacketEventData { get; set; }
        public ParticipantData ParticipantData { get; set; }
        public PacketParticipantsData PacketParticipantsData { get; set; }
        public CarSetupData CarSetupData { get; set; }
        public PacketCarSetupData PacketCarSetupData { get; set; }

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

        public PacketCarTelemetryData PacketCarTelemetryData { get; set; }

        #region carstatusdata
        public CarStatusData _carStatusData;
        public CarStatusData CarStatusData {
            get {
                return _carStatusData;
            }
            set {
                _carStatusData = value;
            }
        }

        private PacketCarStatusData _packetCarStatusData;
        public PacketCarStatusData PacketCarStatusData {
            get {
                return _packetCarStatusData;
            }
            set {
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
        public byte packetCode;
        public PacketHeader packetStruct;

        public F1PacketHeaderDataEventArgs(byte packetCode, PacketHeader packetStruct)
        {
            this.packetCode = packetCode;
            this.packetStruct = packetStruct;
        }
    }

    public class F1PacketSessionDataEventArgs : EventArgs
    {
        public byte packetCode;
        public PacketSessionData packetStruct;

        public F1PacketSessionDataEventArgs(byte packetCode, PacketSessionData packetStruct)
        {
            this.packetCode = packetCode;
            this.packetStruct = packetStruct;
        }
    }

    public class F1PacketLapDataEventArgs : EventArgs
    {
        public byte packetCode;
        public LapData packetStruct;

        public F1PacketLapDataEventArgs(byte packetCode, LapData packetStruct)
        {
            this.packetCode = packetCode;
            this.packetStruct = packetStruct;
        }
    }

    public class F1CarMotionDataEventArgs : EventArgs
    {
        public byte packetCode;
        public CarMotionData packetStruct;

        public F1CarMotionDataEventArgs(byte packetCode, CarMotionData packetStruct)
        {
            this.packetCode = packetCode;
            this.packetStruct = packetStruct;
        }
    }

    public class F1PacketCarStatusDataEventArgs : EventArgs
    {
        public byte packetCode;
        public PacketCarStatusData packetStruct;

        public F1PacketCarStatusDataEventArgs(byte packetCode, PacketCarStatusData packetStruct)
        {
            this.packetCode = packetCode;
            this.packetStruct = packetStruct;
        }
    }

    public class F1CarTelemetryDataEventArgs : EventArgs
    {
        public byte packetCode;
        public CarTelemetryData packetStruct;

        public F1CarTelemetryDataEventArgs(byte packetCode, CarTelemetryData packetStruct)
        {
            this.packetCode = packetCode;
            this.packetStruct = packetStruct;
        }
    }
    #endregion
}