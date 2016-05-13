using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Johnny_Punchfucker
{
    class Barrel : GameObjects
    {
        public int hitPoints;
        public bool HpFull, HPMedium, HPLow;
        public Rectangle textureBox;
        int moveTexX;

        float floatLayerNr;
        public Barrel(Texture2D tex, Vector2 pos)
            : base(tex, pos)
        {
            hitPoints = 3;
            isDead = false;
            moveTexX = 10;
            textureBox = new Rectangle(moveTexX,0, 65, 100);
        }
        public void Update(GameTime gameTime)
        {
            textureBox.X = moveTexX;         
            boundingBox = new Rectangle((int)pos.X, (int)pos.Y +25 , (int)(width / 4f), (int)(height / 2f));
            FloatLayerCalculator();
            BarrelState();
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(tex, pos, textureBox, Color.White, 0, Vector2.Zero, 1.8f, SpriteEffects.None, floatLayerNr);
            spriteBatch.Draw(TextureManager.playerShadow, new Vector2(pos.X + 293 , pos.Y +160), null, new Color(0, 0, 0, 80), 0f, new Vector2(width / 2, height - height / 1.3f), 1.2f, SpriteEffects.None, 0.1f);
            spriteBatch.Draw(tex, boundingBox, null, Color.Red, 0, Vector2.Zero, SpriteEffects.None, 0.9f);
        }
       
 
        public void FloatLayerCalculator()
        {
            floatLayerNr = 0 + (pos.Y + 65) * 0.0010f;
        }
        public void BarrelState()
        {
            if (hitPoints == 3)
                HpFull = true;
            else if (hitPoints == 2)
            {
                HpFull = false;
                HPMedium = true;
                moveTexX = 77;          
            }
            else if (hitPoints == 1)
            {
                HpFull = false;
                HPMedium = false;
                HPLow = true;
                moveTexX = 145;
            }
            if (hitPoints <= 0)
                isDead = true;
        }
    }
}
