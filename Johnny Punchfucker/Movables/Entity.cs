using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;


namespace Johnny_Punchfucker
{
    abstract class Entity : GameObjects
    {
        public Rectangle animationBox, feetBox, punchBox, blockBox;
        public SpriteEffects spriteEffect;
        public int width, height;
        public int walkFrame, fightFrame;
        public float life, maxLife, percentLife, floatLayerNr;
        public int deathBlinkCount;
        public double frameTime, frameInterval, fightFrameTime, fightFrameInterval;
        public double fightTime, fightingCooldown = 500, stunnedTimer, blockTimer;
        public double deathTimer1, deathTimer2; //Två tider, en när han dör och en som gör att han blinkar vit
        public bool moving, onGround = true, fight, punch, block, stunned, dead, whiteNdead, hasHit; /*,blocking;*/


        public Vector2 posJump, speed;

        public Entity(Texture2D tex, Vector2 pos) : base(tex, pos)
        {
            feetBox = new Rectangle((int)pos.X - width / 2, (int)pos.Y + (height - 4) - height / 2, width, height - (height - 4));

            width = tex.Width;
            height = tex.Height;
        }

        public abstract void Update(GameTime gameTime);

        public abstract void Draw(SpriteBatch spriteBatch);

        public void Animation(int animationSpeed, int animationLength, int animationWidth, GameTime gameTime)
        {
            frameInterval = animationSpeed;

            frameTime -= gameTime.ElapsedGameTime.TotalMilliseconds;

            if (frameTime <= 0)
            {
                frameTime = frameInterval;
                walkFrame++;
                animationBox.X = (walkFrame % animationLength) * animationWidth;
            }
        }
        


        public void FightAnimation(int animationSpeed, int animationLength, int animationWidth, GameTime gameTime)
        {
            fightFrameInterval = animationSpeed;

            fightFrameTime -= gameTime.ElapsedGameTime.TotalMilliseconds;

            if (fightFrameTime <= 0)
            {
                fightFrameTime = fightFrameInterval;
                fightFrame++;
                animationBox.X = (fightFrame % animationLength) * animationWidth;
            }            
        }

    }
}
