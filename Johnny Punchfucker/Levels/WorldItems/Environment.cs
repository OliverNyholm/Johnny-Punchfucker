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
    public class Environment
    {
        protected Vector2 position;
        protected Texture2D texture;
        

        public Environment(Vector2 position, Texture2D texture)
        {
            this.position = position;
            this.texture = texture;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0.002f);
        }
    }
}
