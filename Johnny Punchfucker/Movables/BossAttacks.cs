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
    class BossAttacks : GameObjects
    {
        public float floatLayerNr, floatLayerOffsetY;
        public int frame;
        protected SpriteEffects spriteEffects;
        public double bulletTimer, bombTimer;
        public bool exploded, explosionHit;
        public BossAttacks(Texture2D tex, Vector2 pos)
            : base(tex, pos)
        {
            spriteEffects = SpriteEffects.None;
        }

        public virtual void Update(GameTime gameTime)
        {
            boundingBox = new Rectangle((int)pos.X, (int)pos.Y, width, height);
            FloatLayerCalculator();
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(tex, pos, null, Color.White, 0, Vector2.Zero, 1, spriteEffects, floatLayerNr);
            //spriteBatch.Draw(tex, boundingBox, null, Color.Black, 0, Vector2.Zero, SpriteEffects.None, 0.9f);
        }

        public void FloatLayerCalculator()
        {
            floatLayerNr = 0 + (pos.Y + floatLayerOffsetY) * 0.0010f;
        }
    }
}
