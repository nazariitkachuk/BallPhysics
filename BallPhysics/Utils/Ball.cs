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

        Vector2 movementVector = Vector2.Zero;

        public Ball(BallTypes ballType, Vector2 startPosition, float radius = 20.0f, float mass = 1.0f, bool applyGravity = false)
        {
            BallType = ballType;
            Center = startPosition;
            Radius = radius;
            Mass = mass;
            ApplyGravity = applyGravity;
        }

        public void Update(GameTime gameTime)
        {
            Vector2 ballMovement = Vector2.Zero;

            // Do all calculations

            movementVector += ballMovement;
        }

        public void Draw(SpriteBatch spriteBatch)
        {

            var ballColor = (Color)typeof(Color).GetProperty(Enum.GetName(typeof(BallTypes), BallType)).GetValue(null);
            spriteBatch.DrawCircle(Center, Radius, 360, ballColor, thickness:Radius);


        }

        public bool IsCollidingWith(Ball otherBall)
        {
            return Vector2.Distance(Center, otherBall.Center) <= (Radius + otherBall.Radius);

        }

        


    }
}
