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
    class Vitas : Enemy
    {
        List<Player> playerList;
        List<Rectangle> shockwaves = new List<Rectangle>();
        List<Rectangle> fireshieldFloor = new List<Rectangle>();
        public float fireDelay = 2.2f;
        public TimeSpan fireDelayTimer;
        public bool canFire = false;
        public float fireshieldDelay = 0.2f;
        public TimeSpan fireshieldDelayTimer;
        bool fireShield = false;
        int stage = 0;
        bool timerGoing = false;
        Vector2 targetPos;
        int targetintPos = 0;
        Random rnd;
        bool once = true;
        public static bool DEAD = false;
        float oldlife;
        public TimeSpan timer;
        float rot = 0;
        Vector2 bannerPos;
        bool bannerOnce = true;

        public Vitas(Texture2D tex, Vector2 pos, bool AggroOnSpawn, float health, List<Player> playerList)
            : base(tex, pos, AggroOnSpawn, health)
        {
            this.playerList = playerList;
            animationBox = new Rectangle(0, 0, 112, 174);
            spriteEffect = SpriteEffects.FlipHorizontally;
            width /= 9;
            height /= 9;
            aggroRadius = 0;
            damageToPlayer -= 1;
            life = health;
            enemySpeed = 2.25f;
            scale = 1.2f;
            offset = new Vector2(width / 2, height / 2);
            fireDelayTimer = TimeSpan.FromSeconds(fireDelay);
            fireshieldDelayTimer = TimeSpan.FromSeconds(fireshieldDelay);
            rnd = new Random();
            oldlife = life;
            bannerPos = new Vector2(-600, 200);

        }

        public override void Update(GameTime gameTime)
        {

            moving = true;
            animationBox.Y = 0;
            UpdateBanner();

            if (timerGoing)
            {
                if (timer.TotalSeconds > 0)
                    timer = timer.Subtract(gameTime.ElapsedGameTime);
                else
                {
                    if (stage == 0)
                    {
                        stage = 1;
                        canFire = true;
                        fireShield = true;
                    }
                    timerGoing = false;
                }
            }
            if (stage == 0)
            {
                for (int j = 0; j < playerList.Count; j++)
                {
                    if (Vector2.Distance(playerList[j].pos, pos) < 380 && canFire == false)
                    {
                        canFire = true;
                        fireShield = true;
                        timer = TimeSpan.FromSeconds(4);
                        timerGoing = true;
                    }
                }
            }
            else if (stage == 1)
            {
                if (targetintPos % 2 == 1 && once){
                    targetPos = new Vector2(2800, pos.Y);
                    rot = 0;
                }
                else if (once)
                {
                    rot = -0.3f;
                    int n = rnd.Next(0, playerList.Count);
                    targetPos = new Vector2(880, playerList[n].feetBox.Y - 30);
                    if (playerList[0].dead == true)
                    {
                        if (playerList.Count > 1)
                            targetPos = new Vector2(880, playerList[1].feetBox.Y - 30);
                    }
                    if (playerList.Count > 1)
                    {
                        if (playerList[1].dead == true)
                            targetPos = new Vector2(880, playerList[0].feetBox.Y - 30);
                    }

                }

                once = false;
                velocity = targetPos - pos;
                velocity.Normalize();
                if (velocity.X < 0)
                    pos += 28 * velocity;
                else
                    pos += 10 * velocity;
                if (Vector2.Distance(pos, targetPos) < 60)
                {
                    once = true;
                    targetintPos++;
                    if (targetintPos > 9)
                    {
                        stage = 2;
                        targetintPos = 0;
                        fireShield = false;
                        fireshieldFloor.Clear();
                    }
                }
            }
            else if (stage == 2)
            {
                bannerOnce = false;
                fireDelay = 1.3f;
                if (oldlife > life)
                {
                    stage = 0;
                    timer = TimeSpan.FromSeconds(5);
                    timerGoing = true;
                    fireDelay = 2f;
                }
                oldlife = life;
                if (life <= 0)
                {
                    timerGoing = false;
                    stage = 3;
                }


                if (Vector2.Distance(pos, new Vector2(2800, 680)) > 20)
                {
                    velocity = new Vector2(2800, 680) - pos;
                    velocity.Normalize();
                    pos += 2 * velocity;
                }
                



            }

            if (canFire)
            {
                if (fireDelayTimer.TotalSeconds > 0)
                    fireDelayTimer = fireDelayTimer.Subtract(gameTime.ElapsedGameTime);
                else
                {
                    Rectangle rect = new Rectangle(3000, 505, 175, 850);
                    shockwaves.Add(rect);
                    fireDelayTimer = TimeSpan.FromSeconds(fireDelay);
                }
            }


            for (int i = 0; i < shockwaves.Count; i++)
            {
                shockwaves[i] = new Rectangle(shockwaves[i].X - 10, shockwaves[i].Y, shockwaves[i].Width, shockwaves[i].Height);
                for (int j = 0; j < playerList.Count; j++)
                {
                    if ((playerList[j].feetBox.Intersects(shockwaves[i]) && playerList[j].onGround))
                    {
                        if (j == 0)
                            playerList[j].PlayerOneHurt();
                        else
                            playerList[j].PlayerTwoHurt();
                        playerList[j].life -= 0.5f;
                    }
                }
                if (shockwaves[i].X <= -500)
                {
                    shockwaves.RemoveAt(i);
                    i--;
                }
            }

            for (int i = 0; i < fireshieldFloor.Count; i++)
            {
                for (int j = 0; j < playerList.Count; j++)
                {
                    if ((playerList[j].feetBox.Intersects(new Rectangle(fireshieldFloor[i].X + 10, fireshieldFloor[i].Y + 60, fireshieldFloor[i].Width - 10, fireshieldFloor[i].Height - 30))))
                    {
                        if (j == 0)
                            playerList[j].PlayerOneHurt();
                        else
                            playerList[j].PlayerTwoHurt();
                        playerList[j].life -= 0.2f;
                    }
                }
            }

            if (fireShield)
            {
                if (fireshieldDelayTimer.TotalSeconds > 0)
                    fireshieldDelayTimer = fireshieldDelayTimer.Subtract(gameTime.ElapsedGameTime);
                else
                {
                    Rectangle rect = new Rectangle((int)pos.X - 80, (int)pos.Y - 80, 150, 150);
                    fireshieldFloor.Add(rect);
                    fireshieldDelayTimer = TimeSpan.FromSeconds(fireshieldDelay);
                    if (fireshieldFloor.Count > 14)
                        fireshieldFloor.RemoveAt(0);
                }
            }

            if (stage == 2)
                boundingBox = new Rectangle((int)pos.X - width / 2, (int)pos.Y - height / 2, width - 15, height - 10);
            else
                boundingBox = new Rectangle((int)pos.X - width / 2, (int)pos.Y - height / 2, 0, 0);

            feetBox = new Rectangle((int)pos.X - (int)73, (int)pos.Y + (183 - 4) - (int)offset.Y, width - 10, height - (height - 4));

            if (life <= 0)
                DEAD = true;
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(TextureManager.playerShadow, new Vector2(pos.X + 5, pos.Y + ((height / 2) + 8)), null, new Color(0, 0, 0, 120), 0f, new Vector2(width / 2, height - height / 1.3f), 1, SpriteEffects.None, 0.1f);

            #region drawSusan
            if (stage == 2) //när bossen har lågt hp = röd och arg
                spriteBatch.Draw(TextureManager.vitas, pos, animationBox, color, 0, offset, scale, SpriteEffects.None, floatLayerNr);
            else if (stage != 2) // om bossen är normalfärgad. Inte död och inte lågt hp
                spriteBatch.Draw(TextureManager.vitas, pos, animationBox, Color.White, rot, offset, scale, SpriteEffects.None, floatLayerNr);
            else if (whiteNdead && DEAD) // om han är död blir han vit
                spriteBatch.Draw(TextureManager.vitas, pos, animationBox, new Color(255, 255, 255, 0), 0f, offset, scale, SpriteEffects.None, floatLayerNr);
            else if (!whiteNdead && DEAD) //normalfärg mellan blinkningar
                spriteBatch.Draw(TextureManager.vitas, pos, animationBox, Color.Red, 0f, offset, scale, SpriteEffects.None, floatLayerNr);
            #endregion



            foreach (Rectangle r in shockwaves)
            {
                spriteBatch.Draw(TextureManager.shockwaveTex, r, new Rectangle(0, 0, TextureManager.shockwaveTex.Width, TextureManager.shockwaveTex.Height), Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.30f);
            }

            int i = 0;
            foreach (Rectangle r in fireshieldFloor)
            {
                i++;
                if (i % 2 == 0)
                    spriteBatch.Draw(TextureManager.fireshieldTex, r, new Rectangle(0, 0, TextureManager.fireshieldTex.Width, TextureManager.fireshieldTex.Height), new Color(255, 255, 255, 0.3f), 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, floatLayerNr);
                else
                    spriteBatch.Draw(TextureManager.fireshieldTex, r, new Rectangle(0, 0, TextureManager.fireshieldTex.Width, TextureManager.fireshieldTex.Height), new Color(255, 255, 255, 0.3f), 0f, Vector2.Zero, SpriteEffects.None, floatLayerNr);
                spriteBatch.Draw(TextureManager.playerShadow, new Rectangle(r.X + 10, r.Y + 60, r.Width - 10, r.Height - 30), new Rectangle(0, 0, TextureManager.playerShadow.Width, TextureManager.playerShadow.Height), new Color(0, 0, 0, 120), 0f, Vector2.Zero, SpriteEffects.None, floatLayerNr - 0.01f);
            }
            if (stage == 0 && canFire && bannerOnce) //draw banner
                spriteBatch.Draw(TextureManager.susanBanner, bannerPos, null, Color.White, 0f, offset, scale, SpriteEffects.None, 1);
        }

        private void UpdateBanner()
        {
            if (stage == 0 && canFire)
            {
                if (bannerPos.X < (ContentLoader.levelEndPosX - 1938) + 100)
                    bannerPos.X += 10;
                else if (bannerPos.X > (ContentLoader.levelEndPosX - 1938) + 100 && bannerPos.X < (ContentLoader.levelEndPosX - 1938) + 160)
                    bannerPos.X += 0.2f;
                else if (bannerPos.X > (ContentLoader.levelEndPosX - 1938) + 160)
                    bannerPos.X += 50;

            }
        }
    }
}
