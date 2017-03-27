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
    class GrandMother
    {

        #region Fields

        //different players support
        GameConstants.Player playerNumber;

        //graphics support
        Texture2D sprite;
        Rectangle destRectangle;
        Rectangle sourceRectangle;

        //animation support
        float elapsedTime;
        int delay = 200;
        int frame = 0;
        int tempFrame = 0;

        // movement support
        int x_position;
        int y_position;
        int x_offset;
        int y_offset;

        #endregion

        #region Constructors

        /// <summary>
        ///  Constructs a Granny
        /// </summary>
        /// <param name="contentManager">the content manager for loading content</param>
        /// <param name="spriteName">the sprite name</param>
        /// <param name="x">the x location of the center</param>
        /// <param name="y">the y location of the center</param>
        /// <param name="y">number of player</param>
        public GrandMother(ContentManager contentManager, string spriteName, int x, int y, GameConstants.Player player)
        {
            // load content and set remainder of draw rectangle
            this.sprite = contentManager.Load<Texture2D>(spriteName);
            this.playerNumber = player;
            this.x_position = x;
            this.x_offset = sprite.Width / 6;
            this.y_position = y;
            this.y_offset = sprite.Height / 8;
            this.destRectangle = new Rectangle(this.x_position - this.x_offset, this.y_position - this.y_offset,
                                                (int)(1.2 * this.sprite.Width / 3), (int)(1.2 * this.sprite.Height / 4));
            this.sourceRectangle = new Rectangle(0, 0, this.sprite.Width / 3, this.sprite.Height / 4);
        }

        #endregion

        #region Public Properties

        public Rectangle CollisionRactangle
        {
            get
            {
                //offsets are needed because of alpha-canals and to give more realism visually
                return new Rectangle(this.destRectangle.Left + 10, this.destRectangle.Top + 10,
                    this.destRectangle.Width - 20, this.destRectangle.Height - 20);
            }

        }

        #endregion

        #region Public Methods

        public void Update(GameTime gameTime, KeyboardState keyboard)
        {
            this.elapsedTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            // looping of animation support
            if (this.elapsedTime >= this.delay)
            {
                if (this.frame >= 2)
                {
                    this.frame = 0;
                }
                else
                {
                    this.frame++;
                }
                this.elapsedTime = 0;
            }

            // keyboard manegement
            if (playerNumber == GameConstants.Player.One)
            {
                this.playerOneMovement(keyboard);
            }
            else
            {
                this.playerTwoMovement(keyboard);
            }

            // clamp Grannies in window
            if (playerNumber == GameConstants.Player.One)
            {
                this.clampPlayerOne();
            }
            else
            {
                this.clampPlayerTwo();
            }

            this.destRectangle = new Rectangle(this.x_position - this.x_offset, this.y_position - this.y_offset,
                                                (int)(1.2 * this.sprite.Width / 3), (int)(1.2 * this.sprite.Height / 4));

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.sprite, this.destRectangle, this.sourceRectangle, Color.White);
        }

        #endregion

        #region Private Methods

        //player one controller support
        private void playerOneMovement(KeyboardState keyboard)
        {
            if (keyboard.IsKeyDown(Keys.Right))
            {
                this.tempFrame = (this.sprite.Height / 4) * 2;
                this.x_position += GameConstants.PLAYER_MOVEMENT_SPEED;
                this.sourceRectangle = new Rectangle((this.sprite.Width / 3) * this.frame, this.tempFrame,
                                                        this.sprite.Width / 3, this.sprite.Height / 4);
            }
            else if (keyboard.IsKeyDown(Keys.Left))
            {
                this.tempFrame = (this.sprite.Height / 4);
                this.x_position -= GameConstants.PLAYER_MOVEMENT_SPEED;
                this.sourceRectangle = new Rectangle((this.sprite.Width / 3) * this.frame, this.tempFrame,
                                                        this.sprite.Width / 3, this.sprite.Height / 4);
            }
            else if (keyboard.IsKeyDown(Keys.Up))
            {
                this.tempFrame = (this.sprite.Height / 4) * 3;
                this.y_position -= GameConstants.PLAYER_MOVEMENT_SPEED;
                this.sourceRectangle = new Rectangle((this.sprite.Width / 3) * this.frame, this.tempFrame,
                                                        this.sprite.Width / 3, this.sprite.Height / 4);
            }
            else if (keyboard.IsKeyDown(Keys.Down))
            {
                this.tempFrame = 0;
                this.y_position += GameConstants.PLAYER_MOVEMENT_SPEED;
                this.sourceRectangle = new Rectangle((this.sprite.Width / 3) * this.frame, this.tempFrame,
                                                        this.sprite.Width / 3, this.sprite.Height / 4);
            }
            else
            {
                this.sourceRectangle = new Rectangle(0, this.tempFrame, this.sprite.Width / 3, this.sprite.Height / 4);
            }
        }

        //player two controller support
        private void playerTwoMovement(KeyboardState keyboard)
        {
            if (keyboard.IsKeyDown(Keys.D))
            {
                this.tempFrame = (this.sprite.Height / 4) * 2;
                this.x_position += GameConstants.PLAYER_MOVEMENT_SPEED;
                this.sourceRectangle = new Rectangle((this.sprite.Width / 3) * this.frame, this.tempFrame,
                                                        this.sprite.Width / 3, this.sprite.Height / 4);
            }
            else if (keyboard.IsKeyDown(Keys.A))
            {
                this.tempFrame = (this.sprite.Height / 4);
                this.x_position -= GameConstants.PLAYER_MOVEMENT_SPEED;
                this.sourceRectangle = new Rectangle((this.sprite.Width / 3) * this.frame, this.tempFrame,
                                                        this.sprite.Width / 3, this.sprite.Height / 4);
            }
            else if (keyboard.IsKeyDown(Keys.W))
            {
                this.tempFrame = (this.sprite.Height / 4) * 3;
                this.y_position -= GameConstants.PLAYER_MOVEMENT_SPEED;
                this.sourceRectangle = new Rectangle((this.sprite.Width / 3) * this.frame, this.tempFrame,
                                                        this.sprite.Width / 3, this.sprite.Height / 4);
            }
            else if (keyboard.IsKeyDown(Keys.S))
            {
                this.tempFrame = 0;
                this.y_position += GameConstants.PLAYER_MOVEMENT_SPEED;
                this.sourceRectangle = new Rectangle((this.sprite.Width / 3) * this.frame, this.tempFrame,
                                                        this.sprite.Width / 3, this.sprite.Height / 4);
            }
            else
            {
                this.sourceRectangle = new Rectangle(0, this.tempFrame, this.sprite.Width / 3, this.sprite.Height / 4);
            }
        }

        //function that keeps the player one in the given borders
        private void clampPlayerOne()
        {
            // for X-axes (with respect to strange sprite)
            if (this.x_position - this.x_offset + 3 < GameConstants.WINDOW_WIDTH / 2)
            {
                this.x_position = GameConstants.WINDOW_WIDTH / 2 + this.x_offset - 3;
            }
            else if (this.x_position + this.x_offset > GameConstants.WINDOW_WIDTH)
            {
                this.x_position = GameConstants.WINDOW_WIDTH - this.x_offset;
            }
            // for Y-axes
            if (this.y_position - this.y_offset + 5 < 0)
            {
                this.y_position = this.y_offset - 5;
            }
            else if (this.y_position + this.y_offset + 9 > GameConstants.WINDOW_HEIGHT)
            {
                this.y_position = GameConstants.WINDOW_HEIGHT - this.y_offset - 9;
            }
        }

        //function that keeps the player two in the given borders
        private void clampPlayerTwo()
        {
            // for X-axes (with respect to strange sprite)
            if (this.x_position - this.x_offset + 3 < 0)
            {
                this.x_position = this.x_offset - 3;
            }
            else if (this.x_position + this.x_offset > GameConstants.WINDOW_WIDTH / 2)
            {
                this.x_position = GameConstants.WINDOW_WIDTH / 2 - this.x_offset;
            }
            // for Y-axes
            if (this.y_position - this.y_offset + 5 < 0)
            {
                this.y_position = this.y_offset - 5;
            }
            else if (this.y_position + this.y_offset + 9 > GameConstants.WINDOW_HEIGHT)
            {
                this.y_position = GameConstants.WINDOW_HEIGHT - this.y_offset - 9;
            }
        }

        #endregion
    }
}
