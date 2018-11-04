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
                value.m_worldPositionX /= 32767.0f;

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

        private PacketMotionData _packetMotionData;
        public PacketMotionData PacketMotionData {
            get
            {
                return _packetMotionData;
            }
            set
            {
                _packetMotionData = value;
                OnF1PacketMotionDataReceive(this, new F1PacketMotionDataEventArgs(0, _packetMotionData));
            }
        }
        public event EventHandler<F1PacketMotionDataEventArgs> F1PacketMotionDataReceived;
        protected virtual void OnF1PacketMotionDataReceive(object sender, F1PacketMotionDataEventArgs e)
        {
            if (F1PacketMotionDataReceived != null)
            {
                F1PacketMotionDataReceived(this, e);
            }
        }
        

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
                OnF1LapDataReceive(this, new F1LapDataEventArgs(2, _lapData));
            }
        }
        public event EventHandler<F1LapDataEventArgs> F1LapDataReceived;
        protected virtual void OnF1LapDataReceive(object sender, F1LapDataEventArgs e)
        {
            if (F1LapDataReceived != null)
            {
                F1LapDataReceived(this, e);
            }
        }
        #endregion

        public PacketLapData PacketLapData { get; set; }
        public PacketEventData PacketEventData { get; set; }

        private ParticipantData _participantData;
        public ParticipantData ParticipantData {
            get
            {
                return _participantData;
            }
            set
            {
                _participantData = value;
                OnF1ParticipantDataReceive(this, new F1ParticipantDataEventArgs(4, _participantData));
            }
        }
        public event EventHandler<F1ParticipantDataEventArgs> F1ParticipantDataReceived;
        protected virtual void OnF1ParticipantDataReceive(object sender, F1ParticipantDataEventArgs e)
        {
            if (F1ParticipantDataReceived != null)
            {
                F1ParticipantDataReceived(this, e);
            }
        }

        public PacketParticipantsData PacketParticipantsData { get; set; }

        private CarSetupData _carSetupData;
        public CarSetupData CarSetupData {
            get
            {
                return _carSetupData;
            }
            set
            {
                _carSetupData = value;
                OnF1CarSetupDataReceive(this, new F1CarSetupEventArgs(5, _carSetupData));
            }
        }
        public event EventHandler<F1CarSetupEventArgs> F1CarSetupDataReceived;
        protected virtual void OnF1CarSetupDataReceive(object sender, F1CarSetupEventArgs e)
        {
            if (F1CarSetupDataReceived != null)
            {
                F1CarSetupDataReceived(this, e);
            }
        }

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
                OnF1CarStatusDataReceivee(this, new F1CarStatusDataEventArgs(7, _carStatusData));
            }
        }
        public event EventHandler<F1CarStatusDataEventArgs> F1CarStatusDataReceived;
        protected virtual void OnF1CarStatusDataReceivee(object sender, F1CarStatusDataEventArgs e)
        {
            if (F1CarStatusDataReceived != null)
            {
                F1CarStatusDataReceived(this, e);
            }
        }

        private PacketCarStatusData _packetCarStatusData;
        public PacketCarStatusData PacketCarStatusData {
            get {
                return _packetCarStatusData;
            }
            set {
                _packetCarStatusData = value;
            }
        }

        #endregion

        private F12018UDPMgr() {

        }
    }

    #region eventargs
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

    public class F1PacketMotionDataEventArgs : EventArgs
    {
        public byte packetCode;
        public PacketMotionData packetStruct;

        public F1PacketMotionDataEventArgs(byte packetCode, PacketMotionData packetStruct)
        {
            this.packetCode = packetCode;
            this.packetStruct = packetStruct;
        }
    }

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

    public class F1LapDataEventArgs : EventArgs
    {
        public byte packetCode;
        public LapData packetStruct;

        public F1LapDataEventArgs(byte packetCode, LapData packetStruct)
        {
            this.packetCode = packetCode;
            this.packetStruct = packetStruct;
        }
    }

    public class F1ParticipantDataEventArgs : EventArgs
    {
        public byte packetCode;
        public ParticipantData packetStruct;

        public F1ParticipantDataEventArgs(byte packetCode, ParticipantData packetStruct)
        {
            this.packetCode = packetCode;
            this.packetStruct = packetStruct;
        }
    }

    public class F1CarSetupEventArgs : EventArgs
    {
        public byte packetCode;
        public CarSetupData packetStruct;

        public F1CarSetupEventArgs(byte packetCode, CarSetupData packetStruct)
        {
            this.packetCode = packetCode;
            this.packetStruct = packetStruct;
        }
    }

    

    public class F1CarStatusDataEventArgs : EventArgs
    {
        public byte packetCode;
        public CarStatusData packetStruct;

        public F1CarStatusDataEventArgs(byte packetCode, CarStatusData packetStruct)
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