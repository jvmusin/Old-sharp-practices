using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PudgeClient
{
    public static class GeometryUtils
    {
        public static double RadiansToDegrees(double radians)
        {
            return radians * 180 / Math.PI;
        }

        public static double DegreesToRadians(double degrees)
        {
            return degrees * Math.PI / 180;
        }


        public static double From180To360Degrees(double angle)
        {
            if (angle < 0)
                angle += 360;
            return angle;
        }

        public static double From360To180Degrees(double angle)
        {
            if (angle > 180)
                angle -= 360;
            return angle;
        }
    }
}
