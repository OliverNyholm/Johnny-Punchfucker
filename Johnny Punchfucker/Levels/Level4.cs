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

        public Level4(ContentManager Content, PlayerManager playerManager, EnemyManager enemyManager)
            : base(Content, playerManager, enemyManager)
        {
            contentLoader = new ContentLoader(Content, @"Content/Levels/lvl4environment.txt", @"Content/Levels/lvl4items.txt");
        }

        public void Update(GameTime gameTime)
        {
            if (Susan.dead)
                nextLevelBox = new Rectangle(2537, (int)502, 40, 300); //tar man i denna vinner man spelet

            contentLoader.Update(gameTime);
            CameraStopWhenEnemySpawn(playerManager, gameTime);

            for (int i = 0; i < playerManager.playerList.Count; i++)
                if (playerManager.playerList[i].pos.X > 1800 && !bossSpawned)
                    SpawnBoss();

            //spawns enemies if Susan is in phase 4
            if (!Susan.enemySpawned && enemyManager.enemyList.Count > 0)
                SpawnEnemies(enemyManager.enemyList);

            //Om spelaren går i mål så kommer man att ha klarat av spelet
            for (int i = 0; i < playerManager.playerList.Count; i++)
                if (playerManager.playerList[i].boundingBox.Intersects(nextLevelBox))
                {
                    GameManager.completed = true;
                }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            contentLoader.Draw(spriteBatch);
            spriteBatch.Draw(TextureManager.lifeBarTex, nextLevelBox, Color.Black);
        }

        private void SpawnBoss()
        {
            enemyManager.enemyList.Add(new Susan(TextureManager.susan, new Vector2(2300, -100), false, 1, playerManager.playerList, enemyManager));
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
