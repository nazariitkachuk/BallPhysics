using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using C3.MonoGame;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace BallPhysics
{
    public class Spawner
    {

        Random RNG = new Random();

        Vector2 position = Vector2.Zero;
        Vector2 spread = Vector2.Zero;
        int targetNumberOfBalls = 0;
        int ballCounter = 0;
        int ballsPerSecond = 100;

        public Spawner(Vector2 position, Vector2 spread, int targetNumberOfBalls)
        {
            this.position = position;
            this.spread = spread;
            this.targetNumberOfBalls = targetNumberOfBalls;

        }

        public void Update(GameTime gameTime, List<Ball> balls)
        {
            var elapsedTime = gameTime.ElapsedGameTime.TotalSeconds;

            if (ballCounter <= targetNumberOfBalls)
            {
                int ballsToGenerate = (int)Math.Ceiling(Math.Max(1, elapsedTime * ballsPerSecond));
                for (int i = 0; i < ballsToGenerate; i++)
                {
                    Vector2 position = new Vector2(this.position.X + RNG.Next((int)-spread.X / 2, (int)spread.X / 2),
                                                    this.position.Y + RNG.Next((int)-spread.Y / 2, (int)spread.Y / 2));
                    balls.Add(new Ball(BallTypes.Green, position, 2, ballSegments:5));
                    ballCounter++;
                }
            }

        }

    }
}
