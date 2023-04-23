using Cubicle.Components;
using Cubicle.Level;
using Cubicle.Singletons;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using System;
using System.Numerics;
using Vector3 = System.Numerics.Vector3;

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

                float delta = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

                float speed = 0.01f;
                float acceleration = 0.015f;
                float friction = 0.5f;

                var velocity = transform.Velocity;
                var position_delta = Vector3.Zero;

                if (Math.Abs(velocity.X) > 5e-3f) {
                    velocity.X -= Math.Sign(velocity.X) * friction * acceleration * delta;
                } else {
                    velocity.X = 0;
                }

                if (Math.Abs(velocity.Y) > 5e-3f) {
                    velocity.Y -= Math.Sign(velocity.Y) * friction * acceleration * delta;
                } else {
                    velocity.Y = 0;
                    friction = 0.25f;
                }

                if (Math.Abs(velocity.Z) > 5e-3f) {
                    velocity.Z -= Math.Sign(velocity.Z) * friction * acceleration * delta;
                } else {
                    velocity.Z = 0;
                }

                if (input.Forward == Components.KeyState.Pressed) {
                    if (Math.Abs(velocity.X) < speed) {
                        velocity.X += acceleration * delta;
                    } else {
                        velocity.X = speed;
                    }
                }

                if (input.Backward == Components.KeyState.Pressed) {
                    if (Math.Abs(velocity.X) < speed) {
                        velocity.X -= acceleration * delta;
                    } else {
                        velocity.X = -speed;
                    }
                }

                if (input.Left == Components.KeyState.Pressed) {
                    if (Math.Abs(velocity.Z) < speed) {
                        velocity.Z += acceleration * delta;
                    } else {
                        velocity.Z = speed;
                    }
                }

                if (input.Right == Components.KeyState.Pressed) {
                    if (Math.Abs(velocity.Z) < speed) {
                        velocity.Z -= acceleration * delta;
                    } else {
                        velocity.Z = -speed;
                    }
                }

                if (input.Up == Components.KeyState.Pressed) {
                    if (velocity.Y < 2 * speed) {
                        velocity.Y += 2 * acceleration * delta;
                    }
                    position_delta.Y = speed;
                }

                if (input.Down == Components.KeyState.Pressed) {
                    if (velocity.Y < 2 * speed) {
                        velocity.Y += 2 * acceleration * delta;
                    }

                    position_delta.Y = -speed;
                }

                // TODO: Move this to PhysicsSystem

                var horizontal = Vector3.Normalize(new Vector3(transform.Forward.X, 0, transform.Forward.Z));

                position_delta += velocity.X * horizontal;
                position_delta += velocity.Z * Vector3.Cross(Vector3.UnitY, horizontal);

                transform.Position += position_delta * delta;
                transform.Velocity = velocity;

                DebugManager.Text($"XYZ: {transform.Position.X.ToString("0.0")} / {transform.Position.Y.ToString("0.0")} / {transform.Position.Z.ToString("0.0")}");
                var block = Block.ToRelative(transform.Position);
                var chunk = Chunk.ToRelative(transform.Position);
                DebugManager.Text($"Chunk: {block.X} {block.Y} {block.Z} in {chunk.X} {chunk.Y} {chunk.Z}");
                DebugManager.Text($"Vel: {velocity.X.ToString("0.0")} {velocity.Y.ToString("0.0")} {velocity.Z.ToString("0.0")}");
                DebugManager.Div();
            }
        }
    }
}
