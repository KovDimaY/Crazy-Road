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
    class Blood
    {
        #region Fields

        bool isActive = true;
        Texture2D sprite;
        Rectangle drawRectangle;
        float bloodElapsedTime = 0;

        #endregion

        #region Constructors

        public Blood(ContentManager content, int x, int y)
        {
            this.sprite = content.Load<Texture2D>("Pictures/blood");
            this.drawRectangle = new Rectangle(x - this.sprite.Width / 2,
                y - this.sprite.Height / 2, this.sprite.Width, this.sprite.Height);

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

        public void Update(GameTime time)
        {
            this.bloodElapsedTime += time.ElapsedGameTime.Milliseconds;
            if (this.bloodElapsedTime > 3000)
            {
                this.isActive = false;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (this.isActive)
            {
                spriteBatch.Draw(this.sprite, this.drawRectangle, Color.AliceBlue);
            }
        }

        #endregion
    }
}
