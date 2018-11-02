using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

/// <summary>
/// reference site : https://forums.codemasters.com/discussion/136948/f1-2018-udp-specification
/// </summary>
namespace F12018UdpTelemetry
{
    /// <summary>
    ///  2018의 주요 변경 사항은 여러 패킷 유형의 도입입니다. 
    /// 이제 각 패킷은 모든 것을 포함하는 하나의 패킷을 가지지 않고 다른 유형의 데이터를 전달할 수 있습니다. 
    /// 각 패킷에도 헤더가 추가되어 버전 관리를 추적 할 수 있으며 응용 프로그램이 들어오는 데이터를 올바른 방식으로 해석하고 있는지 쉽게 확인할 수 있습니다.
    /// ref code : https://us.v-cdn.net/5021484/uploads/editor/i2/fj958zeqdhf8.png
    /// </summary>
    [Serializable]
    public struct PacketHeader : ISerializable
    {
        public UInt16    m_packetFormat;         // 2018
        public byte      m_packetVersion;        // Version of this packet type, all start from 1
        public byte      m_packetId;             // Identifier for the packet type, see below
        public UInt64    m_sessionUID;           // Unique identifier for the session
        public float     m_sessionTime;          // Session timestamp
        public uint      m_frameIdentifier;      // Identifier for the frame the data was retrieved on
        public byte m_playerCarIndex;       // Index of player's car in the array

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("m_packetFormat", m_packetFormat);
            info.AddValue("m_packetVersion", m_packetVersion);
            info.AddValue("m_packetId", m_packetId);
            info.AddValue("m_sessionUID", m_sessionUID);
            info.AddValue("m_sessionTime", m_sessionTime);
            info.AddValue("m_frameIdentifier", m_frameIdentifier);
            info.AddValue("m_playerCarIndex", m_playerCarIndex);
        }
    }

    /// <summary>
    ///  모션 패킷은 구동되는 모든 자동차에 대한 물리 데이터를 제공합니다. 
    /// 모션 플랫폼 설정을 유도 할 수 있다는 목표로 운전중인 자동차에 대한 추가 데이터가 있습니다.
    /// NB 아래의 정규화 된 벡터의 경우 float 값으로 변환하면 32767.0f로 나눕니다. 
    /// 16 비트 부호있는 값은 방향 값이 항상 -1.0f와 1.0f 사이에 있다는 가정하에 데이터를 묶는 데 사용됩니다.
    /// </summary>
    [Serializable]
    public struct CarMotionData
    {
        public float         m_worldPositionX;           // World space X position
        public float         m_worldPositionY;           // World space Y position
        public float         m_worldPositionZ;           // World space Z position
        public float         m_worldVelocityX;           // Velocity in world space X
        public float         m_worldVelocityY;           // Velocity in world space Y
        public float         m_worldVelocityZ;           // Velocity in world space Z
        public Int16         m_worldForwardDirX;         // World space forward X direction (normalised)
        public Int16         m_worldForwardDirY;         // World space forward Y direction (normalised)
        public Int16         m_worldForwardDirZ;         // World space forward Z direction (normalised)
        public Int16         m_worldRightDirX;           // World space right X direction (normalised)
        public Int16         m_worldRightDirY;           // World space right Y direction (normalised)
        public Int16         m_worldRightDirZ;           // World space right Z direction (normalised)
        public float         m_gForceLateral;            // Lateral G-Force component
        public float         m_gForceLongitudinal;       // Longitudinal G-Force component
        public float         m_gForceVertical;           // Vertical G-Force component
        public float         m_yaw;                      // Yaw angle in radians
        public float         m_pitch;                    // Pitch angle in radians
        public float         m_roll;                     // Roll angle in radians
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct PacketMotionData
    {
        public PacketHeader    m_header;               // Header
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20, MarshalType = "CarMotionData", MarshalTypeRef = typeof(CarMotionData))]
        public CarMotionData[]   m_carMotionData;    // Data for all cars on track (array length:20)

        // Extra player car ONLY data
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, MarshalType = "float", MarshalTypeRef = typeof(float))]
        public float[]         m_suspensionPosition;       // Note: All wheel arrays have the following order: (array length:4)
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, MarshalType = "float", MarshalTypeRef = typeof(float))]
        public float[]         m_suspensionVelocity;       // RL, RR, FL, FR (array length:4)
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, MarshalType = "float", MarshalTypeRef = typeof(float))]
        public float[]         m_suspensionAcceleration;   // RL, RR, FL, FR (array length:4)
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, MarshalType = "float", MarshalTypeRef = typeof(float))]
        public float[]         m_wheelSpeed;               // Speed of each wheel (array length:4)
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, MarshalType = "float", MarshalTypeRef = typeof(float))]
        public float[]         m_wheelSlip;                // Slip ratio for each wheel (array length:4)
        public float         m_localVelocityX;              // Velocity in local space
        public float         m_localVelocityY;              // Velocity in local space
        public float         m_localVelocityZ;              // Velocity in local space
        public float         m_angularVelocityX;            // Angular velocity x-component
        public float         m_angularVelocityY;            // Angular velocity y-component
        public float         m_angularVelocityZ;            // Angular velocity z-component
        public float         m_angularAccelerationX;        // Angular velocity x-component
        public float         m_angularAccelerationY;        // Angular velocity y-component
        public float         m_angularAccelerationZ;        // Angular velocity z-component
        public float         m_frontWheelsAngle;            // Current front wheels angle in radians
    }

    /// <summary>
    /// SESSION PACKET
    /// The session packet includes details about the current session in progress.
    /// Frequency: 2 per second
    /// Size: 147 bytes
    /// </summary>
    [Serializable]    
    public struct MarshalZone
    {
        public float  m_zoneStart;   // Fraction (0..1) of way through the lap the marshal zone starts
        public sbyte   m_zoneFlag;    // -1 = invalid/unknown, 0 = none, 1 = green, 2 = blue, 3 = yellow, 4 = red
    }

    [Serializable]
    public struct PacketSessionData
    {
        public PacketHeader    m_header;               	// Header
        public byte           m_weather;              	// Weather - 0 = clear, 1 = light cloud, 2 = overcast  // 3 = light rain, 4 = heavy rain, 5 = storm
        public sbyte	    m_trackTemperature;    	// Track temp. in degrees celsius
        public sbyte	    m_airTemperature;      	// Air temp. in degrees celsius
        public byte           m_totalLaps;           	// Total number of laps in this race
        public byte          m_trackLength;           	// Track length in metres
        public byte           m_sessionType;         	// 0 = unknown, 1 = P1, 2 = P2, 3 = P3, 4 = Short P // 5 = Q1, 6 = Q2, 7 = Q3, 8 = Short Q, 9 = OSQ // 10 = R, 11 = R2, 12 = Time Trial
        /// <summary>
        /// ref code : https://us.v-cdn.net/5021484/uploads/editor/t2/i3thn8e58vgt.png
        /// </summary>
        public sbyte            m_trackId;         		// -1 for unknown, 0-21 for tracks, see appendix
        public byte           m_era;                  	// Era, 0 = modern, 1 = classic
        public byte          m_sessionTimeLeft;    	// Time left in session in seconds
        public byte          m_sessionDuration;     	// Session duration in seconds
        public byte           m_pitSpeedLimit;      	// Pit speed limit in kilometres per hour
        public byte           m_gamePaused;               // Whether the game is paused
        public byte           m_isSpectating;        	// Whether the player is spectating
        public byte           m_spectatorCarIndex;  	// Index of the car being spectated
        public byte           m_sliProNativeSupport;	// SLI Pro support, 0 = inactive, 1 = active
        public byte           m_numMarshalZones;         	// Number of marshal zones to follow
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 21, MarshalType = "MarshalZone[]", MarshalTypeRef = typeof(MarshalZone[]))]
        public MarshalZone[]  m_marshalZones;         // List of marshal zones – max 21
        public byte           m_safetyCarStatus;          // 0 = no safety car, 1 = full safety car // 2 = virtual safety car
        public byte           m_networkGame;              // 0 = offline, 1 = online
    }

    /// <summary>
    /// LAP DATA PACKET
    /// The lap data packet gives details of all the cars in the session.
    /// Frequency: Rate as specified in menus
    /// Size: 841 bytes
    /// </summary>
    [Serializable]
    public struct LapData
    {
        public float       m_lastLapTime;           // Last lap time in seconds
        public float       m_currentLapTime;        // Current time around the lap in seconds
        public float       m_bestLapTime;           // Best lap time of the session in seconds
        public float       m_sector1Time;           // Sector 1 time in seconds
        public float       m_sector2Time;           // Sector 2 time in seconds
        public float       m_lapDistance;           // Distance vehicle is around current lap in metres – could be negative if line hasn’t been crossed yet
        public float       m_totalDistance;         // Total distance travelled in session in metres – could be negative if line hasn’t been crossed yet
        public float       m_safetyCarDelta;        // Delta in seconds for safety car
        public byte       m_carPosition;           // Car race position
        public byte       m_currentLapNum;         // Current lap number
        public byte       m_pitStatus;             // 0 = none, 1 = pitting, 2 = in pit area
        public byte       m_sector;                // 0 = sector1, 1 = sector2, 2 = sector3
        public byte       m_currentLapInvalid;     // Current lap invalid - 0 = valid, 1 = invalid
        public byte       m_penalties;             // Accumulated time penalties in seconds to be added
        public byte       m_gridPosition;          // Grid position the vehicle started the race in
        public byte       m_driverStatus;          // Status of driver - 0 = in garage, 1 = flying lap 2 = in lap, 3 = out lap, 4 = on track
        public byte       m_resultStatus;          // Result status - 0 = invalid, 1 = inactive, 2 = active 3 = finished, 4 = disqualified, 5 = not classified 6 = retired
    }

    [Serializable]
    public struct PacketLapData
    {
        public PacketHeader    m_header;              // Header
        public LapData[]         m_lapData;         // Lap data for all cars on track, max =20
    }

    
    /// <summary>
    /// his packet gives details of events that happen during the course of the race.
    /// Frequency: When the event occurs
    /// Size: 25 bytes
    /// ref code : https://us.v-cdn.net/5021484/uploads/editor/3p/n40iwrzvzhwq.jpg
    /// </summary>
    [Serializable]
    public struct PacketEventData
    {
        public PacketHeader    m_header;               // Header
        
        public byte[]           m_eventStringCode;   // Event string code, see above, max=4
    }

    
    /// <summary>
    /// PARTICIPANTS PACKET
    /// This is a list of participants in the race. 
    /// If the vehicle is controlled by AI, then the name will be the driver name. 
    /// If this is a multiplayer game, the names will be the Steam Id on PC, or the LAN name if appropriate. On Xbox One, the names will always be the driver name, on PS4 the name will be the LAN name if playing a LAN game, otherwise it will be the driver name.
    /// Frequency: Every 5 seconds
    /// Size: 1082 bytes
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct ParticipantData
    {
        public byte      m_aiControlled;           // Whether the vehicle is AI (1) or Human (0) controlled
        /// <summary>
        /// ref code : https://us.v-cdn.net/5021484/uploads/editor/sz/hj0lzh1oayyn.png
        /// </summary>
        public byte      m_driverId;               // Driver id - see appendix
        /// <summary>
        /// ref code : https://us.v-cdn.net/5021484/uploads/editor/48/y1yaxadoggmk.png
        /// </summary>
        public byte      m_teamId;                 // Team id - see appendix
        public byte      m_raceNumber;             // Race number of the car
        /// <summary>
        /// ref code : https://us.v-cdn.net/5021484/uploads/editor/0o/9wqezks7xzky.png
        /// </summary>
        public byte      m_nationality;            // Nationality of the driver
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 48, MarshalType = "byte[]", MarshalTypeRef = typeof(byte[]))]
        public byte[]       m_name;               // Name of participant in UTF-8 format – null terminated Will be truncated with … (U+2026) if too long, max=48
    }

    [Serializable]
    public struct PacketParticipantsData
    {
        public PacketHeader    m_header;            // Header
        public byte           m_numCars;           // Number of cars in the data
        public ParticipantData[] m_participants;  //max = 20
    }

    /// <summary>
    /// CAR SETUPS PACKET
    /// This packet details the car setups for each vehicle in the session. Note that in multiplayer games, other player cars will appear as blank, you will only be able to see your car setup and AI cars.
    /// Frequency: Every 5 seconds
    /// Size: 841 bytes
    /// </summary>
    [Serializable]    
    public struct CarSetupData
    {
        public byte     m_frontWing;                // Front wing aero
        public byte     m_rearWing;                 // Rear wing aero
        public byte     m_onThrottle;               // Differential adjustment on throttle (percentage)
        public byte     m_offThrottle;              // Differential adjustment off throttle (percentage)
        public float     m_frontCamber;              // Front camber angle (suspension geometry)
        public float     m_rearCamber;               // Rear camber angle (suspension geometry)
        public float     m_frontToe;                 // Front toe angle (suspension geometry)
        public float     m_rearToe;                  // Rear toe angle (suspension geometry)
        public byte     m_frontSuspension;          // Front suspension
        public byte     m_rearSuspension;           // Rear suspension
        public byte     m_frontAntiRollBar;         // Front anti-roll bar
        public byte     m_rearAntiRollBar;          // Front anti-roll bar
        public byte     m_frontSuspensionHeight;    // Front ride height
        public byte     m_rearSuspensionHeight;     // Rear ride height
        public byte     m_brakePressure;            // Brake pressure (percentage)
        public byte     m_brakeBias;                // Brake bias (percentage)
        public float     m_frontTyrePressure;        // Front tyre pressure (PSI)
        public float     m_rearTyrePressure;         // Rear tyre pressure (PSI)
        public byte     m_ballast;                  // Ballast
        public float     m_fuelLoad;                 // Fuel load
    };

    [Serializable]
    public struct PacketCarSetupData
    {
        PacketHeader    m_header;            // Header

        CarSetupData[]    m_carSetups; //max = 20
    };

    /// <summary>
    /// CAR TELEMETRY PACKET
    /// This packet details telemetry for all the cars in the race. It details various values that would be recorded on the car such as speed, throttle application, DRS etc.
    /// Frequency: Rate as specified in menus
    /// Size: 1085 bytes
    /// </summary>
    [Serializable()]
    public struct  CarTelemetryData
    {
        public byte    m_speed;                      // Speed of car in kilometres per hour
        public byte     m_throttle;                   // Amount of throttle applied (0 to 100)
        public sbyte      m_steer;                      // Steering (-100 (full lock left) to 100 (full lock right))
        public byte     m_brake;                      // Amount of brake applied (0 to 100)
        public byte     m_clutch;                     // Amount of clutch applied (0 to 100)
        public sbyte      m_gear;                       // Gear selected (1-8, N=0, R=-1)
        public byte    m_engineRPM;                  // Engine RPM
        public byte     m_drs;                        // 0 = off, 1 = on
        public byte     m_revLightsPercent;           // Rev lights indicator (percentage)        
        public ushort[]    m_brakesTemperature;       // Brakes temperature (celsius), max=4
        public ushort[]    m_tyresSurfaceTemperature; // Tyres surface temperature (celsius), max=4
        public ushort[]    m_tyresInnerTemperature;   // Tyres inner temperature (celsius), max=4
        public ushort m_engineTemperature;          // Engine temperature (celsius)
        public float[]     m_tyresPressure;           // Tyres pressure (PSI), max=4
    };

    [Serializable]
    public struct PacketCarTelemetryData
    {
        public PacketHeader        m_header;                // Header

        public CarTelemetryData[]    m_carTelemetryData;    //max = 20
        /// <summary>
        /// ref code : https://us.v-cdn.net/5021484/uploads/editor/9b/66rgj8cv225n.png
        /// </summary>
        public UInt32              m_buttonStatus;         // Bit flags specifying which buttons are being pressed currently - see appendices
    };


    /// <summary>
    /// CAR STATUS PACKET
    /// This packet details car statuses for all the cars in the race. It includes values such as the damage readings on the car.
    /// Frequency: 2 per second
    /// Size: 1061 bytes
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct CarStatusData
    {
        public byte m_tractionControl;          // 0 (off) - 2 (high)
        public byte m_antiLockBrakes;           // 0 (off) - 1 (on)
        public byte m_fuelMix;                  // Fuel mix - 0 = lean, 1 = standard, 2 = rich, 3 = max
        public byte m_frontBrakeBias;           // Front brake bias (percentage)
        public byte m_pitLimiterStatus;         // Pit limiter status - 0 = off, 1 = on
        public float m_fuelInTank;               // Current fuel mass
        public float m_fuelCapacity;             // Fuel capacity
        public UInt16 m_maxRPM;                   // Cars max RPM, point of rev limiter
        public UInt16 m_idleRPM;                  // Cars idle RPM
        public byte m_maxGears;                 // Maximum number of gears
        public byte m_drsAllowed;               // 0 = not allowed, 1 = allowed, -1 = unknown
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, MarshalType = "byte[]", MarshalTypeRef = typeof(byte[]))]
        public byte[] m_tyresWear;             // Tyre wear percentage, max=4
        public byte m_tyreCompound;             // Modern - 0 = hyper soft, 1 = ultra soft 2 = super soft, 3 = soft, 4 = medium, 5 = hard 6 = super hard, 7 = inter, 8 = wet Classic - 0-6 = dry, 7-8 = wet
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, MarshalType = "byte[]", MarshalTypeRef = typeof(byte[]))]
        public byte[] m_tyresDamage;           // Tyre damage (percentage), max=4
        public byte m_frontLeftWingDamage;      // Front left wing damage (percentage)
        public byte m_frontRightWingDamage;     // Front right wing damage (percentage)
        public byte m_rearWingDamage;           // Rear wing damage (percentage)
        public byte m_engineDamage;             // Engine damage (percentage)
        public byte m_gearBoxDamage;            // Gear box damage (percentage)
        public byte m_exhaustDamage;            // Exhaust damage (percentage)
        public byte m_vehicleFiaFlags;          // -1 = invalid/unknown, 0 = none, 1 = green 2 = blue, 3 = yellow, 4 = red
        public float m_ersStoreEnergy;           // ERS energy store in Joules
        public byte m_ersDeployMode;            // ERS deployment mode, 0 = none, 1 = low, 2 = medium 3 = high, 4 = overtake, 5 = hotlap
        public float m_ersHarvestedThisLapMGUK;  // ERS energy harvested this lap by MGU-K
        public float m_ersHarvestedThisLapMGUH;  // ERS energy harvested this lap by MGU-H
        public float m_ersDeployedThisLap;       // ERS energy deployed this lap
    };

    [Serializable]
    public struct PacketCarStatusData
    {
        public PacketHeader        m_header;            // Header

        public CarStatusData[]       m_carStatusData;  //max=20
    };
}