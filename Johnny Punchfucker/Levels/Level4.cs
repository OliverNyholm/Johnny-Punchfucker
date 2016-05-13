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
    class Level4 : Level
    {
        bool bossSpawned;
        int waveNr;

        public Level4(ContentManager Content, PlayerManager playerManager, EnemyManager enemyManager)
            : base(Content, playerManager, enemyManager)
        {
            contentLoader = new ContentLoader(Content, @"Content/Levels/lvl4environment.txt", @"Content/Levels/lvl1items.txt");
        }

        public void Update(GameTime gameTime)
        {
            //nextLevelBox = new Rectangle(2537, (int)502, 40, 300); //tar man i denna går man över till level 2
            contentLoader.Update(gameTime);
            CameraStopWhenEnemySpawn(playerManager, gameTime);

            for (int i = 0; i < playerManager.playerList.Count; i++)
                if (playerManager.playerList[i].pos.X > 1800 && !bossSpawned)
                    SpawnBoss();

            //spawns enemies if Susan is in phase 4
            if (!Susan.enemySpawned && enemyManager.enemyList.Count > 0)
                SpawnEnemies(enemyManager.enemyList);

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

        private void SpawnBoss()
        {
            enemyManager.enemyList.Add(new Susan(TextureManager.susan, new Vector2(2300, -100), false, 4, playerManager.playerList, enemyManager));
            bossSpawned = true;
        }

        private void SpawnEnemies(List<Enemy> enemyList)
        {
            enemyList.Add(new StandardEnemy(TextureManager.standardEnemyTex, new Vector2(600, 675), true, 4));
            enemyList.Add(new StandardEnemy(TextureManager.standardEnemyTex, new Vector2(2800, 675), true, 4));
            Susan.enemySpawned = true;
        }

        public void CameraStopWhenEnemySpawn(PlayerManager playerManager, GameTime gameTime)
        {
            ContentLoader.levelEndPosX = 2525;
        }
    }
}
