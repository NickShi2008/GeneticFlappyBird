using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace GeneticFlappyBird
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch sb;
       // private Bird[] birds;
        private Rectangle birdHB;
        private Vector2 birdStart;
        private Point Size;
        private Texture2D pipeTex;
        private Sprite background;

        private Sprite Ground;
        const int groundHeight = 100;
        //  KeyboardState lastKeyState;

        private float screenMoveSpeed;

        //private List<Pipes> obstacles;
        private Sprite[] numbers;
        private Texture2D[] numTex;
        private int ScreenSize = 800;
        Sprite birdSprite;
        Random random = new Random();
        NeuralNetwork[] nn;
        Trainer trainer;
        double mutationRate = 0.05;
        (NeuralNetwork, double)[] population;
        Texture2D groundText;

       // private (Bird, List<Pipes>)[] Games;
        float spacingX;
        float spacingY;

        ActivationFunction sigmoidFunc;
        ErrorFunction errorFunction;

        private Bird[] birds;
        private List<Pipes> pipes;
        private float groundMove = 0f;


        private float updateTime = 3;
        private const int numOfBirds = 100;

        private int generation;
        private int birdsLeft;

        private SpriteFont font;
        private const int GAPSIZE = 225;
        HashSet<Pipes> pipeCount = new HashSet<Pipes>();
        private int currentScore = 0;
        int highScore = 0;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            //graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            //graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
           graphics.PreferredBackBufferHeight = ScreenSize;
            graphics.PreferredBackBufferWidth = (int) (ScreenSize * 0.5);
            graphics.ApplyChanges();
            

            base.Initialize();
        }

        protected override void LoadContent()
        {
            nn = new NeuralNetwork[numOfBirds];
            screenMoveSpeed = -1f;
            birdStart = new Vector2(graphics.PreferredBackBufferWidth/10, graphics.PreferredBackBufferHeight/4);
            Size = new Point(50,50);
            birdsLeft = numOfBirds;
            generation = 1;
            sb = new SpriteBatch(GraphicsDevice);

            Texture2D birdTex = Content.Load<Texture2D>("yellowbird-midflap");
            pipeTex = Content.Load<Texture2D>("pipe-green");
            Texture2D backTex = Content.Load<Texture2D>("background-day");
            groundText = Content.Load<Texture2D>("base");
            numTex = new Texture2D[10];
            numbers = new Sprite[10];
            for(int i = 0; i < 10; i++)
            {
                numTex[i] = Content.Load<Texture2D>($"{i}");
                numbers[i] = new Sprite(numTex[i], Vector2.Zero, Color.White, Vector2.Zero, new Vector2(2f, 2f));
            }
            

            Vector2 scale = new Vector2(
            graphics.PreferredBackBufferWidth / (float)backTex.Width,
            graphics.PreferredBackBufferHeight / (float)backTex.Height
            );
            background = new Sprite(
                backTex,
                Vector2.Zero,
                Color.White,
                Vector2.Zero,
                scale
            );
            font = Content.Load<SpriteFont>("Arial");

            scale = new Vector2(
                graphics.PreferredBackBufferWidth / (float)groundText.Width,
                groundHeight / (float)groundText.Height
            );
            Ground = new Sprite(groundText, new Vector2(0, graphics.PreferredBackBufferHeight - groundHeight), Color.White, Vector2.Zero, scale);
            birdSprite = new Sprite(birdTex, birdStart, Color.White, new Vector2(0,0), new Vector2(1.75f, 1.75f));
            sigmoidFunc = new ActivationFunction(
               x => 1 / (1 + Math.Exp(-x)),
               x =>
               {
                   double y = 1 / (1 + Math.Exp(-x));
                   return y * (1 - y);
               });
            errorFunction = new ErrorFunction(
                (input, expected) => Math.Pow(expected - input, 2),
                (input, expected) => (input - expected));

            Random random = new Random();


            //Games = new (Bird, List<Pipes>)[100];
            birds = new Bird[nn.Length];
            float randY = random.Next(0, (ScreenSize-groundHeight) - GAPSIZE);
            for (int i = 0; i < nn.Length; i++)
            {
                nn[i] = new NeuralNetwork(errorFunction, sigmoidFunc, new int[] { 2, 4, 1 });
                nn[i].Randomize(random, -1, 1);

                birds[i] = (new Bird(sb, birdStart, Size, nn[i], birdSprite));
            }
            pipes = new List<Pipes>();

            pipes.Add(new Pipes(
                screenMoveSpeed,
                sb,
                new Vector2(graphics.PreferredBackBufferWidth, randY),
                graphics.PreferredBackBufferHeight, pipeTex)
            );
            trainer = new Trainer();
            population = new (NeuralNetwork, double)[nn.Length];


            


            // TODO: use this.Content to load your game content here
        }

        
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            birdsLeft = birds.Count(b => !b.gameEnd);

            for (int t = 0; t < updateTime; t++)
            {
                if (birds.All(b => b.gameEnd))
                {
                    ResetGame();
                }
                groundMove += screenMoveSpeed;

                float groundWidth = groundText.Width * Ground.Scale.X;

                if (groundMove <= -groundWidth)
                {
                    groundMove += groundWidth;
                }
                float speed = pipes[0].Speed;
                for (int i = 0; i < pipes.Count; i++)
                {

                    pipes[i].Move();
                    if (pipes[i].Location.X + pipes[i].PipeSize < 0)
                    {
                        pipes.Remove(pipes[i]);
                    }
                    else if (pipes.Count < 2 && pipes[i].Location.X + pipes[i].PipeSize < graphics.PreferredBackBufferWidth / 2)
                    {
                        float randY = random.Next(0, (ScreenSize - groundHeight) - GAPSIZE);
                        pipes.Add(new Pipes(screenMoveSpeed, sb,
                        new Vector2(graphics.PreferredBackBufferWidth, randY), graphics.PreferredBackBufferHeight, pipeTex));
                    }

                    if(birds.Any(b => b.hitbox.Left > pipes[i].Location.X + pipes[i].PipeSize && !b.gameEnd) && !pipeCount.Contains(pipes[i]))
                    {
                        pipeCount.Add(pipes[i]);
                        currentScore++;
                        if (currentScore > highScore)
                        {
                            highScore = currentScore;
                            //currentScore = 0;
                        }
                    }
                }




                for (int j = 0; j < nn.Length; j++)
                {

                    if (birds[j].gameEnd)
                    {
                        birds[j].Fall();
                        if (birds[j].hitbox.Y + birds[j].hitbox.Height > graphics.PreferredBackBufferHeight - groundHeight)
                        {
                            birds[j].HitGround();
                            birds[j].Location = new Vector2(
                                birds[j].Location.X,
                                graphics.PreferredBackBufferHeight - groundHeight - birds[j].hitbox.Height
                            );
                        }
                    }
                    else
                    {
                        if (birds[j].hitbox.Y + birds[j].hitbox.Height > graphics.PreferredBackBufferHeight - groundHeight)
                        {
                            birds[j].KillBird(speed);
                            birds[j].Location = new Vector2(
                                birds[j].Location.X,
                                graphics.PreferredBackBufferHeight - groundHeight - birds[j].hitbox.Height
                            );
                            birds[j].HitGround();
                        }
                        else if (birds[j].hitbox.Y < 0)
                        {
                            birds[j].KillBird(speed);
                        }
                        else if (birds[j].hitbox.Right < 0)
                        {
                            birds[j].StopBird();
                        }
                        birds[j].Fitness++;

                        Pipes nextPipe = pipes.First(p => p.Location.X + p.PipeSize > birds[j].hitbox.Left);
                        double xPipeDistance = (nextPipe.Location.X - birds[j].hitbox.Right) / graphics.PreferredBackBufferWidth * 2 - 1;

                        double yPipeDistance = (nextPipe.enterPoint.Center.Y - birds[j].hitbox.Center.Y) / graphics.PreferredBackBufferHeight * 2 - 1;

                        double[] output = birds[j].net.Compute([xPipeDistance, yPipeDistance]);
                        birds[j].Fall();
                        if (output[0] > 0.5)
                        {
                            birds[j].Jump();
                        }

                        if (pipes.Any(p => p.Intersects(birds[j].hitbox)))
                        {
                            birds[j].KillBird(speed);
                        }

                    }

                }
            }



                
           
            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            sb.Begin();
            background.Draw(sb, 1);

            foreach (var pipe in pipes)
            {
                pipe.Draw();
            }
            
            foreach (var bird in birds)
            {
                bird.Draw();
            }
            float groundY = graphics.PreferredBackBufferHeight - groundHeight;
            float groundWidth = groundText.Width * Ground.Scale.X;


            for (float x = groundMove; x < graphics.PreferredBackBufferWidth; x += groundWidth)
            {
                sb.Draw(
                    groundText,
                    new Vector2(x, groundY),
                    null,
                    Color.White,
                    0f,
                    Vector2.Zero,
                    Ground.Scale,
                    SpriteEffects.None,
                    0f
                );
            }

            string infoText = $"Generation: {generation}  |  Birds: {birdsLeft}/{numOfBirds}  |  High Score: {highScore}";
            Vector2 textSize = font.MeasureString(infoText);


            Vector2 textPosition = new Vector2(
                (graphics.PreferredBackBufferWidth - textSize.X) / 2,
                graphics.PreferredBackBufferHeight - textSize.Y - groundHeight/4
            );

            sb.DrawString(font, infoText, textPosition, Color.Black);
            DrawScore(currentScore, new Vector2(graphics.PreferredBackBufferWidth / 2 + graphics.PreferredBackBufferWidth/20, 50));

            // TODO: Add your drawing code here
            sb.End();
            base.Draw(gameTime);
        }

        public void DrawScore(int score, Vector2 centerPos)
        {
            string  strScore = score.ToString();

            float width = numTex[0].Width * numbers[0].Scale.X;
            float spacing = 5f;
            float totalWidth = (strScore.Length * width) + ((strScore.Length - 1) * spacing);

            float startX = centerPos.X - (totalWidth / 2);
            float currentX = startX;
            foreach (char c in strScore)
            {
                int digit = c - '0'; 

                sb.Draw(numTex[digit],new Vector2(currentX, centerPos.Y),null,Color.White,0f,
                    new Vector2(numTex[digit].Width / 2, numTex[digit].Height / 2), numbers[digit].Scale,SpriteEffects.None,0f);

                currentX += width + spacing;
            }
        }


        public void Train()
        {
            for(int i = 0; i < nn.Length; i++)
            {
                population[i] = (birds[i].net, birds[i].Fitness);
            }
            double bestFitness = birds.Max(b => b.Fitness);
       //     Debug.WriteLine($"Best fitness: {bestFitness}");
            trainer.Train(population, random, mutationRate);
            for(int i = 0; i < nn.Length; i++)
            {
                birds[i].net = population[i].Item1;
                birds[i].Fitness = 0;
            }
        }

        public void ResetGame()
        {
            Train();
            currentScore = 0;
            pipeCount.Clear();
            generation++;
            birdsLeft = numOfBirds;
            float randY = random.Next(0, (ScreenSize - groundHeight) - GAPSIZE);

            for (int i = 0; i < nn.Length; i++)
            {
                birds[i].Reset(birdStart);
            }
            pipes.Clear();
            pipes.Add(new Pipes(screenMoveSpeed,sb,
                    new Vector2(graphics.PreferredBackBufferWidth, randY)
                    ,graphics.PreferredBackBufferHeight, pipeTex));
        }
    }
}
