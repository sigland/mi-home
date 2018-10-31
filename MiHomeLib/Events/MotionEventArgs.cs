using System;

namespace MiHomeLib.Events
{
    public class MotionEventArgs : EventArgs
    {
        public MotionEventArgs(uint lux)
        {
            Lux = lux;
        }

        public uint Lux { get; set; }
    }
}
