using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticFlappyBird
{
    public class Bird
    {
        const float JUMPHEIGHT = -8;

        const float GRAVITY = 0.2f;

        private float Speed;

        private SpriteBatch sb;

        public Vector2 Location;
        public Point Size { get; private set; }

        public RectangleF hitbox;

        public bool gameEnd;

        public Bird(SpriteBatch spriteBatch, Vector2 location, Point size)
        {
            sb = spriteBatch;
            Location = location;
            Size = size;
            hitbox = new RectangleF(Location, Size);
        }

        public void Draw()
        {
            sb.DrawRectangle(new RectangleF(Location, Size), Color.Black);
        }

        private void CalcSpeed()
        {
            Speed = Speed + GRAVITY;
        }

        public void Fall()
        {
            CalcSpeed();
            Location.Y += Speed;
            hitbox.X = Location.X;
            hitbox.Y = Location.Y;
            
        }

        public void Jump()
        {
            Speed = JUMPHEIGHT;
            
        }

        public void StopBird()
        {
            Speed = 0;
            gameEnd = true;
        }

    }
}
