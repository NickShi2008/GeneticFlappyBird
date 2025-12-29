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
    public class Sprite
    {
        private Texture2D texture;

        public Texture2D Texture
        {
            get
            {
                return texture;
            }
        }
        public Rectangle Hitbox
        {
            get
            {
                return new Rectangle(position.ToPoint() - new Point(30, 30), new Point(texture.Bounds.Size.X, texture.Bounds.Size.Y) * scale.ToPoint());
            }
        }
        private Color color;
        public Color RGB
        {
            get
            {
                return color;
            }
            set
            {
                color = value;
            }
        }
        private Vector2 position;
        public Vector2 Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
            }
        }

        private Vector2 origin;

        public Vector2 Origin
        {
            get
            {
                return origin;
            }
        }

        private Vector2 scale;

        public Vector2 Scale
        {
            get
            {
                return scale;
            }
            set
            {
                scale = value;
            }
        }

        public Sprite(Texture2D texture, Vector2 position, Color color, Vector2 origin, Vector2 scale)
        {
            this.texture = texture;
            this.color = color;
            this.position = position;
            this.origin = origin;
            this.scale = scale;
        }
        public Sprite(Texture2D texture, Vector2 position)
            : this(texture, position, Color.White, new Vector2(texture.Width / 2, texture.Height / 2), new Vector2(1, 1)) { }

        public Sprite(Texture2D texture, Vector2 position, Vector2 size)
            : this(texture, position, Color.White, new Vector2(texture.Width / 2, texture.Height / 2), size) { }

        public void Draw(SpriteBatch spriteBatch,int layer = 0)
        {
            spriteBatch.Draw(texture, position, null, color, 0, origin, scale, SpriteEffects.None, layer);
        }

        public void Draw(SpriteBatch sb, Vector2 drawPosition, float rotation)
        {
            Vector2 rotationOrigin = new Vector2(
                texture.Width / 2f,
                texture.Height / 2f
            );
            Vector2 adjustedPosition = drawPosition + rotationOrigin * scale;

            sb.Draw(texture,adjustedPosition,null,color,rotation,rotationOrigin, scale,SpriteEffects.None,0f);
        }

        public void DrawColor(SpriteBatch spriteBatch, Color color)
        {
            spriteBatch.Draw(texture, position, null, color, 0, origin, scale, SpriteEffects.None, default);
        }


        public void DrawFlip(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, null, color, 0, origin, scale, SpriteEffects.FlipHorizontally, default);
        }

        public void DrawHitbox(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Hitbox, Color.Red);
        }


    }
}

