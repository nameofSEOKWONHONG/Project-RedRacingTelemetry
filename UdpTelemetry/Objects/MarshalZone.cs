﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Project_RedRacingTelemetry.Objects
{
    public class MarshalZone
    {
        public float m_zoneStart;   // Fraction (0..1) of way through the lap the marshal zone starts
        public sbyte m_zoneFlag;    // -1 = invalid/unknown, 0 = none, 1 = green, 2 = blue, 3 = yellow, 4 = red
    }
}
