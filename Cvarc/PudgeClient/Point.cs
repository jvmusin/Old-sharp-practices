using System;
using AIRLab.Mathematics;
using CVARC.V2;

using static PudgeClient.GeometryUtils;

namespace PudgeClient
{
    public class Point : IComparable<Point>, IComparable, IEquatable<Point>
    {
        public double X { get; }
        public double Y { get; }
        public double RadiusVectorLength { get; }
        public double Angle { get; }

        public Point(double x, double y)
        {
            X = x;
            Y = y;
            RadiusVectorLength = Math.Sqrt(x*x + y*y);
            Angle = RadiansToDegrees(Math.Atan2(y, x));
        }

        public Point(LocatorItem position) : this(position.X, position.Y)
        {
        }

        public Point(Point2D position) : this(position.X, position.Y)
        {
        }

        public double DistanceTo(Point other)
        {
            return (this - other).RadiusVectorLength;
        }

        public static Point operator -(Point current, Point other)
        {
            return new Point(current.X - other.X, current.Y - other.Y);
        }

        public static Point operator -(LocatorItem current, Point other)
        {
            return new Point(current.X - other.X, current.Y - other.Y);
        }
        
        #region Comapre, equals and hashcode

        public int CompareTo(Point other)
        {
            var cmp = X.CompareTo(other.X);
            if (cmp == 0)
                cmp = Y.CompareTo(other.Y);
            return cmp;
        }


        public int CompareTo(object obj)
        {
            if (!(obj is Point)) return 100500;
            return CompareTo((Point) obj);
        }


        public bool Equals(Point other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            const double eps = 1e-6;
            return
                DoubleExtensions.Equals(X, other.X, eps) &&
                DoubleExtensions.Equals(Y, other.Y, eps);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Point) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X.GetHashCode()*100500) ^ Y.GetHashCode();
            }
        }

        #endregion

        public Point Normalize()
        {
            return new Point(X/RadiusVectorLength, Y/RadiusVectorLength);
        }

        public override string ToString()
        {
            return $"X: {X}, Y: {Y}";
        }
    }
}
