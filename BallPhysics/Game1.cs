using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using C3.MonoGame;
using System;
using System.Collections.Generic;
using System.Diagnostics;

// Starter code for implementing Goal-Based Vector Field Pathfinding
// The explanation can be found at:
// https://gamedevelopment.tutsplus.com/tutorials/understanding-goal-based-vector-field-pathfinding--gamedev-9007


namespace BallPhysics
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteBatch HUDSpriteBatch;

        List<Ball> greenBalls;

        Grid tileGrid;
        Spawner ballSpawner;

        MouseState lastMouseState;
        MouseState currentMouseState;

        Vector2 mousePosition = Vector2.Zero;

        SpriteFont HUDFont;

        string explanationText = "All green balls have to 'chase' the mouse cursor";

        Random RNG = new Random();

        FrameCounter frameCounter = new FrameCounter();

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferHeight = 800,
                PreferredBackBufferWidth = 1200
            };
            Content.RootDirectory = "Content";

            // IF fixed framerate
            //int targetFPS = 90;
            //IsFixedTimeStep = true;
            //graphics.SynchronizeWithVerticalRetrace = false; //Vsync
            //TargetElapsedTime = System.TimeSpan.FromMilliseconds(1000.0f / targetFPS);


            // ELSE (variable timestep)
            graphics.SynchronizeWithVerticalRetrace = false; //Vsync
            IsFixedTimeStep = false;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            greenBalls = new List<Ball>();
            ballSpawner = new Spawner(new Vector2(graphics.PreferredBackBufferWidth/2, graphics.PreferredBackBufferHeight/2), 
                                        new Vector2(graphics.PreferredBackBufferWidth/8, graphics.PreferredBackBufferHeight/8), 1000);

            int tileWidth = 25, tileHeight = 25;
            tileGrid = new Grid(graphics.PreferredBackBufferWidth/tileWidth, graphics.PreferredBackBufferHeight / tileHeight, tileWidth, tileHeight);

            this.IsMouseVisible = true;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            HUDSpriteBatch = new SpriteBatch(GraphicsDevice);

            HUDFont = Content.Load<SpriteFont>("hudFontSmall");

            // TODO: use this.Content to load your game content here


        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //Update FPS Count
            frameCounter.Update((float)gameTime.ElapsedGameTime.TotalSeconds);

            ballSpawner.Update(gameTime, greenBalls);


            // The active state from the last frame is now old
            lastMouseState = currentMouseState;

            // Get the mouse state relevant for this frame
            currentMouseState = Mouse.GetState();
            mousePosition = currentMouseState.Position.ToVector2();

            // To make it easier clamp to window size
            mousePosition.X = Math.Max(Math.Min(graphics.PreferredBackBufferWidth, mousePosition.X), 0);
            mousePosition.Y = Math.Max(Math.Min(graphics.PreferredBackBufferHeight, mousePosition.Y), 0);


            foreach (var ball in greenBalls)
            {
                ball.Update(gameTime);
            }


            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            tileGrid.Draw(spriteBatch);

            foreach (var ball in greenBalls)
            {
                ball.Draw(spriteBatch);
            }


            spriteBatch.End();

            HUDSpriteBatch.Begin();
            HUDSpriteBatch.DrawString(HUDFont, explanationText, new Vector2(20, 20), Color.White);
            HUDSpriteBatch.DrawString(HUDFont, "FPS: " + frameCounter.CurrentFramesPerSecond.ToString("0.0") + "   Average FPS: " + frameCounter.AverageFramesPerSecond.ToString("0.0"), new Vector2(20, 40), Color.White);
            HUDSpriteBatch.DrawString(HUDFont, "Number of balls: " + greenBalls.Count, new Vector2(20, 60), Color.White);
            HUDSpriteBatch.DrawString(HUDFont, "Mouse position: " + mousePosition, new Vector2(20, 80), Color.White);

            HUDSpriteBatch.End();


            base.Draw(gameTime);
        }
    }
}
