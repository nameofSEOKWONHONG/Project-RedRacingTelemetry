using Project_RedRacingTelemetry.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project_RedRacingTelemetry.Objects
{
    /// <summary>
    ///  모션 패킷은 구동되는 모든 자동차에 대한 물리 데이터를 제공합니다. 
    /// 모션 플랫폼 설정을 유도 할 수 있다는 목표로 운전중인 자동차에 대한 추가 데이터가 있습니다.
    /// NB 아래의 정규화 된 벡터의 경우 float 값으로 변환하면 32767.0f로 나눕니다. 
    /// 16 비트 부호있는 값은 방향 값이 항상 -1.0f와 1.0f 사이에 있다는 가정하에 데이터를 묶는 데 사용됩니다.
    /// </summary>
    [Serializable]
    public class CarMotionData : IF1PacketObject
    {
        public float m_worldPositionX;           // World space X position
        public float m_worldPositionY;           // World space Y position
        public float m_worldPositionZ;           // World space Z position
        public float m_worldVelocityX;           // Velocity in world space X
        public float m_worldVelocityY;           // Velocity in world space Y
        public float m_worldVelocityZ;           // Velocity in world space Z
        public Int16 m_worldForwardDirX;         // World space forward X direction (normalised)
        public Int16 m_worldForwardDirY;         // World space forward Y direction (normalised)
        public Int16 m_worldForwardDirZ;         // World space forward Z direction (normalised)
        public Int16 m_worldRightDirX;           // World space right X direction (normalised)
        public Int16 m_worldRightDirY;           // World space right Y direction (normalised)
        public Int16 m_worldRightDirZ;           // World space right Z direction (normalised)
        public float m_gForceLateral;            // Lateral G-Force component
        public float m_gForceLongitudinal;       // Longitudinal G-Force component
        public float m_gForceVertical;           // Vertical G-Force component
        public float m_yaw;                      // Yaw angle in radians
        public float m_pitch;                    // Pitch angle in radians
        public float m_roll;                     // Roll angle in radians

        public event EventHandler OnPacketReceived;

        public void Parse(byte[] buffer)
        {
            throw new NotImplementedException();
        }
    }
}
