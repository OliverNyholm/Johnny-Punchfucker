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
    class StandardEnemy : Enemy
    {
        bool AggroOnSpawn;
        public StandardEnemy(Texture2D tex, Vector2 pos, bool AggroOnSpawn, float health)
            : base(tex, pos, AggroOnSpawn, health)
        {
            animationBox = new Rectangle(0, 0, 112, 174);
            width /= 9;
            height /= 9;
            aggroRadius = 525;
            damageToPlayer -= 1;
            enemySpeed = 2.25f;
            scale = 1;
            offset = new Vector2(width / 2, height / 2);
        }

        public override void Update(GameTime gameTime)
        {
            boundingBox = new Rectangle((int)pos.X - width / 2, (int)pos.Y - height / 2, width - 15, height - 10);
            feetBox = new Rectangle((int)pos.X - (int)55, (int)pos.Y + (169 - 5) - (int)offset.Y, width - 30, height - (height - 5));
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(tex, boundingBox, null, Color.Blue, 0, Vector2.Zero, SpriteEffects.None, 0.9f);
            base.Draw(spriteBatch);
        }
    }
}
