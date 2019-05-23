using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using C3.MonoGame;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace BallPhysics
{
    public class Grid
    {
        public int gridWidth = 0, gridHeight = 0, tileWidth = 0, tileHeight = 0;

        public Tile[,] tiles;

        public Grid(int gridWidth, int gridHeight, int tileWidth, int tileHeight)
        {
            this.gridWidth = gridWidth;
            this.gridHeight = gridHeight;
            this.tileWidth = tileWidth;
            this.tileHeight = tileHeight;

            tiles = new Tile[gridHeight, gridWidth];

            for (int i = 0; i < gridHeight ; i++)
            {
                for (int j = 0; j < gridWidth; j++)
                {
                    bool isObstacle = ( (j > 5 * gridWidth / 8 && j < 6*gridWidth/8 && i > 4 * gridHeight / 8)
                                       || 
                                       (j > 2*gridWidth /8  && j < 3*gridWidth / 8 && i < gridHeight/2)
                                       ||
                                       (i > 4 * gridHeight / 8 && i < 5*gridHeight / 8 && j > 5 * gridWidth / 8 && j < 7 * gridWidth/8)
                                       ) ? true : false;

                    tiles[i, j] = new Tile(tileWidth, tileHeight, tileWidth * j, tileHeight * i, isObstacle);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var tile in tiles )
            {
                tile.Draw(spriteBatch);
            }
        }
    }


    public class Tile
    {
        public Rectangle rect;
        public Vector2 center;

        public bool isObstacle;

        Tile(){
        }

        public Tile(int tileWidth, int tileHeight, int posX, int posY, bool isObstacle)
        {
            rect = new Rectangle(posX, posY, tileWidth, tileHeight);
            center = new Vector2(posX + tileWidth / 2, posY + tileHeight / 2);
            this.isObstacle = isObstacle;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (isObstacle)
                spriteBatch.FillRectangle(rect, Color.Red);
            else
                spriteBatch.DrawRectangle(rect, Color.Black);
        }

        
    }

}
