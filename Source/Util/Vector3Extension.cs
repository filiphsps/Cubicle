using System;
using System.Numerics;

namespace Cubicle.Util {
    public enum CardinalDirection {
        /// <summary>
        /// +X
        /// </summary>
        East,

        /// <summary>
        /// -X
        /// </summary>
        West,

        /// <summary>
        /// -Z
        /// </summary>
        North,

        /// <summary>
        /// +Z
        /// </summary>
        South
    }

    public static partial class StringHelpers {
        public static String CardinalToString(this CardinalDirection dir) {
            switch (dir) {
                case CardinalDirection.East:
                    return "East (+X)";
                case CardinalDirection.West:
                    return "West (-X)";
                case CardinalDirection.North:
                    return "North (+Z)";
                case CardinalDirection.South:
                default:
                    return "South (-Z)";
            }
        }
    }

    public static class Vector3Extension {
        public static CardinalDirection Cardinal(this Vector3 direction) {
            var dir = Vector3.Normalize(direction);
            float east = Vector3.Dot(new Vector3(1, 0, 0), dir);
            float north = Vector3.Dot(new Vector3(0, 0, -1), dir);

            if (Math.Abs(east) > Math.Abs(north)) {
                if (east > 0)
                    return CardinalDirection.East;
                return CardinalDirection.West;
            } else {
                if (north > 0)
                    return CardinalDirection.North;
                return CardinalDirection.South;
            };
        }
    }
}
