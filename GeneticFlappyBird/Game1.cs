using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace GeneticFlappyBird
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch sb;
        private Bird bird;
        private Rectangle birdHB;
        private Vector2 birdStart;
        private Point Size;

        KeyboardState lastKeyState;

        private float screenMoveSpeed;

        private List<Pipes> obstacles;

        private int ScreenSize = 800;

        Random random = new Random();

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            graphics.PreferredBackBufferHeight = ScreenSize;
            graphics.PreferredBackBufferWidth = (int) (ScreenSize * 1.5);
            graphics.ApplyChanges();
           

            base.Initialize();
        }

        protected override void LoadContent()
        {
            birdStart = new Vector2(graphics.PreferredBackBufferWidth/10, graphics.PreferredBackBufferHeight/4);
            Size = new Point(50,50);

            sb = new SpriteBatch(GraphicsDevice);
            bird = new Bird(sb, birdStart, Size);


            screenMoveSpeed = -3f;
            float randY = random.Next(ScreenSize - ScreenSize / 2) + ScreenSize / 4;
            obstacles = new List<Pipes>();
            obstacles.Add(new Pipes(screenMoveSpeed, sb,
                new Vector2(graphics.PreferredBackBufferWidth, randY), graphics.PreferredBackBufferHeight));
            
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here



            if(bird.gameEnd || bird.hitbox.Y + bird.hitbox.Height > graphics.PreferredBackBufferHeight)
            {
                StopGame();   
            }
            else
            {
                bird.Fall();
                for (int i = 0; i < obstacles.Count; i++)
                {
                    obstacles[i].Move();
                    if (obstacles[i].Location.X + obstacles[i].PipeSize < 0)
                    {
                        obstacles.Remove(obstacles[i]);
                    }
                    else if (obstacles.Count < 2 && obstacles[i].Location.X + obstacles[i].PipeSize < graphics.PreferredBackBufferWidth / 2)
                    {
                        float randY = random.Next(ScreenSize - ScreenSize / 3) + ScreenSize / 5;
                        obstacles.Add(new Pipes(screenMoveSpeed, sb,
                        new Vector2(graphics.PreferredBackBufferWidth, randY), graphics.PreferredBackBufferHeight));
                    }
                    if (obstacles[i].Intersects(bird.hitbox))
                    {
                        StopGame();
                    }
                }
            }
            


            KeyboardState keyState = Keyboard.GetState();
            if (keyState.IsKeyUp(Keys.Up) && lastKeyState.IsKeyDown(Keys.Up))
            {
                bird.Jump();
            }

          

            lastKeyState = keyState;
            base.Update(gameTime);
        }

        public void StopGame()
        {
            bird.StopBird();
            for(int i = 0; i < obstacles.Count; i++)
            {
                obstacles[i].Stop(); 
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            sb.Begin();

            bird.Draw();
            for (int i = 0; i < obstacles.Count; i++)
            {
                obstacles[i].Draw();
            }

            // TODO: Add your drawing code here
            sb.End();
            base.Draw(gameTime);
        }
    }
}
