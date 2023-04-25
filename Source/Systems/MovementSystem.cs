using Cubicle.Components;
using Cubicle.Gearset;
using Cubicle.Level;
using Cubicle.Singletons;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using System.Numerics;
using Vector3 = System.Numerics.Vector3;

namespace Cubicle.Systems {
    public class MovementSystem : EntityUpdateSystem {
        private ComponentMapper<Input> _inputMapper;
        private ComponentMapper<Transform> _transformMapper;

        public MovementSystem()
            : base(Aspect.All(typeof(Input), typeof(Transform))) {
        }

        public override void Initialize(IComponentMapperService mapperService) {
            _inputMapper = mapperService.GetMapper<Input>();
            _transformMapper = mapperService.GetMapper<Transform>();
        }

        public override void Update(GameTime gameTime) {
            GS.BeginMark("Movement", Color.YellowGreen);
            foreach (var entityId in ActiveEntities) {
                var input = _inputMapper.Get(entityId);
                var transform = _transformMapper.Get(entityId);

                var mouseDelta = input.MouseDelta * input.Sensitivity;
                transform.Forward = Vector3.Transform(transform.Forward,
                    Matrix4x4.CreateFromAxisAngle(Vector3.UnitY, (-MathHelper.PiOver4 / 150) * mouseDelta.X));
                transform.Forward = Vector3.Transform(transform.Forward,
                    Matrix4x4.CreateFromAxisAngle(Vector3.Cross(Vector3.UnitY, transform.Forward), (MathHelper.PiOver4 / 100) * mouseDelta.Y));
                transform.Forward = Vector3.Normalize(transform.Forward);

                float delta = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                var position = Vector3.Zero;
                var speed = 0.01f;

                if (input.Forward == Components.KeyState.Pressed) {
                    position += new Vector3(1, 0, 0);
                }

                if (input.Backward == Components.KeyState.Pressed) {
                    position += new Vector3(-1, 0, 0);
                }

                if (input.Left == Components.KeyState.Pressed) {
                    position += new Vector3(0, 0, 1);
                }

                if (input.Right == Components.KeyState.Pressed) {
                    position += new Vector3(0, 0, -1);
                }

                if (input.Up == Components.KeyState.Pressed) {
                    transform.Position += delta * speed * new Vector3(0, 1, 0);
                }

                if (input.Down == Components.KeyState.Pressed) {
                    transform.Position += delta * speed * new Vector3(0, -1, 0);
                }

                // TODO: physics-based movement
                var horizontal = Vector3.Normalize(transform.Forward);
                transform.Position += delta * speed * position.X * transform.Forward;
                transform.Position += delta * speed * position.Z * Vector3.Cross(Vector3.UnitY, horizontal);

                DebugManager.Text($"XYZ: {transform.Position.X.ToString("0.0")} / {transform.Position.Y.ToString("0.0")} / {transform.Position.Z.ToString("0.0")}");
                var block = Block.ToRelative(transform.Position);
                var chunk = Chunk.ToRelative(transform.Position);
                DebugManager.Text($"Chunk: {block.X} {block.Y} {block.Z} in {chunk.X} {chunk.Y} {chunk.Z}");
                DebugManager.Div();
            }
            GS.EndMark("Movement");
        }
    }
}
