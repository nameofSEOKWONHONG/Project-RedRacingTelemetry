using System;

namespace F12018UdpTelemetry {
    public class F12018PacketInstance {
        private static F12018PacketInstance _instance;
        private static object _syncObj = new object();
        public static F12018PacketInstance Instance {
            get{
                if(_instance == null) {
                    lock(_syncObj) {
                        if(_instance == null) {
                            _instance = new F12018PacketInstance();

                        }
                    }
                }

                return _instance;
            }
        }

        public PacketHeader PacketHeader {get;set;}
        public CarMotionData CarMotionData {get;set;}
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

        public PacketCarTelemetryData PacketCarTelemetryData {get;set;}

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

        private F12018PacketInstance() {
            
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
}