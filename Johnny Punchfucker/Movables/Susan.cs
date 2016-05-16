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
    class Susan : Enemy
    {
        List<Player> playerList;
        EnemyManager enemyManager;
        public bool GotNr, hitByShockwave, shootUp, initShockwaves;
        public static bool dead, enemySpawned;
        double shockwaveSpawnTime, animationTime;
        int shockwaveSpawn, phase, bulletCount, previousHealth, frame;
        Rectangle shockwaveRectangle, shockwaveAnimation;
        Vector2 shootPos, evadePos, shockwavePos, bannerPos;


        public Susan(Texture2D tex, Vector2 pos, bool AggroOnSpawn, float health, List<Player> playerList, EnemyManager enemyManager)
            : base(tex, pos, AggroOnSpawn, health)
        {
            this.playerList = playerList;
            this.enemyManager = enemyManager;
            bannerPos = new Vector2(-600, 200);
            animationBox = new Rectangle(0, 0, 112, 174);
            spriteEffect = SpriteEffects.FlipHorizontally;
            phase = 0;
            width /= 9;
            height /= 9;
            aggroRadius = 0;
            damageToPlayer -= 1;
            previousHealth = (int)life;
            enemySpeed = 2.25f;
            scale = 1.2f;
            offset = new Vector2(width / 2, height / 2);
            enemySpawned = true;

            shockwaveAnimation = new Rectangle(0, 0, 1920, 123);
            shootPos = new Vector2(2300, 675);
            evadePos = new Vector2(2400, 200);
            shockwavePos = new Vector2(630, 502);
        }

        public override void Update(GameTime gameTime)
        {
            spriteEffect = SpriteEffects.FlipHorizontally;
            pos += velocity;
            animationBox.Y = 0;

            UpdateBanner();

            if (initShockwaves && !dead)
                SpawnShockwaves(gameTime);

            if (life <= 0)
                dead = true;
            if (!dead) //Controls the bossfight if boss isn't dead
                BossFight(gameTime, playerList, enemyManager);

            if (!hitByShockwave)
                ShockWaveDamage(playerList);

            if (phase == 0 || phase == 1 || phase == 3 || phase == 5)
                boundingBox = new Rectangle((int)pos.X - width / 2, (int)pos.Y - height / 2, 0, 0);
            else
                boundingBox = new Rectangle((int)pos.X - width / 2, (int)pos.Y - height / 2, width - 15, height - 10);

            feetBox = new Rectangle((int)pos.X - (int)73, (int)pos.Y + (183 - 4) - (int)offset.Y, width - 10, height - (height - 4));

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(TextureManager.playerShadow, new Vector2(pos.X + 5, pos.Y + ((height / 2) + 8)), null, new Color(0, 0, 0, 120), 0f, new Vector2(width / 2, height - height / 1.3f), 1, SpriteEffects.None, 0.1f);

            #region drawSusan
            if (life <= 1 && !dead) //när bossen har lågt hp = röd och arg
                spriteBatch.Draw(tex, pos, animationBox, color, 0f, offset, scale, spriteEffect, floatLayerNr);
            else if (life > 1 && !dead && phase == 0) // om bossen är normalfärgad. Inte död och inte lågt hp och inte spawnad
                spriteBatch.Draw(tex, pos, animationBox, Color.White, 0f, offset, scale, spriteEffect, 0.2f);
            else if (life > 1 && !dead && phase != 0) // om bossen är normalfärgad. Inte död och inte lågt hp
                spriteBatch.Draw(tex, pos, animationBox, Color.White, 0f, offset, scale, spriteEffect, floatLayerNr);
            else if (whiteNdead && dead) // om han är död blir han vit
                spriteBatch.Draw(tex, pos, animationBox, new Color(255, 255, 255, 0), 0f, offset, scale, spriteEffect, floatLayerNr);
            else if (!whiteNdead && dead) //normalfärg mellan blinkningar
                spriteBatch.Draw(tex, pos, animationBox, Color.White, 0f, offset, scale, spriteEffect, floatLayerNr);
            #endregion

            if (initShockwaves && !dead) //ritar ut shockwaves
            {
                if (shockwaveSpawnTime < 1000)
                    spriteBatch.Draw(TextureManager.sandripple, new Rectangle((int)shockwavePos.X, (int)shockwavePos.Y + 123 * shockwaveSpawn, 1920, 123), shockwaveAnimation, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0.05f);
                if (shockwaveSpawnTime >= 1000)
                    spriteBatch.Draw(TextureManager.shockwave, shockwaveRectangle, shockwaveAnimation, Color.Blue, 0, Vector2.Zero, SpriteEffects.None, 0.05f);
            }


            if (phase == 0) //draw banner
                spriteBatch.Draw(TextureManager.susanBanner, bannerPos, null, Color.White, 0f, offset, scale, SpriteEffects.None, 1);
        }

        private void BossFight(GameTime gameTime, List<Player> playerList, EnemyManager enemyManager)
        {
            if (phase == 0)
                DramaticEntrance();
            if (phase == 1)
                KnockbackPlayers(playerList);
            if (phase == 2)
                ShootBullets(gameTime, enemyManager.bossAttackList);
            if (phase == 3)
                RunAway(gameTime);
            if (phase == 4)
            {
                if (!initShockwaves) initShockwaves = true;
                CowardPhase(gameTime, enemyManager.bossAttackList, enemyManager.enemyList, playerList);
            }
            if (phase == 5)
                BackToAttack();


        }

        private void DramaticEntrance()
        {
            moving = true;
            if (pos.Y < 502 && pos.Y > 675)
            {
                pos.Y += 1.6f;
                floatLayerNr = 0.4f;
            }
            if (pos.Y < 675)
                pos.Y += 1.6f;
            else
                phase = 1;
        }

        private void SpawnShockwaves(GameTime gameTime)
        {
            if (!GotNr)
                shockwaveSpawn = RandomizeShockwave();

            shockwaveSpawnTime += gameTime.ElapsedGameTime.TotalMilliseconds;

            if (shockwaveSpawnTime <= 1000)
            {
                RippleAnimation(gameTime);
            }
            if (shockwaveSpawnTime >= 1000)
            {
                shockwaveRectangle = new Rectangle((int)shockwavePos.X, (int)shockwavePos.Y + 123 * shockwaveSpawn, 1920, 123);
                ShockWaveAnimation(gameTime);
            }

            if (shockwaveSpawnTime >= 1300)
            {
                shockwaveSpawnTime = 0;
                shockwaveRectangle = new Rectangle((int)shockwavePos.X, (int)shockwavePos.Y + 123 * shockwaveSpawn, 0, 0);
                GotNr = false; //false so pick new number for next shockwave
                hitByShockwave = false; //false so take damage by next shockwave
            }

        }

        private int RandomizeShockwave()
        {
            int spawnNr = Game1.random.Next(0, 3);

            while (spawnNr == shockwaveSpawn)
                spawnNr = Game1.random.Next(0, 3);
            shockwaveSpawn = spawnNr;

            GotNr = true;
            return spawnNr;
        }

        public void ShockWaveDamage(List<Player> playerList)
        {
            for (int i = 0; i < playerList.Count; i++)
            {
                float playerFeetPos = playerList[i].feetBox.Y;
                float bottomSideOfShockWave = (502 + 123 * shockwaveSpawn) + 123;

                if (shockwaveRectangle.Intersects(playerList[i].boundingBox) && playerFeetPos < bottomSideOfShockWave) //om man tar i shockwave och inte står under vågen
                {
                    hitByShockwave = true;
                    playerList[i].life -= 2f;
                    if ((playerList[i] == playerList[0]) && playerList[0].life >= 1)
                    {
                        playerList[0].PlayerOneHurt();
                    }
                    if (PlayerManager.players == 2 && (playerList[i] == playerList[1]) && playerList[1].life >= 1)
                    {
                        playerList[1].PlayerTwoHurt();
                    }
                }
            }
        }

        private void KnockbackPlayers(List<Player> playerList)
        {
            int knockbackPos = 800; //617
            previousHealth = (int)life;
            if (PlayerManager.players == 1)
            {
                if (playerList[0].pos.X > knockbackPos)
                    playerList[0].pos.X -= 30;
                else
                    phase = 2;
            }
            else if(PlayerManager.players == 2)
            {
                if (playerList[0].pos.X > knockbackPos)
                    playerList[0].pos.X -= 30;
                if (playerList[1].pos.X > knockbackPos)
                    playerList[1].pos.X -= 30;
                else if (playerList[0].pos.X < knockbackPos && playerList[1].pos.X < knockbackPos)
                    phase = 2;
            }

        }

        private void ShootBullets(GameTime gameTime, List<BossAttacks> bossAttacks)
        {
            moving = false;
            bossShootTimer += gameTime.ElapsedGameTime.TotalMilliseconds;

            if (bossShootTimer >= 350)
            {
                bossShootTimer = 0;
                bossAttacks.Add(new Bullet(TextureManager.bulletTex, new Vector2(pos.X - 30, pos.Y), -1, -0.3f + (0.05f * bulletCount)));
                if (AudioManager.sound)
                    AudioManager.Bullet.Play();
                if (shootUp)
                    bulletCount--;
                else
                    bulletCount++;
            }
            if (bulletCount < 0 && shootUp)
                shootUp = false;
            else if (bulletCount > 10 && !shootUp)
                shootUp = true;

            if (previousHealth > life)
                phase = 3;
        }

        private void RunAway(GameTime gameTime)
        {
            Vector2 direction = evadePos - pos;
            direction.Normalize();
            moving = true;

            velocity.X = 5f * direction.X;
            velocity.Y = 5f * direction.Y;

            if (pos.Y < 240)
            {
                phase = 4;
                spriteEffect = SpriteEffects.FlipHorizontally;
                velocity = new Vector2(0, 0);
                moving = false;
                punch = false;
                enemySpawned = false;
            }
        }

        private void CowardPhase(GameTime gameTime, List<BossAttacks> bossAttacks, List<Enemy> enemyList, List<Player> playerList)
        {
            //When going into phase enemies will spawn, and when they are dead big bullets will be spawned following players
            if (enemyList.Count < 2 && enemySpawned)
            {
                phase = 5;
                //bossShootTimer += gameTime.ElapsedGameTime.TotalMilliseconds;

                //if (bossShootTimer >= 500 && playerList.Count == 1)
                //{

                //    bossAttacks.Add(new BigBullet(TextureManager.bulletTex, new Vector2(pos.X, pos.Y), playerList[0].pos.X, playerList[0].pos.Y));
                //    BigBulletSpawned = true;
                //}
                //else if (bossShootTimer >= 500 && playerList.Count == 2)
                //{
                //    bossAttacks.Add(new BigBullet(TextureManager.bulletTex, new Vector2(pos.X, pos.Y), playerList[0].pos.X, playerList[0].pos.Y));
                //    bossAttacks.Add(new BigBullet(TextureManager.bulletTex, new Vector2(pos.X, pos.Y), playerList[1].pos.X, playerList[1].pos.Y));
                //    BigBulletSpawned = true;
                //}

            }

        }

        private void BackToAttack()
        {
            moving = true;
            Vector2 direction = shootPos - pos;
            direction.Normalize();

            velocity.X = 10f * direction.X;
            velocity.Y = 10f * direction.Y;

            if (Vector2.Distance(pos, shootPos) < 30)
            {
                phase = 1;
                velocity = new Vector2(0, 0);
                moving = false;

            }
        }

        private void RippleAnimation(GameTime gameTime)
        {
            animationTime -= gameTime.ElapsedGameTime.TotalMilliseconds;
            if (animationTime <= 0)
            {
                animationTime = 100;
                frame++;
                shockwaveAnimation.Y = (frame % 2) * 123;
            }
        }

        private void ShockWaveAnimation(GameTime gameTime)
        {
            animationTime -= gameTime.ElapsedGameTime.TotalMilliseconds;
            if (animationTime <= 0)
            {
                animationTime = 100;
                frame++;
                shockwaveAnimation.Y = (frame % 3) * 123;
            }
        }

        private void UpdateBanner()
        {
            if (phase == 0)
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
