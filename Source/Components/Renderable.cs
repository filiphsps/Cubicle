using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cubicle.Components {
    public class Renderable {
        public Matrix World;
        public Matrix View;
        public Matrix Projection;

        public Effect Effect;
    }
}
