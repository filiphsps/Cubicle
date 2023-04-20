using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using Microsoft.Xna.Framework.Graphics;

namespace Cubicle.Source.Systems {
    public class RenderSystem : EntityDrawSystem {
        GraphicsDevice _graphics;

        public RenderSystem(GraphicsDevice graphics)
            : base(Aspect.All()) {
            _graphics = graphics;
        }

        public override void Initialize(IComponentMapperService mapperService) { }

        public override void Draw(GameTime gameTime) { }
    }
}
