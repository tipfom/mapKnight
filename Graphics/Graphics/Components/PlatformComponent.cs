using mapKnight.Core;
using System;
using System.Collections.Generic;

namespace mapKnight.Extended.Components {
    public class PlatformComponent : Component {
        const Interpolation interpolationMode = Interpolation.Linear;

        private float speed; // tiles per second
        private List<Vector2> waypoints;

        private int currentWaypointIndex;
        private Vector2 currentWaypoint { get { return waypoints[currentWaypointIndex]; } }
        private int nextWaypointIndex;
        private Vector2 nextWaypoint { get { return waypoints[nextWaypointIndex]; } }
        private int nextWaypointTime;
        private int currentWaypointDuration;

        public PlatformComponent (Entity owner, List<Vector2> waypoints, float speed) : base (owner) {
            this.State = new Vector2 (0, 0);
            this.waypoints = waypoints;
            this.speed = (float)Math.Pow (speed, 2);

            // translate waypoints (since they are 0,0 based and need to start at the entities startposition)
            for (int i = 0; i < waypoints.Count; i++)
                waypoints[i] += owner.Transform.Center;
        }

        public override void Prepare () {
            currentWaypointIndex = 0;
            nextWaypointIndex = 1;
            nextWaypointTime = Environment.TickCount;
            PrepareNextStep ();
        }

        public override void Update (float dt) {
            if (Environment.TickCount >= nextWaypointTime) {
                // next waypoint
                currentWaypointIndex = nextWaypointIndex;
                nextWaypointIndex++;
                if (nextWaypointIndex == waypoints.Count) {
                    nextWaypointIndex = 0;
                }
                PrepareNextStep ();
            }

            float progressPercent = 1f - (nextWaypointTime - Environment.TickCount) / (float)currentWaypointDuration;
            Vector2 nextPosition = Mathf.Interpolate (currentWaypoint, nextWaypoint, progressPercent);
            this.Owner.Transform.Translate (nextPosition);
        }

        private int GetCurrentWaypointDuration () {
            return (int)((currentWaypoint - nextWaypoint).MagnitudeSqr () / speed * 1000);
        }

        private void PrepareNextStep () {
            currentWaypointDuration = GetCurrentWaypointDuration ();
            nextWaypointTime += currentWaypointDuration;
            this.State = (nextWaypoint - currentWaypoint) / (currentWaypointDuration / 1000f); // current velocity
        }
    }
}