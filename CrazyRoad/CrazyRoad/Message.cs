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
    class Message
    {
        #region Fields

        bool isActive = true;
        Texture2D[] tempSprite = new Texture2D[3];
        Rectangle[] tempDrawRectangle = new Rectangle[3];
        Texture2D currentSprite;
        Rectangle currentRectangle;

        #endregion

        #region Constructors

        public Message(ContentManager content)
        {
            for (int i = 0; i < 3; i++)
            {
                this.tempSprite[i] = content.Load<Texture2D>("Message/" + (i + 1).ToString());
                this.tempDrawRectangle[i] = new Rectangle((GameConstants.WINDOW_WIDTH - this.tempSprite[i].Width) / 2,
                    (GameConstants.WINDOW_HEIGHT - this.tempSprite[i].Height) / 2, this.tempSprite[i].Width, this.tempSprite[i].Height);
            }
        }

        #endregion

        #region Properties

        public bool Active
        {
            get { return isActive; }
            set { isActive = value; }
        }


        #endregion

        #region Public Methods

        public void Update(int number)
        {
            this.currentSprite = this.tempSprite[number - 1];
            this.currentRectangle = this.tempDrawRectangle[number - 1];
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (this.isActive)
            {
                spriteBatch.Draw(this.currentSprite, this.currentRectangle, Color.AliceBlue);
            }
        }

        #endregion
    }
}
