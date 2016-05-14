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
    class Item : GameObjects
    {
        float floatLayerNr;
        public Item(Texture2D tex, Vector2 pos)
            : base(tex, pos)
        {
            boundingBox = new Rectangle((int)pos.X + 9, (int)pos.Y + 60, (int)(width / 1.5f), (int)(height / 2f));
        }

        public virtual void Update(GameTime gameTime)
        {
            
            FloatLayerCalculator();
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(tex, pos, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, floatLayerNr);
            spriteBatch.Draw(tex, boundingBox, null, Color.Black, 0, Vector2.Zero, SpriteEffects.None, 0.9f);
        }

        public void FloatLayerCalculator()
        {
            floatLayerNr = 0 + (pos.Y + 15) * 0.0010f;
        }
    }
}
