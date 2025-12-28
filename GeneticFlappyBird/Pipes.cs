using MonoGame.Extended.Graphics;
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
    public class Pipes
    {
        Pipe bottomPipe;
        Pipe topPipe;
        public float Speed;
        SpriteBatch sb;
        private Point gapSize = new Point(100, 225);
        public Vector2 Location;
        public int PipeSize = 100;
        public Pipes(float speed, SpriteBatch sb, Vector2 location, float height)
        {
            Speed = speed;
            this.sb = sb;
            Location = location;
            bottomPipe = new Pipe(sb, new Vector2(location.X, location.Y + gapSize.Y),
                new Point(PipeSize, (int) (height - location.Y)), speed);
            topPipe = new Pipe(sb, new Vector2(location.X, 0), new Point(PipeSize, (int)(location.Y)), speed);
        }

        public void Draw()
        {
           // sb.DrawRectangle(new RectangleF(Location, gapSize), Color.Red);
            
            bottomPipe.Draw();
            topPipe.Draw();
        }

        public void Move()
        {
            Location.X += Speed;
            bottomPipe.Move();
            topPipe.Move();
        }

        public void Stop()
        {
            topPipe.Speed = 0;
            bottomPipe.Speed = 0;
            Speed = 0;
        }

        public bool Intersects(RectangleF hitbox)
        {
            bool bottomCheck = bottomPipe.hitbox.Intersects(hitbox);
            bool topCheck = topPipe.hitbox.Intersects(hitbox);
            return bottomCheck || topCheck;
        }
    }
}
