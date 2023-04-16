﻿using Cubicle.NET.Util;
using Silk.NET.OpenGL;
using Silk.NET.Maths;

namespace Cubicle.NET.Engine.Rendering
{
    public class Renderer : Manager
    {
        private GL gl;

        public static Player player;
        private Chunk chunk;
        public static Camera camera;

        public unsafe Renderer(GL gl)
        {
            this.gl = gl;


            camera = new Camera(gl);

            float[] player_vertices = { };
            uint[] player_indices = { };

            //player = new Player(gl, "scene.vert", "scene.frag", player_vertices, player_indices);
            chunk = new Chunk(gl, new Vector3D<int>(0, 0, 0 ));
            
        }

        public override void Update(double delta)
        {
            //player.Update(delta);
        }

        public unsafe void Render(double delta)
        {
            gl.Enable(EnableCap.DepthTest);
            gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            gl.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);


            chunk.Draw(delta, camera);
        }
    }
}
