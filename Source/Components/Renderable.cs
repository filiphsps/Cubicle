using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cubicle.Components {
    public class Renderable {
        public Matrix World;
        public Matrix View;
        public Matrix Projection;

        public Effect Effect;
    }
}
