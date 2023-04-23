

using Microsoft.Xna.Framework;
using System;
using Vector3 = System.Numerics.Vector3;

namespace Cubicle.Components {
    public class Camera {
        public Matrix _world;

        public Vector3 Position = new Vector3(0, 0, 0);
        public Vector3 Forward = new Vector3(0, 0, -1);
        public Vector3 Up = Vector3.UnitY;

        public Matrix View = Matrix.Identity;
        public Matrix Projection = Matrix.Identity;

        // FIXME: move this to util
        // FIXME: enum
        // X = east
        // -X = west
        // Z = south
        // -Z = north
        public string Direction {
            get {
                var dir = Vector3.Normalize(Forward);
                float east = Vector3.Dot(new Vector3(1, 0, 0), dir);
                float north = Vector3.Dot(new Vector3(0, 0, -1), dir);

                if (Math.Abs(east) > Math.Abs(north)) {
                    if (east > 0)
                        return "East (+X)";
                    return $"West (-X)";
                } else {
                    if (north > 0)
                        return "North (-Z)";
                    return $"South (+Z)";
                };
            }
        }
    }
}
