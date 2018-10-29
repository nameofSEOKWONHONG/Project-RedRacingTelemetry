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
        public CarTelemetryData CarTelemetryData {get;set;}
        public PacketCarTelemetryData PacketCarTelemetryData {get;set;}

        public CarStatusData _carStatusData;
        public CarStatusData CarStatusData {
            get{
                return _carStatusData;
            }
            set{
                _carStatusData = value;
                OnF1PacketDataReceived(this, new F1PacketEventArgs(6, _packetCarStatusData));
            }
        }

        private PacketCarStatusData _packetCarStatusData;
        public PacketCarStatusData PacketCarStatusData {
            get{
                return _packetCarStatusData;
            }
            set{
                _packetCarStatusData = value;                
                OnF1PacketDataReceived(this, new F1PacketEventArgs(7, _packetCarStatusData));
            }
        }

        public event EventHandler<F1PacketEventArgs> F1PacketDataReceive;

        protected virtual void OnF1PacketDataReceived(object sender, F1PacketEventArgs e)
        {
            if (F1PacketDataReceive != null)
            {
                F1PacketDataReceive(this, e);
            }
        }

        private F12018PacketInstance() {
            
        }
    }

    public class F1PacketEventArgs : EventArgs
    {
        public byte packetCode {get;set;}
        public object packetStruct { get; set; }
        public string Fps { get; set; }
        public string Frame { get; set; }

        public F1PacketEventArgs(byte packetCode, object packetStruct)
        {
            this.packetCode = packetCode;
            this.packetStruct = packetStruct;
        }
    }
}