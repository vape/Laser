using System;

namespace Laser
{
    public static class GlobalTime
    {
        public static DateTime Current
        {
            get
            {
                return DateTime.UtcNow;
            }
        }
    }
}
