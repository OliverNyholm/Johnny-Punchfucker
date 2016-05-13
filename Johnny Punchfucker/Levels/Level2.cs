using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.IO;

namespace Johnny_Punchfucker
{
    class Level2 : Level
    {
        public ParticleExplosion particleExplosion;
        bool spawn1;

        public Level2(ContentManager Content, PlayerManager playerManager, EnemyManager enemyManager)  : base(Content, playerManager, enemyManager)
        {
            contentLoader = new ContentLoader(Content, @"Content/Levels/lvl2environment.txt", @"Content/Levels/lvl2items.txt");
            SpawnBoss();
        }

        public void Update(GameTime gameTime)
        {
            contentLoader.Update(gameTime);
            CameraStopWhenEnemySpawn(playerManager, gameTime);
            BossFightStart(playerManager);
            SpawnAdds(enemyManager.enemyList);
            ThornDamage(playerManager.playerList);

            //Om Mingy är död så får målet en hitbox
            if (Mingy.died)
                nextLevelBox = new Rectangle(ContentLoader.levelEndPosX, (int)335, 40, 300);
            else
                nextLevelBox = new Rectangle(ContentLoader.levelEndPosX, (int)335, 0, 0);

            //Om spelaren går i mål så kommer man att ha klarat av level 1 och level 2 ska börja
            for (int i = 0; i < playerManager.playerList.Count; i++)
                if (playerManager.playerList[i].boundingBox.Intersects(nextLevelBox))
                {
                    contentLoader.NextLevel(playerManager, enemyManager, 1200);
                    GameManager.levelNr++;
                }

            #region particleeffect
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
            #endregion
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            contentLoader.Draw(spriteBatch);
            spriteBatch.Draw(TextureManager.lifeBarTex, nextLevelBox, Color.Black);
        }

        public void ThornDamage(List<Player> playerList)
        {
            for (int i = 0; i < playerList.Count; i++)
            {
                if (playerList[i].posJump.Y <= 450)
                {
                    playerList[i].life -= 0.012f;

                    particleExplosion = new ParticleExplosion(TextureManager.bloodTex, new Vector2(playerList[i].feetBox.X + 40, playerList[i].feetBox.Y), Color.DarkRed);
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

        private void SpawnBoss()
        {
            enemyManager.enemyList.Add(new Mingy(TextureManager.mingy, new Vector2(3900, 675), false, 12));
        }

        private void SpawnAdds(List<Enemy> enemyList)
        {
            for (int i = 0; i < enemyList.Count; i++) //spawnar fiender under bossfighten
            {
                if (enemyList[i].life <= 6 && enemyList[i] is Mingy && !spawn1)
                {
                    enemyList.Add(new StandardEnemy(TextureManager.standardEnemyTex, new Vector2(3900, 675), true, 6));
                    enemyList.Add(new StandardEnemy(TextureManager.standardEnemyTex, new Vector2(3000, 1200), true, 6));
                    enemyList.Add(new StandardEnemy(TextureManager.standardEnemyTex, new Vector2(2250, 450), true, 6));
                    spawn1 = true;
                    if (PlayerManager.players == 2 && AudioManager.sound)
                        AudioManager.Tommy_YMCA.Play();
                }
            }
        }

        public void BossFightStart(PlayerManager playerManager)
        {
            if (playerManager.playerList[0].pos.X >= 2805)
            {
                Mingy.bossEngaged = true;
            }
        }

        public void CameraStopWhenEnemySpawn(PlayerManager playerManager, GameTime gameTime)
        {
            ContentLoader.levelEndPosX = 3750;
        }
    }
}
