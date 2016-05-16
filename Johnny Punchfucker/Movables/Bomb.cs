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
    class Bomb : BossAttacks
    {
        double frameTime = 700;
        Texture2D tex2;
        Rectangle animationBox;
        public Bomb(Texture2D tex, Vector2 pos, Texture2D tex2)
            : base(tex, pos)
        {
            this.tex2 = tex2;
            width /= 3;
            bombTimer = 1;
            floatLayerOffsetY = 16;
            animationBox = new Rectangle(0, 0, 156, 150);
        }

        public override void Update(GameTime gameTime)
        {
            bombTimer += gameTime.ElapsedGameTime.TotalMilliseconds;

            boundingBox = new Rectangle((int)pos.X, (int)pos.Y, width, height);

            frameTime -= gameTime.ElapsedGameTime.TotalMilliseconds;
            if (!exploded)
            {
                if (frameTime <= 0)
                {
                    frameTime = 700;
                    frame++;
                    animationBox.X = (frame % 3) * 160;
                }
            }
            else
            {
                animationBox.Width = 120;
                animationBox.Height = 120;
                width /= 10;
                if (frameTime <= 0)
                {
                    frameTime = 60;
                    frame++;
                    animationBox.X = (frame % 10) * 120;
                }
            }

            if (!exploded && frame >= 3)
            {
                if (AudioManager.sound)
                    AudioManager.Explosion.Play();
                exploded = true;
                frameTime = 60;
            }
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!exploded)
                spriteBatch.Draw(tex, pos, animationBox, Color.White, 0, Vector2.Zero, 1, spriteEffects, floatLayerNr);
            else
                spriteBatch.Draw(tex2, pos, animationBox, Color.White, 0, Vector2.Zero, 1, spriteEffects, floatLayerNr);

            spriteBatch.Draw(TextureManager.playerShadow, new Vector2(pos.X + 75, pos.Y + 112), null, new Color(0, 0, 0, 120), 0f, new Vector2(94 / 2, 31 / 2), 0.7f, SpriteEffects.None, 0.1f);

            //spriteBatch.Draw(tex, boundingBox, null, Color.Black, 0, Vector2.Zero, SpriteEffects.None, 0.9f);
        }
    }
}
