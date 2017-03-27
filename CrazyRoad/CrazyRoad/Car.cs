using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace CrazyRoad
{
    class Car
    {
        #region Fields

        bool active = true;
        GameConstants.Direction carDirection;

        // drawing support
        Texture2D sprite;
        Rectangle drawRectangle;

        // velocity information
        float x_Velocity;

        #endregion

        #region Constructors

        /// <summary>
        ///  Constructs a car with the given x velocity
        /// </summary>
        /// <param name="direction">the car direction</param>
        /// <param name="sprite">the sprite for the car</param>
        /// <param name="x">the x location of the center of the projectile</param>
        /// <param name="y">the y location of the center of the projectile</param>
        /// <param name="xVelocity">the x velocity for the projectile</param>
        public Car(GameConstants.Direction direction, Texture2D sprite, int x, int y,
            float xVelocity)
        {
            this.carDirection = direction;
            this.sprite = sprite;
            this.x_Velocity = xVelocity;
            this.drawRectangle = new Rectangle(x - this.sprite.Width / 2,
                y - this.sprite.Height / 2, this.sprite.Width,
                this.sprite.Height);
        }

        #endregion

        #region Public Properties

        public Rectangle CollisionRactangle
        {
            get
            {
                //offsets are needed because of alpha-canals and to give more realism visually
                return new Rectangle(this.drawRectangle.Left + 15, this.drawRectangle.Top + 15,
                    this.drawRectangle.Width - 30, this.drawRectangle.Height - 30);
            }

        }

        public bool Active
        {
            get { return active; }
        }


        #endregion

        #region Public Methods

        public void Update(GameTime gameTime)
        {
            // move projectile
            if (this.carDirection == GameConstants.Direction.Right)
            {
                this.drawRectangle.X += (int)this.x_Velocity;
            }
            else
            {
                this.drawRectangle.X -= (int)this.x_Velocity;
            }

            // check for outside game window
            if (this.drawRectangle.Y < 0)
            {
                this.active = false;
            }
            else if (this.drawRectangle.Y > GameConstants.WINDOW_HEIGHT)
            {
                this.active = false;
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.sprite, this.drawRectangle, Color.White);
        }

        #endregion
    }
}
