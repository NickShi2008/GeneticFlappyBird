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

        private Sprite pipe;
        public Pipe(SpriteBatch sb, Vector2 location, Point size, float speed, Sprite pipe)
        {
            this.sb = sb;
            this.Location = location;
            this.Size = size;
            Speed = speed;
            hitbox = new RectangleF(Location, size);
            this.pipe = pipe;
        }

        public void Draw(float rotation = 0)
        {
           // sb.DrawRectangle(hitbox, Color.Black);
            pipe.Draw(sb, Location, rotation);
        }

        public void Move()
        {
            Location.X += Speed;
            hitbox.X = Location.X;
            hitbox.Y = Location.Y;
        }
    }
}
