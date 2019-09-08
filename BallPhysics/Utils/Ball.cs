using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using C3.MonoGame;
using System.Diagnostics;

public enum BallTypes
{
    Green,
    Red
}

namespace BallPhysics
{
    public class Ball
    {
        public Vector2 Center { get; set; }
        public float Radius { get; set; }
        public BallTypes BallType { get; set; }
        public float Mass { get; set; }
        public bool ApplyGravity { get; set; }
        private int ballSegments = 36;

        public List<Ball> ballsToIgnoreOnCurrentCollision;
        Vector2 movementVector = Vector2.Zero;

        public Ball(BallTypes ballType, Vector2 startPosition, float radius = 20.0f, float mass = 1.0f, bool applyGravity = false, int ballSegments = 36)
        {
            BallType = ballType;
            Center = startPosition;
            Radius = radius;
            Mass = mass;
            ApplyGravity = applyGravity;
            this.ballSegments = ballSegments;
            ballsToIgnoreOnCurrentCollision = new List<Ball>();
        }

        public void Update(GameTime gameTime)
        {
            Vector2 ballMovement = Vector2.Zero;

            // Do all calculations
            if (BallType == BallTypes.Green)
            {
                ballMovement.Y += Game1.Gravity;
            }

            movementVector += ballMovement;
            if (movementVector.Y > 7)
                movementVector.Y = 7;

            Center += movementVector;
        }

        public void Draw(SpriteBatch spriteBatch)
        {

            var ballColor = (Color)typeof(Color).GetProperty(Enum.GetName(typeof(BallTypes), BallType)).GetValue(null);
            spriteBatch.DrawCircle(Center, Radius, ballSegments, ballColor, thickness: Radius);


        }

        public bool IsCollidingWith(Ball otherBall)
        {
            return Vector2.Distance(Center, otherBall.Center) <= (Radius + otherBall.Radius);

        }

        private void ComputeCollisionForBalls(Ball otherBall)
        {
            //move balls one frame back so they dont overlap;
            this.Center -= this.movementVector;
            otherBall.Center -= otherBall.movementVector;

            //balls were created inside of each other.
            //just destroy one of them
            if (this.IsCollidingWith(otherBall))
            {
                this.Center = new Vector2(2000, 2000);
                return;
            }

            Vector2 v1prim = this.movementVector - ((2 * otherBall.Mass / (this.Mass + otherBall.Mass)) *
                (Vector2.Dot((this.movementVector - otherBall.movementVector), (this.Center - otherBall.Center)) /
                Vector2.DistanceSquared(this.Center, otherBall.Center)) *
                (this.Center - otherBall.Center));

            Vector2 v2prim = otherBall.movementVector - ((2 * this.Mass / (this.Mass + otherBall.Mass)) *
                (Vector2.Dot((otherBall.movementVector - this.movementVector), (otherBall.Center - this.Center)) /
                Vector2.DistanceSquared(otherBall.Center, this.Center)) *
                (otherBall.Center - this.Center));

            this.movementVector = v1prim;
            otherBall.movementVector = v2prim;
        }

        public static void ApplyCollisionBetweenBalls(Ball ball, Ball otherBall)
        {
            if (ball == otherBall)
                return;
            if (ball.ballsToIgnoreOnCurrentCollision.Contains(otherBall))
                return;
            if (ball.IsCollidingWith(otherBall))
            {
                otherBall.ballsToIgnoreOnCurrentCollision.Add(ball);
                ball.ComputeCollisionForBalls(otherBall);
            }
            //ball.BallType == BallTypes.Green && otherBall.BallType == BallTypes.Green
        }

        public bool IsOutOfScreen(int width, int height)
        {
            var leftSide = Center.X - Radius;
            var rightSide = Center.X + Radius;
            var topSide = Center.Y - Radius;
            var bottomSide = Center.Y + Radius;

            if (rightSide < 0 || bottomSide < 0)
                return true;

            if (leftSide > width || topSide > height)
                return true;

            return false;
        }


    }
}
