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
    class PinaColada : Item
    {
        public PinaColada(Texture2D tex, Vector2 pos)
            : base(tex, pos)
        {

        }

        public override void Update(GameTime gameTime)
        {
            boundingBox = new Rectangle((int)pos.X, (int)pos.Y + 20, (int)(width / 1.5f), (int)(height / 2f));
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(TextureManager.playerShadow, new Vector2(pos.X + 6, pos.Y + height), null, new Color(0, 0, 0, 80), 0f, new Vector2(width / 2, height - height / 1.3f), 0.45f, SpriteEffects.None, 0.1f);
            base.Draw(spriteBatch);
        }
    }
}
