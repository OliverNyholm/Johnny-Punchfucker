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
    class Enemy : Entity
    {
        protected int aggroRadius;
        protected Vector2 velocity, direction, acceleration;
        public int damageToPlayer, hurtSound;
        protected float enemySpeed, scale;
        public Color color;
        public double bossShootTimer, bossDropBombTimer;
        public bool AggroOnSpawn;
        protected bool enraged = false, hurtTalk;
        private TimeSpan hurtTalkTimer;


        public Enemy(Texture2D tex, Vector2 pos, bool AggroOnSpawn, float health)
            : base(tex, pos)
        {
            this.AggroOnSpawn = AggroOnSpawn;
            color = new Color(190, 90, 90);
            bossShootTimer = 380;
            life = health;
            
        }

        public override void Update(GameTime gameTime)
        {
            

            if (hurtTalk)
            {
                if (hurtTalkTimer.TotalSeconds > 0)
                    hurtTalkTimer = hurtTalkTimer.Subtract(gameTime.ElapsedGameTime);
                else
                {
                    hurtTalk = false;
                }

            }
            if (!dead)
            {
                //boundingBox = new Rectangle((int)pos.X - width / 2, (int)pos.Y - height / 2, width, height);
                AnimationTypes(gameTime);
            }
            else
                punchBox = new Rectangle((int)pos.X - 44, (int)pos.Y - 65, 0, 0); //tar bort punchboxen om han dör

            Death(gameTime);
            FloatLayerCalculator();

            if ((fightFrame == 0 && !moving) || walkFrame == 0)
            {
                animationBox.X = 0;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(TextureManager.playerShadow, new Vector2(pos.X, pos.Y + (height / 2)), null, new Color(0, 0, 0, 120), 0f, new Vector2(width / 2, height - height / 1.3f), 1, SpriteEffects.None, 0.1f);
            if (whiteNdead) // om han är död blir han vit
                spriteBatch.Draw(tex, pos, animationBox, new Color(255, 255, 255, 0), 0f, offset, scale, spriteEffect, floatLayerNr);
            else if (!enraged)// om han inte är död är han färggrann
                spriteBatch.Draw(tex, pos, animationBox, Color.White, 0f, offset, scale, spriteEffect, floatLayerNr);
            else if (enraged)
                spriteBatch.Draw(tex, pos, animationBox, color, 0f, offset, scale, spriteEffect, floatLayerNr);
            spriteBatch.Draw(tex, feetBox, null, Color.Red, 0f, offset, spriteEffect, 0.8f);
            //spriteBatch.Draw(tex, punchBox, null, Color.Red, 0f, offset, spriteEffect, 0.8f);
            //spriteBatch.Draw(tex, boundingBox, null, Color.Red, 0f, offset, spriteEffect, 0.8f);

        }

        public void AnimationTypes(GameTime gameTime)
        {

            if (moving)
            {
                Animation(150, 3, 112, gameTime);
            }
            else if (!moving && !punch)
                Animation(150, 1, 112, gameTime);

            if (stunned)
            {
                animationBox.Y = 370;
                animationBox.X = 0;
                fightingCooldown = 0; //3 rader ner = resettar fiendens slag
                fightFrame = 0;
                punchBox = new Rectangle((int)pos.X - 44, (int)pos.Y - 65, 0, 0);
                stunnedTimer += gameTime.ElapsedGameTime.TotalMilliseconds;
                if (stunnedTimer >= 250)
                {
                    stunned = false;
                    stunnedTimer = 0;
                }
            }
        }

        public void Aggro(List<Player> player, List<Enemy> enemy) // aggrofunktionen när dom är nära spelaren
        {

            animationBox.Y = 0;
            Vector2 feetPos = new Vector2(feetBox.X, feetBox.Y);


            int i = 0;
            if (PlayerManager.players == 2) //kollar avståndet mellan spelarna. Den med minst avstånd går fienden till
            {
                float dist1 = Vector2.Distance(pos, player[0].pos);
                float dist2 = Vector2.Distance(pos, player[1].pos);

                if (dist1 > dist2)
                    i = 1;
            }
            if (PlayerManager.players == 2 && player[0].dead) //om spelare ett dör går han direkt till spelare 2
                i = 1;


            Vector2 playerfeetPos = new Vector2(player[i].feetBox.X, player[i].feetBox.Y);
            if (Vector2.Distance(feetPos, playerfeetPos) < aggroRadius && !(feetBox.Intersects(player[i].playerRightBox) || feetBox.Intersects(player[i].playerLeftBox)))
            {
                moving = true;

                if (feetPos.X + feetBox.Width / 2 < playerfeetPos.X)
                {
                    direction.X = player[i].playerLeftPos.X - feetPos.X;
                    direction.Y = player[i].playerLeftPos.Y - feetPos.Y;
                }
                else
                {
                    direction.X = player[i].playerRightPos.X - 3 - feetPos.X;
                    direction.Y = player[i].playerRightPos.Y - 3 - feetPos.Y;
                }

                direction.Normalize();
                velocity.X = enemySpeed * direction.X;
                velocity.Y = enemySpeed * direction.Y;

                velocity = Vector2.Add(velocity, acceleration);
                if (velocity.Length() > enemySpeed) //truncate
                {
                    velocity.Normalize();
                    velocity *= enemySpeed;
                }


                if ((velocity.X < 5 && velocity.X > -5) && (velocity.Y < 5 && velocity.Y > -5))
                    pos += velocity;

                if (direction.X < 0)
                    spriteEffect = SpriteEffects.FlipHorizontally;
                else if (direction.X > 0)
                    spriteEffect = SpriteEffects.None;
            }
            else
                moving = false;

            if (Vector2.Distance(feetPos, playerfeetPos) < aggroRadius && !(feetBox.Intersects(player[i].playerRightBox) || feetBox.Intersects(player[i].playerLeftBox))
                && fightFrame >= 1) //om fienden är mitt i ett slag och hamnar ur range från spelaren så resettas animationen till animation.X = 0;
                animationBox.X = 0;

            if (feetBox.Intersects(player[i].playerRightBox))
                spriteEffect = SpriteEffects.FlipHorizontally;
            else if (feetBox.Intersects(player[i].playerLeftBox))
                spriteEffect = SpriteEffects.None;

            acceleration = acceleration * 0.0f;
        }

        public void SpawnAggro(List<Player> player) // aggrofunktionen när dom spawnar och inte är i aggrozonen
        {
            int i = 0;
            if (PlayerManager.players == 2)
            {
                float dist1 = Vector2.Distance(pos, player[0].pos);
                float dist2 = Vector2.Distance(pos, player[1].pos);

                if (dist1 > dist2)
                    i = 1;
            }

            Vector2 feetPos = new Vector2(feetBox.X, feetBox.Y);
            Vector2 playerfeetPos = new Vector2(player[i].feetBox.X, player[i].feetBox.Y);
            if (Vector2.Distance(feetPos, playerfeetPos) > aggroRadius)
            {
                moving = true;
                if (feetPos.X < playerfeetPos.X)
                {
                    direction = player[i].playerLeftPos - feetPos;
                }
                else
                    direction = player[i].playerRightPos - feetPos;

                direction.Normalize();
                velocity.X = enemySpeed * direction.X;
                velocity.Y = enemySpeed * direction.Y;
                pos += velocity;

                if (direction.X < 0)
                    spriteEffect = SpriteEffects.FlipHorizontally;
                else
                    spriteEffect = SpriteEffects.None;

            }
        }

        public void Fight(GameTime gameTime, List<Player> player)
        {
            int pcount = 0;
            for (int i = 0; i < player.Count; i++)
            {
                if (feetBox.Intersects(player[i].playerLeftBox) || feetBox.Intersects(player[i].playerRightBox) && !stunned)
                {
                    pcount++;
                    if (pcount == 2)
                        enraged = true;
                    else
                        enraged = false;
                    fightingCooldown += gameTime.ElapsedGameTime.TotalMilliseconds;
                    punch = true;
                    if (fightingCooldown > 500)
                    {
                        animationBox.Y = 577;
                        FightAnimation(150, 3, 138, gameTime);
                        #region slagets Hitbox
                        if (spriteEffect == SpriteEffects.FlipHorizontally) // om han är vänd åt ena hållet så rör sig slagets hitbox beroende på vilken frame den är på
                        {
                            if (fightFrame >= 1)
                            {
                                punchBox = new Rectangle((int)pos.X + 22, (int)pos.Y - 90, 45, 30);
                            }
                            if (fightFrame >= 2)
                            {
                                punchBox = new Rectangle((int)pos.X - 66, (int)pos.Y - 27, 40, 30);
                            }
                        }
                        else
                        {
                            if (fightFrame >= 1)
                            {
                                punchBox = new Rectangle((int)pos.X - 66, (int)pos.Y - 97, 45, 30);
                            }
                            if (fightFrame >= 2)
                            {
                                punchBox = new Rectangle((int)pos.X + 8, (int)pos.Y - 22, 40, 30);
                            }
                        }
                        #endregion
                    }
                }

                if (PlayerManager.players == 2)
                {
                    if (!(feetBox.Intersects(player[0].playerLeftBox) || feetBox.Intersects(player[0].playerRightBox)))
                    {
                        if (!(feetBox.Intersects(player[1].playerLeftBox) || feetBox.Intersects(player[1].playerRightBox)))
                        {
                            fightingCooldown = 0; //3 rader ner = resettar fiendens slag
                            fightFrame = 0;
                            punchBox = new Rectangle((int)pos.X - 44, (int)pos.Y - 65, 0, 0);
                        }
                    }
                }
                else
                {
                    if (!(feetBox.Intersects(player[0].playerLeftBox) || feetBox.Intersects(player[0].playerRightBox)))
                    {
                        fightingCooldown = 0; //3 rader ner = resettar fiendens slag
                        fightFrame = 0;
                        punchBox = new Rectangle((int)pos.X - 44, (int)pos.Y - 65, 0, 0);
                    }
                }

            }

            if (punch && fightFrame >= 3 && fightFrameTime <= 70)
            {
                punch = false;
                hasHit = false;
                fightFrame = 0;
                fightingCooldown = -200;
                punchBox = new Rectangle((int)pos.X - 44, (int)pos.Y - 65, 0, 0); //resettar slaget hitbox ovanför gubben igen
            }
        }

        public void Death(GameTime gameTime)
        {
            if (life <= 0)
            {
                dead = true;
                animationBox.Y = 957;
                animationBox.X = 0;
                animationBox.Width = 187;
                deathTimer1 += gameTime.ElapsedGameTime.TotalMilliseconds;
                boundingBox = new Rectangle((int)pos.X - width / 2, (int)pos.Y - height / 2, 0, 0);

                if (deathTimer1 >= 1500)
                {
                    deathTimer2 += gameTime.ElapsedGameTime.TotalMilliseconds;
                    if (deathTimer2 >= 150 && !whiteNdead)
                    {
                        whiteNdead = true; //gör gubben vit
                        deathBlinkCount++; //räknar, efter 7 blinkningar deletas gubben från banan
                        deathTimer2 = 0;
                    }
                    if (deathTimer2 >= 150 && whiteNdead)
                    {
                        whiteNdead = false; //gör gubben färggrann
                        deathBlinkCount++;
                        deathTimer2 = 0;
                    }
                }
            }
        }

        public void FloatLayerCalculator()
        {
            floatLayerNr = 0 + pos.Y * 0.0010f; //numret blir mellan 0.335 och 0.583, vilket placerar en i rätt ordning(ritas först 0, ritas sist 1(?))
        }

        public void BossShoot(List<BossAttacks> bossAttacksList, GameTime gameTime, int dirNr)
        {
            bossShootTimer += gameTime.ElapsedGameTime.TotalMilliseconds;

            if (bossShootTimer >= 400)
            {
                bossShootTimer = 0;
                bossAttacksList.Add(new Bullet(TextureManager.bulletTex, new Vector2(pos.X, pos.Y + 15), dirNr, 0));
                if (AudioManager.sound)
                    AudioManager.Bullet.Play();
            }
        }

        public void BossDropBomb(List<BossAttacks> bossAttacksList, GameTime gameTime)
        {
            bossDropBombTimer += gameTime.ElapsedGameTime.TotalMilliseconds;

            if (bossDropBombTimer >= 700)
            {


                bossDropBombTimer = 0;
                bossAttacksList.Add(new Bomb(TextureManager.bombTex, new Vector2(pos.X, pos.Y), TextureManager.explosionTex));
            }
        }

        public void BossHurt()
        {
            hurtSound = Game1.random.Next(1, 7);

            if (!hurtTalk && AudioManager.sound)
            {
                hurtTalk = true;
                switch (hurtSound)
                {
                    case 1:
                        AudioManager.Mingy_CutItOute.Play();
                        hurtTalkTimer = TimeSpan.FromMilliseconds(AudioManager.Mingy_CutItOute.Duration.TotalMilliseconds);
                        break;
                    case 2:
                        AudioManager.Mingy_keepTryin.Play();
                        hurtTalkTimer = TimeSpan.FromMilliseconds(AudioManager.Mingy_keepTryin.Duration.TotalMilliseconds);
                        break;
                    case 3:
                        AudioManager.Mingy_Ouch.Play();
                        hurtTalkTimer = TimeSpan.FromMilliseconds(AudioManager.Mingy_Ouch.Duration.TotalMilliseconds);
                        break;
                    case 4:
                        AudioManager.Mingy_niceTry.Play();
                        hurtTalkTimer = TimeSpan.FromMilliseconds(AudioManager.Mingy_niceTry.Duration.TotalMilliseconds);
                        break;
                    case 5:
                        AudioManager.Mingy_mjlk.Play();
                        hurtTalkTimer = TimeSpan.FromMilliseconds(AudioManager.Mingy_mjlk.Duration.TotalMilliseconds);
                        break;
                    case 6:
                        AudioManager.Mingy_ScrewThis.Play();
                        hurtTalkTimer = TimeSpan.FromMilliseconds(AudioManager.Mingy_ScrewThis.Duration.TotalMilliseconds);
                        break;
                }
            }

        }
    }
}
