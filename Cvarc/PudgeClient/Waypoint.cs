using System.Collections.Generic;
using CVARC.V2;

namespace PudgeClient
{
    public class Waypoint : Point
    {
        private readonly List<Waypoint> connectedWaypoints;
        public IEnumerable<Waypoint> ConnectedWaypoints => connectedWaypoints;
        public bool Available { get; private set; }
        
        public Waypoint(double x, double y) : base(x, y)
        {
            Available = true;
            connectedWaypoints = new List<Waypoint>();
        }

        public Waypoint(LocatorItem position) : base(position)
        {
        }

        public void MarkAsUnavailable()
        {
            Available = false;
        }

        public void Connect(Waypoint other)
        {
            connectedWaypoints.Add(other);
            other.connectedWaypoints.Add(this);
        }
    }
}
