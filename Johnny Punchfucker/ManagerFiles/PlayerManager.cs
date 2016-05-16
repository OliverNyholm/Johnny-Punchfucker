using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;

namespace Johnny_Punchfucker
{
    class PlayerManager
    {
        public List<Player> playerList = new List<Player>();
        public static int players = 2;
        public ParticleExplosion particleExplosion;
        double gameOverDelay;
        public bool deathSound1, deathSound2;
        public PlayerManager()
        {
            AddPlayer();

        }

        public void Update(GameTime gameTime)
        {


            foreach (ParticleExplosion e in ParticleExplosion.explosionList.ToList())//updaterar alla partikeleffekter 
            {
                e.Update(gameTime);
            }
            foreach (ParticleExplosion e in ParticleExplosion.explosionList)
            {
                if (e.IsDead)
                    ParticleExplosion.explosionList.Remove(e);
                break;
            }

            foreach (Player player in playerList)
            {
                player.Update(gameTime);
            }

            #region Om spelarna dör gör dom ljud
            if (playerList[0].life <= 0 && !deathSound1)
            {
                if (AudioManager.sound)
                    AudioManager.Johnny_Oaaooooa.Play();
                deathSound1 = true;
            }
            if (players == 2 && playerList[1].life <= 0 && !deathSound2)
            {
                if (AudioManager.sound)
                    AudioManager.Tommy_Aoa.Play();
                deathSound2 = true;
            }
            //if(players == 2 && playerList[0].life <= 0 && playerList[1].life >= 1)
            //    Camera.smooth = true;
            #endregion
            #region Spelare två kan inte försvinna från skärmen
            if (players == 2) //gör att spelare två inte kan försvinna från skärmen
                if (playerList[0].pos.X >= playerList[1].pos.X + 1600 && !playerList[1].dead)
                {
                    playerList[0].ableToMoveRight = false;
                }
                else
                    playerList[0].ableToMoveRight = true;
            #endregion
            #region if Players Die
            if (players == 1)
            {
                if (playerList[0].dead)
                {
                    gameOverDelay += gameTime.ElapsedGameTime.TotalMilliseconds;

                    if (gameOverDelay >= 2000)
                        GameManager.failed = true;
                }
            }
            else if (players == 2)
            {
                if (playerList[0].dead && playerList[1].dead)
                {
                    gameOverDelay += gameTime.ElapsedGameTime.TotalMilliseconds;

                    if (gameOverDelay >= 2000)
                        GameManager.failed = true;
                }
            }
            #endregion
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Player player in playerList)
            {
                player.Draw(spriteBatch);
            }
            foreach (ParticleExplosion e in ParticleExplosion.explosionList)
            {
                e.Draw(spriteBatch);
            }
        }

        public void AddPlayer()
        {
            if (playerList.Count <= 0 && players == 1)
            {
                playerList.Add(new Player(TextureManager.Player1tex, new Vector2(1200, 550), PlayerIndex.One));
            }
            if (playerList.Count <= 0 && players == 2)
            {
                playerList.Add(new Player(TextureManager.Player1tex, new Vector2(1200, 550), PlayerIndex.One));
                playerList.Add(new Player(TextureManager.Player2tex, new Vector2(1350, 550), PlayerIndex.Two));
            }
        }

        public void LandingPunches(EnemyManager enemyManager)
        {
            for (int i = 0; i < playerList.Count; i++)
            {
                for (int j = 0; j < enemyManager.enemyList.Count; j++)
                {
                    float Ydistance = (playerList[i].feetBox.Y - 4) - (enemyManager.enemyList[j].feetBox.Y - 4);
                    if (Ydistance < 0)
                    {
                        Ydistance *= -1;
                    }

                    if (playerList[i].punchBox.Intersects(enemyManager.enemyList[j].boundingBox)
                       && Ydistance < 25 && !playerList[i].hasHit && playerList[i].fightFrame >= 1) // om vi intersectar och vi står i samma y-led och vi inte har träffat än
                    {
                        playerList[i].hasHit = true;
                        if (!(enemyManager.enemyList[j] is Susan) && !(enemyManager.enemyList[j] is Vitas))
                            enemyManager.enemyList[j].stunned = true;

                        #region Spelare gör ljud vid slag
                        if ((playerList[i] == playerList[0]) && playerList[0].life >= 1 && enemyManager.enemyList[j].life <= 1)
                        {
                            playerList[0].PlayerOneKillHit();
                        }
                        if (PlayerManager.players == 2 && (playerList[i] == playerList[1]) && playerList[1].life >= 1 && enemyManager.enemyList[j].life <= 1)
                        {
                            playerList[1].PlayerTwoKillHit();
                        }
                        #endregion
                        if (playerList[i].spriteEffect == SpriteEffects.FlipHorizontally)
                        {
                            playerList[0].Hit();
                            //enemyManager.enemyList[j].pos.X -= 2;
                            enemyManager.enemyList[j].life -= 1;


                            if (enemyManager.enemyList[j] is Mingy)
                                enemyManager.enemyList[j].BossHurt();

                            particleExplosion = new ParticleExplosion(TextureManager.bloodTex, new Vector2(playerList[i].punchBox.X, playerList[i].punchBox.Y), Color.Red);

                            break;

                        }
                        else
                        {
                            playerList[0].Hit();
                            //enemyManager.enemyList[j].pos.X += 2;
                            particleExplosion = new ParticleExplosion(TextureManager.bloodTex, new Vector2(playerList[i].punchBox.X + playerList[i].punchBox.Width, playerList[i].punchBox.Y), Color.Red);
                            if (enemyManager.enemyList[j] is Mingy)
                                enemyManager.enemyList[j].BossHurt();
                            enemyManager.enemyList[j].life -= 1;
                        }
                        break;
                    }
                }
            }
        }

        public void PunchBarrel(ContentLoader barrel)
        {
            for (int i = 0; i < playerList.Count; i++)
            {
                for (int j = 0; j < barrel.barrels.Count; j++)
                {
                    if (playerList[i].punchBox.Intersects(barrel.barrels[j].boundingBox)
                       && !playerList[i].hasHit && playerList[i].fightFrame >= 1)
                    {
                        playerList[i].hasHit = true;
                        playerList[0].Hit();
                        barrel.barrels[j].hitPoints -= 1;
                        break;
                    }
                }
            }
        }

        public void CollectItems(ContentLoader item)
        {
            for (int i = 0; i < playerList.Count; i++)
            {
                for (int j = 0; j < item.itemList.Count; j++)
                {
                    if (playerList[i].feetBox.Intersects(item.itemList[j].boundingBox))
                    {
                        item.itemList.RemoveAt(j);
                        if (playerList[i].life <= 9)
                            playerList[i].life++;
                        if (playerList[i].life >= 9)
                            playerList[i].life = 10;

                        if (AudioManager.sound)
                            AudioManager.Eat.Play();

                        //particleExplosion = new ParticleExplosion(TextureManager.bloodTex, new Vector2(playerList[0].feetBox.X, playerList[0].feetBox.Y), Color.Yellow);
                    }
                }
            }
        }
    }
}
