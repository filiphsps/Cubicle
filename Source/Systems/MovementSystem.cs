using Apos.Input;
using Cubicle.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using System;
using System.Numerics;
using Vector3 = System.Numerics.Vector3;
using Vector2 = System.Numerics.Vector2;

namespace Cubicle.Systems {
    public class MovementSystem : EntityUpdateSystem {
        Game _game;

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
            foreach (var entityId in ActiveEntities) {
                var input = _inputMapper.Get(entityId);
                var transform = _transformMapper.Get(entityId);

                var mouseDelta = input.MouseDelta * input.Sensitivity;
                transform.Forward = Vector3.Transform(transform.Forward,
                    Matrix4x4.CreateFromAxisAngle(Vector3.UnitY, (-MathHelper.PiOver4 / 150) * mouseDelta.X));
                transform.Forward = Vector3.Transform(transform.Forward,
                    Matrix4x4.CreateFromAxisAngle(Vector3.Cross(Vector3.UnitY, transform.Forward), (MathHelper.PiOver4 / 100) * mouseDelta.Y));
                transform.Forward = Vector3.Normalize(transform.Forward);

                float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
                float speed = 500f;

                var velocity = Vector3.Zero;
                var positionDelta = Vector3.Zero;

                if (input.Forward == Components.KeyState.Pressed)
                    velocity.X += delta * speed;
                if (input.Backward == Components.KeyState.Pressed)
                    velocity.X -= delta * speed;

                if (input.Left == Components.KeyState.Pressed)
                    velocity.Z += delta * speed;
                if (input.Right == Components.KeyState.Pressed)
                    velocity.Z -= delta * speed;

                if (input.Up == Components.KeyState.Pressed) {
                    velocity.Y += delta * speed;
                    positionDelta.Y += delta * velocity.Y;
                }
                if (input.Down == Components.KeyState.Pressed) {
                    velocity.Y += delta * speed;
                    positionDelta.Y -= delta * velocity.Y;
                }

                var hor = Vector3.Normalize(new Vector3(transform.Forward.X, 0, transform.Forward.Z));

                positionDelta += delta * velocity.X * hor;
                positionDelta += delta * velocity.Z * Vector3.Cross(Vector3.UnitY, hor);

                transform.Position += positionDelta;
            }
        }
    }
}
