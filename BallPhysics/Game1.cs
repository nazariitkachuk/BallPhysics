﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using C3.MonoGame;
using System;
using System.Collections.Generic;
using System.Diagnostics;

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
        List<Ball> redBalls;

        MouseState lastMouseState;
        MouseState currentMouseState;

        SpriteFont HUDFont;

        string explanationText = "Press Left Mouse to place green balls, press Right Mouse to place red balls";

        Random RNG = new Random();

        FrameCounter frameCounter = new FrameCounter();

        static int ScreenWidth = 1000;
        static int ScreenHeight = 800;
        public static float Gravity = 0.1f;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = ScreenHeight;
            graphics.PreferredBackBufferWidth = ScreenWidth;
            Content.RootDirectory = "Content";


            graphics.SynchronizeWithVerticalRetrace = false; //Vsync
            //IsFixedTimeStep = false;

            //TargetElapsedTime = System.TimeSpan.FromMilliseconds(1000.0f / targetFPS);
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
            redBalls = new List<Ball>();

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


            // The active state from the last frame is now old
            lastMouseState = currentMouseState;

            // Get the mouse state relevant for this frame
            currentMouseState = Mouse.GetState();

            if (lastMouseState.LeftButton == ButtonState.Released && currentMouseState.LeftButton == ButtonState.Pressed)
            {
                greenBalls.Add(new Ball(BallTypes.Green, currentMouseState.Position.ToVector2(), RNG.Next(15, 30), RNG.Next(4, 11), applyGravity: true));
            }

            if (lastMouseState.RightButton == ButtonState.Released && currentMouseState.RightButton == ButtonState.Pressed)
            {
                redBalls.Add(new Ball(BallTypes.Red, currentMouseState.Position.ToVector2(), RNG.Next(30, 40), RNG.Next(8, 25)));
            }

            //prepare balls for checking collision
            foreach (var ball in greenBalls)
            {
                ball.ballsToIgnoreOnCurrentCollision.Clear();
            }
            foreach (var ball in redBalls)
            {
                ball.ballsToIgnoreOnCurrentCollision.Clear();
            }

            //check every-with-every collision, without repeatings
            greenBalls.RemoveAll((ball) => ball.IsOutOfScreen(ScreenWidth, ScreenHeight));
            foreach (var ball in greenBalls)
            {
                foreach (var gb in greenBalls)
                {
                    Ball.ApplyCollisionBetweenBalls(ball, gb);
                }
                foreach (var rb in redBalls)
                {
                    Ball.ApplyCollisionBetweenBalls(ball, rb);
                }
                ball.Update(gameTime);
            }

            redBalls.RemoveAll((ball) => ball.IsOutOfScreen(ScreenWidth, ScreenHeight));
            foreach (var ball in redBalls)
            {
                foreach (var gb in greenBalls)
                {
                    Ball.ApplyCollisionBetweenBalls(ball, gb);
                }
                foreach (var rb in redBalls)
                {
                    Ball.ApplyCollisionBetweenBalls(ball, rb);
                }
                ball.Update(gameTime);
            }

            Debug.Print("FPS: {0}", 1 / (float)gameTime.ElapsedGameTime.TotalSeconds);

            // TODO: Add your update logic here

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

            foreach (var ball in greenBalls)
            {
                ball.Draw(spriteBatch);
            }

            foreach (var ball in redBalls)
            {
                ball.Draw(spriteBatch);
            }


            spriteBatch.End();

            HUDSpriteBatch.Begin();
            HUDSpriteBatch.DrawString(HUDFont, explanationText, new Vector2(20, 20), Color.White);
            HUDSpriteBatch.DrawString(HUDFont, "FPS: " + frameCounter.CurrentFramesPerSecond.ToString("0.0"), new Vector2(20, 40), Color.White);
            HUDSpriteBatch.DrawString(HUDFont, "Average FPS: " + frameCounter.AverageFramesPerSecond.ToString("0.0"), new Vector2(20, 60), Color.White);

            HUDSpriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
