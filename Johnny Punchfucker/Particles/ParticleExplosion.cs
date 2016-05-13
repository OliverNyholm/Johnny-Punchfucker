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
    class ParticleExplosion : GameObjects
    {
        ParticleEnginge particleEngine;
        List<Texture2D> particleList;

        public static List<ParticleExplosion> explosionList = new List<ParticleExplosion>();

        double timer;

        public ParticleExplosion(Texture2D tex, Vector2 pos, Color color)
            : base(tex, pos)
        {
            timer = 260;
            particleList = new List<Texture2D>();
            particleList.Add(tex);

            particleEngine = new ParticleEnginge(particleList, pos, color);
            particleEngine.nrOfParticle();

            explosionList.Add(this);

        }
        public void Update(GameTime gameTime)
        {
            particleEngine.Update();

            timer -= gameTime.ElapsedGameTime.TotalMilliseconds;
            if (timer <= 0)
            {
                IsDead = true;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            particleEngine.Draw(spriteBatch);
        }
    }
}
