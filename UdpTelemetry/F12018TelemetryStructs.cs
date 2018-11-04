using System;
using System.Runtime.InteropServices;

/// <summary>
/// reference site : https://forums.codemasters.com/discussion/136948/f1-2018-udp-specification
/// </summary>
namespace F12018UdpTelemetry
{
    

   

    [Serializable]
    public struct PacketMotionData
    {
        public PacketHeader    m_header;               // Header
        public CarMotionData[]   m_carMotionData;    // Data for all cars on track (array length:20)
        
        // Extra player car ONLY data
        public float[]         m_suspensionPosition;       // Note: All wheel arrays have the following order: (array length:4)
        public float[]         m_suspensionVelocity;       // RL, RR, FL, FR (array length:4)
        public float[]         m_suspensionAcceleration;   // RL, RR, FL, FR (array length:4)
        public float[]         m_wheelSpeed;               // Speed of each wheel (array length:4)
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