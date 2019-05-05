using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using C3.MonoGame;
using System;
using System.Collections.Generic;

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

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 800;
            graphics.PreferredBackBufferWidth = 1000;
            Content.RootDirectory = "Content";
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
            
            // The active state from the last frame is now old
            lastMouseState = currentMouseState;

            // Get the mouse state relevant for this frame
            currentMouseState = Mouse.GetState();

            if (lastMouseState.LeftButton == ButtonState.Released && currentMouseState.LeftButton == ButtonState.Pressed)
            {
                greenBalls.Add(new Ball(BallTypes.Green, currentMouseState.Position.ToVector2(), RNG.Next(15, 30), applyGravity:true));
            }

            if (lastMouseState.RightButton == ButtonState.Released && currentMouseState.RightButton == ButtonState.Pressed)
            {
                redBalls.Add(new Ball(BallTypes.Red, currentMouseState.Position.ToVector2(), RNG.Next(30, 40)));
            }


            foreach (var ball in greenBalls)
            {
                ball.Update(gameTime);
            }
            foreach (var ball in redBalls)
            {
                ball.Update(gameTime);
            }


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
            HUDSpriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
