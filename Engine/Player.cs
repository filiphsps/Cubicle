using Cubicle.NET.Util;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Steamworks.Ugc;
using System.Numerics;

namespace Cubicle.NET.Engine
{
    public class Player : Entity
    {
        public float CameraRotationX;
        public float CameraRotationY;

        public Player()
        {
        }

        public override void Update(double delta)
        {
            Velocity *= (1.0f - 0.01f);
            Position += Velocity * (float)delta;
        }

        public void Move(float f, float l, double delta)
        {
            float speed = 2.0f;
            // in order to move forward in the direction of the camera, some matrix magic is needed
            // im not entirely sure how this works but it does lol
            Matrix4 camToWorld = LocalToWorld() * Matrix4.RotateY(CameraRotationY);

            var temp = new Vector3D<float>(-l, 0, -f);
            var dir = new Vector3D<float>(
                camToWorld.M[0] * temp.X + camToWorld.M[1] * temp.Y + camToWorld.M[2] * temp.Z,
                camToWorld.M[4] * temp.X + camToWorld.M[5] * temp.Y + camToWorld.M[6] * temp.Z,
                camToWorld.M[8] * temp.X + camToWorld.M[9] * temp.Y + camToWorld.M[10] * temp.Z);
            Velocity += dir * (float)(speed * delta);
        }

        public Matrix4 WorldToCamera()
        {
            return
                Matrix4.RotateX(-CameraRotationX) *
                Matrix4.RotateY(-CameraRotationY) *
                WorldToLocal();
        }
    }
}
