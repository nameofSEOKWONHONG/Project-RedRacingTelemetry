using Project_RedRacingTelemetry.Abstract;
using Project_RedRacingTelemetry.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project_RedRacingTelemetry.Objects
{
    [Serializable]
    public class PacketSessionData : IF1PacketObject
    {
        public PacketHeader m_header;               	// Header
        public byte m_weather;              	// Weather - 0 = clear, 1 = light cloud, 2 = overcast  // 3 = light rain, 4 = heavy rain, 5 = storm
        public sbyte m_trackTemperature;    	// Track temp. in degrees celsius
        public sbyte m_airTemperature;      	// Air temp. in degrees celsius
        public byte m_totalLaps;           	// Total number of laps in this race
        public byte m_trackLength;           	// Track length in metres
        public byte m_sessionType;         	// 0 = unknown, 1 = P1, 2 = P2, 3 = P3, 4 = Short P // 5 = Q1, 6 = Q2, 7 = Q3, 8 = Short Q, 9 = OSQ // 10 = R, 11 = R2, 12 = Time Trial
        /// <summary>
        /// ref code : https://us.v-cdn.net/5021484/uploads/editor/t2/i3thn8e58vgt.png
        /// </summary>
        public sbyte m_trackId;         		// -1 for unknown, 0-21 for tracks, see appendix
        public byte m_era;                  	// Era, 0 = modern, 1 = classic
        public byte m_sessionTimeLeft;    	// Time left in session in seconds
        public byte m_sessionDuration;     	// Session duration in seconds
        public byte m_pitSpeedLimit;      	// Pit speed limit in kilometres per hour
        public byte m_gamePaused;               // Whether the game is paused
        public byte m_isSpectating;        	// Whether the player is spectating
        public byte m_spectatorCarIndex;  	// Index of the car being spectated
        public byte m_sliProNativeSupport;	// SLI Pro support, 0 = inactive, 1 = active
        public byte m_numMarshalZones;         	// Number of marshal zones to follow
        public MarshalZone[] m_marshalZones;         // List of marshal zones – max 21
        public byte m_safetyCarStatus;          // 0 = no safety car, 1 = full safety car // 2 = virtual safety car
        public byte m_networkGame;              // 0 = offline, 1 = online

        public event EventHandler packetReceived;

        object objectLock = new Object();

        event EventHandler OnPacketReceived
        {
            add
            {
                lock (objectLock)
                {
                    packetReceived += value;
                }
            }
            remove
            {
                lock (objectLock)
                {
                    packetReceived -= value;
                }
            }
        }

        public void Parse()
        {
            // Raise IDrawingObject's event before the object is drawn.
            packetReceived?.Invoke(this, new F1PacketEventEventArgs(packetSessionData:this));
        }
    }
}
