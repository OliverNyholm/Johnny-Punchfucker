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
    class ContentLoader
    {
        StreamReader streamReaderEnvironment, streamReaderItems;
        List<string> strings = new List<string>();
        List<Environment> enviromentList = new List<Environment>();
        public List<Item> itemList = new List<Item>();
        public static int levelEndPosX;
        public static bool end;
        public List<Barrel> barrels = new List<Barrel>();

        public ContentLoader(ContentManager Content, string environemntTxt, string itemTxt)
        {
            streamReaderEnvironment = new StreamReader(environemntTxt);
            streamReaderItems = new StreamReader(itemTxt);
            MapReader();
            ItemReader();
        }

        public void Update(GameTime gameTime)
        {
            foreach (Item item in itemList)
            {
                item.Update(gameTime);
            }
            foreach(Barrel barrel in barrels)
            {
                barrel.Update(gameTime);
            }
            BarrelRemove();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Environment enviroment in enviromentList)
            {
                enviroment.Draw(spriteBatch);
            }
            foreach (Item item in itemList)
            {
                item.Draw(spriteBatch);
            }
            foreach(Barrel barrel in barrels)
            {
                barrel.Draw(spriteBatch);
            }
        }

        public void MapReader()
        {
            while (!streamReaderEnvironment.EndOfStream)
            {
                strings.Add(streamReaderEnvironment.ReadLine());
            }
            streamReaderEnvironment.Close();

            for (int i = 0; i < strings.Count; i++)
            {
                for (int j = 0; j < strings[i].Length; j++)
                {
                    if (strings[i][j] == 'c')
                    {
                        enviromentList.Add(new BackgroundImage(TextureManager.beachbackgroundTex, new Vector2(j * 136, i * 123)));
                    }
                    if (strings[i][j] == 'd')
                    {
                        enviromentList.Add(new BackgroundImage(TextureManager.beachback2groundTex, new Vector2(j * 136, i * 123)));
                    }
                    if (strings[i][j] == 'f')
                    {
                        enviromentList.Add(new BackgroundImage(TextureManager.beachback3groundTex, new Vector2(j * 136, i * 123)));
                    }
                    if (strings[i][j] == 'v')
                    {
                        enviromentList.Add(new BackgroundImage(TextureManager.jungleBackground1Tex, new Vector2(j * 136, i * 123)));
                    }
                    if (strings[i][j] == 'h')
                    {
                        enviromentList.Add(new BackgroundImage(TextureManager.jungleBackground2Tex, new Vector2(j * 136, i * 123)));
                    }
                    if (strings[i][j] == 'q')
                    {
                        enviromentList.Add(new BackgroundImage(TextureManager.bridgebackgroundTex, new Vector2(j * 136, i * 123)));
                    }
                    if (strings[i][j] == 'z')
                    {
                        enviromentList.Add(new BackgroundImage(TextureManager.waterbackgroundTex, new Vector2(j * 136, i * 123)));
                    }
                    if (strings[i][j] == 'r')
                    {
                        enviromentList.Add(new Road(TextureManager.roadTex, new Vector2(j * 123, 15 + i * 123)));
                    }
                    if (strings[i][j] == 'j')
                    {
                        enviromentList.Add(new Road(TextureManager.jungleRoadTex, new Vector2(j * 123, 15 + i * 123)));
                    }
                    if (strings[i][j] == 'k')
                    {
                        enviromentList.Add(new Road(TextureManager.jungleRoadThornTex, new Vector2(j * 123, 15 + i * 123)));
                    }
                    if (strings[i][j] == 't')
                    {
                        enviromentList.Add(new Road(TextureManager.templeRoadTex, new Vector2(j * 123, 15 + i * 123)));
                    }
                    if (strings[i][j] == 'm')
                    {
                        enviromentList.Add(new SmallPlant(TextureManager.smallPlantTex, new Vector2(j * 123, 15 + i * 123)));
                    }
                }
            }
            strings.Clear();
        }

        public void ItemReader()
        {
            while (!streamReaderItems.EndOfStream)
            {
                strings.Add(streamReaderItems.ReadLine());
            }
            streamReaderItems.Close();

            for (int i = 0; i < strings.Count; i++)
            {
                for (int j = 0; j < strings[i].Length; j++)
                {
                    if (strings[i][j] == 'w')
                    {
                        itemList.Add(new Cake(TextureManager.cake, new Vector2(j * 136, i * 123)));
                    }
                    if (strings[i][j] == 'p')
                    {
                        itemList.Add(new PinaColada(TextureManager.pinacolada, new Vector2(j * 136, i * 123)));
                    }
                    if (strings[i][j] == 'J') //konstigt att trädet är i items, men yolo
                    {
                        enviromentList.Add(new JungleTree(TextureManager.jungleEntranceTex, new Vector2(j * 123, 15 + i * 123)));
                    }
                    if (strings[i][j] == 'b') 
                    {                  
                        barrels.Add(new Barrel(TextureManager.barrel, new Vector2(j * 136, i * 110)));
                    }

                }
            }
            strings.Clear();
        }
        public void BarrelRemove()
        {
            for (int i = 0; i < barrels.Count; i++)
            {
                if(barrels[i].IsDead)
                {
                    barrels.RemoveAt(i);
                }
            }
        }
        public void NextLevel(PlayerManager playerManager, EnemyManager enemyManager, int newStartPosX)
        {
            for (int i = 0; i < playerManager.playerList.Count; i++)
            {

                if (playerManager.playerList[0].life <= 9)
                    playerManager.playerList[0].life++;
                if (PlayerManager.players == 2)
                    if (playerManager.playerList[1].life <= 9)
                        playerManager.playerList[1].life++;

                playerManager.playerList[0].pos = new Vector2(newStartPosX, 600);
                playerManager.playerList[0].dead = false;
                playerManager.playerList[0].animationBox.Height = 174;
                if (PlayerManager.players == 2)
                {
                    playerManager.playerList[1].pos = new Vector2(newStartPosX, 700);
                    playerManager.playerList[1].dead = false;
                    playerManager.playerList[1].animationBox.Height = 174;
                }



                Camera.prevCentre.X = 0; //resettar så att kameran hamnar på spelar 1 igen
                enemyManager.enemyList.Clear(); //raderar alla gamla fiender
                enemyManager.bossAttackList.Clear();
                enviromentList.Clear();
                itemList.Clear();
                barrels.Clear();
                Game1.ready = false; //fet loadingscreen
                GameManager.levelInitialized = false;
            }
        }
    }
}
