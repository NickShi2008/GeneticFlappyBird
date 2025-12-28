using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace GeneticFlappyBird
{
    public class Pipe
    {
        private SpriteBatch sb;

        public Vector2 Location;
        public Point Size { get; private set; }

        public float Speed;

        public RectangleF hitbox;

        public Pipe(SpriteBatch sb, Vector2 location, Point size, float speed)
        {
            this.sb = sb;
            this.Location = location;
            this.Size = size;
            Speed = speed;
            hitbox = new RectangleF(Location, size);
        }

        public void Draw()
        {
            sb.DrawRectangle(hitbox, Color.Black);
        }

        public void Move()
        {
            Location.X += Speed;
            hitbox.X = Location.X;
            hitbox.Y = Location.Y;
        }
    }
}
