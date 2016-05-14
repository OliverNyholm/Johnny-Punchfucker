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
    class Level1 : Level
    {
        private bool spawn2, spawn3;
        private int waveNr;
        public Level1(ContentManager Content, PlayerManager playerManager, EnemyManager enemyManager) : base(Content, playerManager, enemyManager)
        {
            contentLoader = new ContentLoader(Content, @"Content/Levels/lvl1environment.txt", @"Content/Levels/lvl1items.txt");
            SpawnEnemy1(enemyManager.enemyList);
            nextLevelPosX = 1200;
        }

        public void Update(GameTime gameTime)
        {
            nextLevelBox = new Rectangle(7500, (int)502, 40, 300); //tar man i denna går man över till level 2
            contentLoader.Update(gameTime);
            CameraStopWhenEnemySpawn(playerManager, gameTime);

            //Kollar spelarens positioner innan den spawnar nya snubbar
            for (int j = 0; j < playerManager.playerList.Count; j++)
            {
                if (playerManager.playerList[j].pos.X > 3000 && !spawn2) // Kollar om spelaren gått till positionen och spawnar fiender och gör bool true så att det bara spawnas en gång
                    SpawnEnemy2(enemyManager.enemyList);

                if (playerManager.playerList[j].pos.X > 4500 && !spawn3)
                    SpawnEnemy3(enemyManager.enemyList);
            }


            //Om spelaren går i mål så kommer man att ha klarat av level 1 och level 2 ska börja
            for (int i = 0; i < playerManager.playerList.Count; i++) 
                if (playerManager.playerList[i].boundingBox.Intersects(nextLevelBox))
                {
                    contentLoader.NextLevel(playerManager, enemyManager, 1200);
                    GameManager.levelNr++;
                }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            contentLoader.Draw(spriteBatch);
            spriteBatch.Draw(TextureManager.lifeBarTex, nextLevelBox, Color.Black);
        }

        private void SpawnEnemy1(List<Enemy> enemyList)
        {
            enemyList.Add(new StandardEnemy(TextureManager.standardEnemyTex, new Vector2(675, 750), false, 2));
            enemyList.Add(new StandardEnemy(TextureManager.standardEnemyTex, new Vector2(600, 450), false, 2));
            enemyList.Add(new StandardEnemy(TextureManager.standardEnemyTex, new Vector2(750, 600), false,2 ));
            enemyList.Add(new StandardEnemy(TextureManager.standardEnemyTex, new Vector2(3000, 675), false,2 ));
        }

        private void SpawnEnemy2(List<Enemy> enemyList)
        {
            enemyList.Add(new StandardEnemy(TextureManager.standardEnemyTex, new Vector2(3000, 1200), true, 2));
            enemyList.Add(new StandardEnemy(TextureManager.standardEnemyTex, new Vector2(3975, 1012), true, 2));
            spawn2 = true;
            if (PlayerManager.players == 1 && AudioManager.sound)
                AudioManager.Johnny_screamForMe.Play();
            else if (PlayerManager.players == 2 && AudioManager.sound && !playerManager.playerList[1].dead)
                AudioManager.Tommy_LickMyPeePee.Play();
        }

        private void SpawnEnemy3(List<Enemy> enemyList)
        {
            enemyList.Add(new StandardEnemy(TextureManager.standardEnemyTex, new Vector2(4950, 1200), true, 2));
            enemyList.Add(new StandardEnemy(TextureManager.standardEnemyTex, new Vector2(3600, 675), true, 2));
            enemyList.Add(new StandardEnemy(TextureManager.standardEnemyTex, new Vector2(4800, 450), true, 2));
            enemyList.Add(new StandardEnemy(TextureManager.standardEnemyTex, new Vector2(6325, 825), true, 2));
            enemyList.Add(new StandardEnemy(TextureManager.standardEnemyTex, new Vector2(4200, 1200), true, 2));
            enemyList.Add(new StandardEnemy(TextureManager.standardEnemyTex, new Vector2(4500, 1200), true, 2));
            spawn3 = true;
            if (!playerManager.playerList[0].dead && AudioManager.sound)
                AudioManager.Johnny_OhMommy.Play();
        }

        public void CameraStopWhenEnemySpawn(PlayerManager playerManager, GameTime gameTime)
        {
            if (!spawn2 && !spawn3)
            {
                ContentLoader.levelEndPosX = 2100;
            }

            if (!spawn2 && !spawn3 && enemyManager.enemyList.Count <= 1)
            {
                ContentLoader.levelEndPosX = 3675;
                Camera.smooth = true;
                if (waveNr == 1)
                {
                    if (AudioManager.sound)
                        AudioManager.Johnny_ICouldDoItAllNight.Play();
                    waveNr++;
                }
            }
            if (spawn2 && !spawn3 && enemyManager.enemyList.Count <= 0)
            {
                ContentLoader.levelEndPosX = 5700;
                Camera.smooth = true;
                if (waveNr == 2)
                {
                    if (PlayerManager.players == 1 && AudioManager.sound)
                        AudioManager.Johnny_FasterThenTheSpeedOfFight.Play();
                    else if (PlayerManager.players == 2 && AudioManager.sound && !playerManager.playerList[1].dead)
                        AudioManager.Tommy_ImGonnaBeAnAngel.Play();
                    waveNr++;
                }
            }
            if (spawn2 && spawn3 && enemyManager.enemyList.Count <= 0)
            {
                ContentLoader.levelEndPosX = 7500;
                Camera.smooth = true;
                if (waveNr == 3)
                {
                    if (PlayerManager.players == 1 && AudioManager.sound)
                        AudioManager.Johnny_NoTimeForButt.Play();
                    else if (PlayerManager.players == 2 && AudioManager.sound && !playerManager.playerList[1].dead)
                        AudioManager.Tommy_Toodiloo.Play();
                    waveNr++;
                }
            }
        }
    }
}
