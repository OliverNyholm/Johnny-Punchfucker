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
    class EnemyManager
    {
        public List<Enemy> enemyList = new List<Enemy>();
        public List<BossAttacks> bossAttackList = new List<BossAttacks>();
        public ParticleExplosion particleExplosion;

        public EnemyManager(GraphicsDeviceArcade graphicsDevice)
        {

        }

        public void Update(GameTime gameTime)
        {
            foreach (Enemy enemy in enemyList)
            {
                enemy.Update(gameTime);
            }
            foreach (BossAttacks bossAttack in bossAttackList)
            {
                bossAttack.Update(gameTime);
            }
            RemoveEnemy();
            BossAttacks(gameTime);


        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Enemy enemy in enemyList)
            {
                enemy.Draw(spriteBatch);
            }
            foreach (BossAttacks bossAttack in bossAttackList)
            {
                bossAttack.Draw(spriteBatch);
            }
        }

        public void RemoveEnemy()
        {
            for (int i = 0; i < enemyList.Count; i++)
            {
                if (enemyList[i].deathBlinkCount >= 7) // när han blinkat vitt ett par gånger så dör han
                {
                    enemyList.RemoveAt(i);
                }
            }
        }

        public void AggroPlayer(PlayerManager playerManager, GameTime gameTime)
        {
            for (int i = 0; i < enemyList.Count; i++)
            {
                if (!(enemyList[i] is Mingy) && !(enemyList[i] is Vitas) && !(enemyList[i] is Susan))
                {
                    if (!enemyList[i].dead && !enemyList[i].stunned && !enemyList[i].AggroOnSpawn && Game1.ready)
                    {
                        enemyList[i].Aggro(playerManager.playerList, enemyList);
                        enemyList[i].Fight(gameTime, playerManager.playerList);
                    }
                    else if (!enemyList[i].dead && !enemyList[i].stunned && enemyList[i].AggroOnSpawn && Game1.ready)
                    {
                        enemyList[i].SpawnAggro(playerManager.playerList);
                        enemyList[i].Aggro(playerManager.playerList, enemyList);
                        enemyList[i].Fight(gameTime, playerManager.playerList);
                    }
                }

            }
        }

        public void KeepDistance(GameTime gameTime)
        {
            float speed = 2.25f;
            for (int i = 0; i < enemyList.Count; i++)
            {
                for (int j = 0; j < enemyList.Count; j++)
                {
                    if (i == j || enemyList[i].dead || enemyList[j].dead)
                        continue;
                    if (Vector2.Distance(enemyList[i].pos, enemyList[j].pos) < 30)
                    {
                        if (enemyList[i].pos.Y < enemyList[j].pos.Y)
                        {
                            enemyList[i].pos.Y -= (float)speed;
                            enemyList[j].pos.Y += (float)speed;
                        }
                        else
                        {
                            enemyList[i].pos.Y += (float)speed;
                            enemyList[j].pos.Y -= (float)speed;
                        }
                    }
                }
            }
        }

        public void FightPlayer(PlayerManager playerManager)
        {
            for (int i = 0; i < playerManager.playerList.Count; i++)
            {
                for (int j = 0; j < enemyList.Count; j++)
                {
                    float Ydistance = (enemyList[j].feetBox.Y - 4) - (playerManager.playerList[i].feetBox.Y - 4);
                    if (Ydistance < 0)
                    {
                        Ydistance *= -1;
                    }
                    if (enemyList[j].punchBox.Intersects(playerManager.playerList[i].boundingBox)
                       && Ydistance < 25 && !enemyList[j].hasHit && enemyList[j].fightFrame >= 2
                        && !(enemyList[j].punchBox.Intersects(playerManager.playerList[i].blockBox))) // om vi intersectar och vi står i samma y-led och vi inte har träffat än och vi är vid slutet av animationen
                    {
                        enemyList[j].hasHit = true;
                        if (playerManager.playerList[i].life > 0)
                        {
                            playerManager.playerList[i].life -= 1;
                            particleExplosion = new ParticleExplosion(TextureManager.bloodTex, new Vector2(enemyList[j].punchBox.X, enemyList[j].punchBox.Y), Color.DarkRed);
                            if ((playerManager.playerList[i] == playerManager.playerList[0]) && playerManager.playerList[0].life >= 1)
                            {
                                playerManager.playerList[0].PlayerOneHurt();
                                playerManager.playerList[0].Hit();
                            }
                            if (PlayerManager.players == 2 && (playerManager.playerList[i] == playerManager.playerList[1]) && playerManager.playerList[1].life >= 1)
                            {
                                playerManager.playerList[1].PlayerTwoHurt();
                                playerManager.playerList[0].Hit();
                            }
                            break;
                        }
                    }
                }
            }
        }

        public void BossAttacks(GameTime gameTime)
        {
            #region Spawning Boss Attacks
            for (int i = 0; i < enemyList.Count; i++)
            {
                if (enemyList[i] is Mingy && Mingy.shootLeft)
                {
                    enemyList[i].BossShoot(bossAttackList, gameTime, 1);
                }
                if (enemyList[i] is Mingy && Mingy.shootRight)
                {
                    enemyList[i].BossShoot(bossAttackList, gameTime, -1);

                }
                if (enemyList[i] is Mingy && Mingy.dropBomb)
                {
                    enemyList[i].BossDropBomb(bossAttackList, gameTime);
                }
            }
            #endregion
            #region Remove Boss Attacks
            for (int j = 0; j < bossAttackList.Count; j++)
            {
                if (bossAttackList.Count > 0 && bossAttackList[j].bulletTimer >= 3.5f && bossAttackList[j] is Bullet)
                {
                    bossAttackList.RemoveAt(j);
                }
                if (bossAttackList.Count > 0 && bossAttackList[j] is Bomb && bossAttackList[j].frame >= 13)
                {
                    bossAttackList.RemoveAt(j);
                }
            }
            #endregion
        }

        public void BossDamage(PlayerManager playerManager)
        {
            for (int i = 0; i < playerManager.playerList.Count; i++)
            {
                for (int j = 0; j < bossAttackList.Count; j++)
                {
                    if (bossAttackList[j] is Bullet)
                    {
                        float Ydistance = (playerManager.playerList[i].feetBox.Y - 4) - (bossAttackList[j].pos.Y + 78);
                        if (Ydistance < 0)
                        {
                            Ydistance *= -1;

                        }

                        if (bossAttackList[j].boundingBox.Intersects(playerManager.playerList[i].boundingBox) && Ydistance <= 15)
                        {
                            bossAttackList.RemoveAt(j);
                            playerManager.playerList[i].life -= 2;
                            particleExplosion = new ParticleExplosion(TextureManager.bloodTex, new Vector2(playerManager.playerList[i].pos.X, playerManager.playerList[i].pos.Y), Color.DarkRed);

                            if ((playerManager.playerList[i] == playerManager.playerList[0]) && playerManager.playerList[0].life >= 1)
                            {
                                playerManager.playerList[0].PlayerOneHurt();
                            }
                            if (PlayerManager.players == 2 && (playerManager.playerList[i] == playerManager.playerList[1]) && playerManager.playerList[1].life >= 1)
                            {
                                playerManager.playerList[1].PlayerTwoHurt();
                            }
                        }
                    }
                    else if (bossAttackList[j] is Bomb)
                    {
                        float Ydistance = (playerManager.playerList[i].feetBox.Y - 4) - (bossAttackList[j].pos.Y + 97);
                        if (Ydistance < 0)
                        {
                            Ydistance *= -1;
                        }

                        if (bossAttackList[j].boundingBox.Intersects(playerManager.playerList[i].boundingBox) && Ydistance <= 54
                            && bossAttackList[j].exploded && !bossAttackList[j].explosionHit)
                        {

                            bossAttackList[j].explosionHit = true;
                            playerManager.playerList[i].life -= 6;
                            particleExplosion = new ParticleExplosion(TextureManager.bloodTex, new Vector2(playerManager.playerList[i].pos.X, playerManager.playerList[i].pos.Y), Color.DarkRed);

                            if ((playerManager.playerList[i] == playerManager.playerList[0]) && playerManager.playerList[0].life >= 1)
                            {
                                playerManager.playerList[0].PlayerOneHurt();
                            }
                            if (PlayerManager.players == 2 && (playerManager.playerList[i] == playerManager.playerList[1]) && playerManager.playerList[1].life >= 1)
                            {
                                playerManager.playerList[1].PlayerTwoHurt();
                            }
                        }
                    }
                }
            }
        }

        public void IsBlocked(PlayerManager playerManager, GameTime gameTime) //stoppar gubbens slag om han slår på blocken
        {
            for (int i = 0; i < playerManager.playerList.Count; i++)
            {
                for (int j = 0; j < enemyList.Count; j++)
                {
                    if (enemyList[j].punchBox.Intersects(playerManager.playerList[i].blockBox))
                    {
                        enemyList[j].Animation(150, 1, 75, gameTime);
                        enemyList[j].punch = false;
                        enemyList[j].hasHit = false;
                        enemyList[j].fightFrame = 0;
                        enemyList[j].fightingCooldown = -200;
                        enemyList[j].punchBox = new Rectangle((int)enemyList[j].pos.X - 44, (int)enemyList[j].pos.Y - 65, 0, 0); //resettar slaget hitbox ovanför gubben igen

                        particleExplosion = new ParticleExplosion(TextureManager.bloodTex, new Vector2(playerManager.playerList[i].blockBox.X, playerManager.playerList[i].blockBox.Y + 50), Color.Blue);
                    }
                }
            }
        }
    }
}
