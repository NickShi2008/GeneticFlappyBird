using MonoGame.Extended.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using SharpDX.Direct3D9;
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

        public Rectangle enterPoint;
        public Pipes(float speed, SpriteBatch sb, Vector2 location, float height, Texture2D pipe)
        {
            Speed = speed;
            this.sb = sb;
            Location = location;
            Vector2 origin = new Vector2(pipe.Width / 2f, 0f);
            float bottomHeight = height - location.Y - gapSize.Y;

            Vector2 bottomScale = new Vector2(
                PipeSize / (float)pipe.Width,
                bottomHeight / (float)pipe.Height
            );

            Vector2 bottomOrigin = new Vector2(0, 0f);

            bottomPipe = new Pipe(
                sb,
                new Vector2(location.X, location.Y + gapSize.Y),
                new Point(PipeSize, (int)bottomHeight),
                speed,
                new Sprite(pipe, location, Color.White, bottomOrigin, bottomScale)
            );

            float topHeight = location.Y;

            Vector2 topScale = new Vector2(
                PipeSize / (float)pipe.Width,
                topHeight / (float)pipe.Height
            );

            Vector2 topOrigin = new Vector2(pipe.Width, pipe.Height);

            topPipe = new Pipe(
                sb,
                new Vector2(location.X, 0),
                new Point(PipeSize, (int)topHeight),
                speed,
                new Sprite(pipe, location, Color.White, topOrigin, topScale)
            );


            enterPoint = new Rectangle(new Point((int)Location.X,(int) Location.Y), gapSize);
        }

        public void Draw()
        {
           // sb.DrawRectangle(new RectangleF(Location, gapSize), Color.Red);
            
            bottomPipe.Draw();
            topPipe.Draw(float.Pi);
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
