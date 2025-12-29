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
        const float JUMPHEIGHT = -4.5f;

        const float GRAVITY = 0.35f;
        const float MAXFALL = 10f;

        private float Speed;

        private SpriteBatch sb;

        public Vector2 Location;
        public Point Size { get; private set; }

        public RectangleF hitbox;

        public bool gameEnd;

        private Sprite bird;
        public NeuralNetwork net;
        public double Fitness;

        private const float MAXROTATIONUP = -MathF.PI/4;
        private const float MAXROTATIONDOWN = MathF.PI/2;
        private const float ROTATIONSPEED = 0.1f;
        private float rotation;
        private bool isDead;
        private bool hasHitGround;
        private float pipeSpeed;
        public Bird(SpriteBatch spriteBatch, Vector2 location, Point size, NeuralNetwork nn, Sprite bird)
        {
            sb = spriteBatch;
            Location = location;
            Size = size;
            hitbox = new RectangleF(Location, Size);
            this.bird = bird;
            this.net = nn;
            rotation = 0;

            isDead = false;
            hasHitGround = false;
            pipeSpeed = 0f;
        }

        public void Draw()
        {
            //sb.DrawRectangle(new RectangleF(Location, Size), Color.Black);
            
            bird.Draw(sb, Location, rotation);
        }

        private void CalcSpeed()
        {
            Speed = Speed + GRAVITY;
            Speed = MathHelper.Clamp(Speed, -10, 10);
            if (Speed > 0) 
            {
                rotation = MathHelper.Lerp(rotation, MAXROTATIONDOWN, ROTATIONSPEED);
            }
            else if (Speed < 0) 
            {
                rotation = MathHelper.Lerp(rotation, MAXROTATIONUP, ROTATIONSPEED);
            }
        }

        public void Fall()
        {
            if (!hasHitGround)
            {
                CalcSpeed();
                Location.Y += Speed;
            }
            hitbox.X = Location.X;
            hitbox.Y = Location.Y;

            if (isDead)
            {
                Location.X += pipeSpeed;
            }
        }

        public void Reset(Vector2 birdStart)
        {
            Location = birdStart;
            gameEnd = false;
            isDead = false;
            hasHitGround = false;
            Speed = 0;
            rotation = 1;
            pipeSpeed = 0f;
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
            
        }

        public void KillBird(float speed)
        {
            if (!isDead)
            {
                isDead = true;
                pipeSpeed = speed; 
                gameEnd = true;
            }
        }

        public void HitGround()
        {
            hasHitGround = true;
            Speed = 0;
            gameEnd = true;
        }

    }
}
