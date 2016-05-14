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
    class GameManager
    {
        SoundEffectInstance LevelMusic, MenuMusic;
        public Menu menu;
        public PlayerManager playerManager;
        EnemyManager enemyManager;
        float musicVolume = 0.2f;
        Level1 level1;
        Level2 level2;
        Level3 level3;
        Level4 level4;
        public static int levelNr = 1;
        public int firstDigitSeconds, secondDigitSeconds, firstDigitMinutes, secondDigitMinutes, firstDigitHours, secondDigitHours;
        public double time, digitSeconds;
        public int intro = 0; // vilket intro det är
        public static bool levelInitialized, completed, failed, hardcore;


        public TimeSpan introSwitch;

        public enum GameState
        {
            Intro, Menu, Play, Pause, Died, Won, ReloadLevel
        }
        public GameState gameState;

        public void LoadContent(ContentManager Content, GraphicsDeviceArcade GraphicsDevice, SpriteBatch spriteBatch)
        {
            AudioManager.LoadContent(Content);

            LevelMusic = AudioManager.Level.CreateInstance();
            MenuMusic = AudioManager.MenuMusic.CreateInstance();

            TextureManager.LoadContent(Content);
            menu = new Menu(Content);
            playerManager = new PlayerManager();
            enemyManager = new EnemyManager(GraphicsDevice);
            InitializeLevels(Content);
            gameState = GameState.Menu;
        }

        public void Update(GameTime gameTime, GraphicsDeviceArcade GraphicsDevice, ContentManager Content)
        {
            Console.WriteLine(introSwitch);
            switch (gameState)
            {
                case GameState.Intro:
                    LevelMusic.IsLooped = true;

                    MenuMusic.IsLooped = true;
                    if (intro == 0)
                        MediaPlayer.Play(AudioManager.intro);

                    if (introSwitch.TotalSeconds > 0)
                        introSwitch = introSwitch.Subtract(gameTime.ElapsedGameTime);
                    else
                    {
                        if (intro == 0)
                            introSwitch = TimeSpan.FromSeconds(15); // sköter om hur många sekunder varje intro ska vara
                        if (intro == 1)
                            introSwitch = TimeSpan.FromSeconds(21);
                        if (intro == 2)
                            introSwitch = TimeSpan.FromSeconds(16);
                        intro++;
                    }

                    if (intro == 4)
                    {
                        gameState = GameState.Menu;
                    }

                    if (InputHandler.IsButtonDown(PlayerIndex.One, PlayerInput.Blue, true) && InputHandler.IsButtonUp(PlayerIndex.One, PlayerInput.Blue, false))
                    {
                        gameState = GameState.Menu;
                    }
                    break;

                case GameState.Menu:

                    MediaPlayer.Stop();

                    if (!AudioManager.sound)
                    {
                        MenuMusic.Volume = musicVolume - musicVolume;
                        LevelMusic.Volume = musicVolume - musicVolume;
                    }
                    else
                        MenuMusic.Volume = musicVolume;
                    MenuMusic.Play();
                    LevelMusic.Stop();

                    menu.Update(gameTime);

                    if (menu.play == true)
                    {

                        LevelMusic.Play();

                        gameState = GameState.Play;
                        levelNr = 1;
                        enemyManager = new EnemyManager(GraphicsDevice); //allt under resettar och laddar in allt på nytt ifall man valt quit i pausmenyn
                        playerManager = new PlayerManager();
                        InitializeLevels(Content);
                        ResetTime();
                        Game1.ready = false;
                    }
                    break;

                case GameState.Play:
                    MenuMusic.Stop();
                    LevelMusic.IsLooped = true;
                    if (Game1.ready && (InputHandler.IsButtonDown(PlayerIndex.One, PlayerInput.Start, true) && InputHandler.IsButtonUp(PlayerIndex.One, PlayerInput.Start, false)))
                    {
                        gameState = GameState.Pause;
                        menu.play = false;
                        menu.menuState = Menu.MenuState.Pause;
                    }

                    if (!levelInitialized)
                        InitializeLevels(Content);
                    UpdateLevels(gameTime);
                    //level1.Update(gameTime);
                    //contentLoader.Update(gameTime);
                    //contentLoader.NextLevel(playerManager, enemyManager);

                    playerManager.Update(gameTime);
                    playerManager.LandingPunches(enemyManager);
                    if (levelInitialized)
                    {
                        playerManager.CollectItems(GetLevel().contentLoader);
                        playerManager.PunchBarrel(GetLevel().contentLoader);

                    }

                    enemyManager.Update(gameTime);
                    enemyManager.AggroPlayer(playerManager, gameTime);
                    enemyManager.FightPlayer(playerManager);
                    enemyManager.KeepDistance(gameTime);
                    enemyManager.IsBlocked(playerManager, gameTime);
                    enemyManager.BossDamage(playerManager);

                    TotalPlayTime(gameTime);

                    //if (keyBoardState.IsKeyDown(Keys.NumPad8))
                    //{
                    //    musicVolume += 0.02f;
                    //    if (musicVolume > 1)
                    //        musicVolume = 1;                                                          FIX IN MENU
                    //}
                    //else if (keyBoardState.IsKeyDown(Keys.NumPad2))
                    //{
                    //    musicVolume -= 0.02f;
                    //    if (musicVolume < 0)
                    //        musicVolume = 0;
                    //}

                    if (!AudioManager.sound)
                    {
                        LevelMusic.Volume = musicVolume - musicVolume;
                    }
                    else
                        LevelMusic.Volume = musicVolume;


                    time += gameTime.ElapsedGameTime.TotalSeconds;

                    if (GameManager.failed)
                        gameState = GameState.Died;
                    if (GameManager.completed)
                        gameState = GameState.Won;
                    break;

                case GameState.Pause:
                    menu.Update(gameTime);

                    if (!AudioManager.sound)
                    {
                        LevelMusic.Volume = musicVolume - musicVolume;
                    }
                    else
                        LevelMusic.Volume = musicVolume;

                    if (menu.play == true)
                        gameState = GameState.Play;

                    if (menu.menuState == Menu.MenuState.MainMenu)
                    {
                        gameState = GameState.Menu;
                        RestartCamera();
                    }
                    break;

                case GameState.Died:

                    menu.play = false;
                    LevelMusic.Stop();
                    if (InputHandler.IsButtonDown(PlayerIndex.One, PlayerInput.Red, true) && InputHandler.IsButtonUp(PlayerIndex.One, PlayerInput.Blue, false))
                    {
                        GameManager.failed = false;
                        gameState = GameState.ReloadLevel;
                    }
                    break;

                case GameState.Won:

                    menu.play = false;
                    LevelMusic.Stop();
                    if (InputHandler.IsButtonDown(PlayerIndex.One, PlayerInput.Red, true) && InputHandler.IsButtonUp(PlayerIndex.One, PlayerInput.Blue, false))
                    {
                        GameManager.failed = false;
                        ClearLists();
                        gameState = GameState.Menu;
                        menu.menuState = Menu.MenuState.MainMenu;
                        RestartCamera();
                    }
                    break;

                case GameState.ReloadLevel:
                    ReloadLevel(Content);
                    RestartCamera();
                    playerManager.playerList[0].life = 10;
                    if (PlayerManager.players == 2) //Om det är två spelare, gör samma sak för spelare 2)
                        playerManager.playerList[1].life = 10;
                    gameState = GameState.Play;
                    break;
            }
        }

        public void DrawStats(SpriteBatch spriteBatch)
        {

            for (int i = 0; i < playerManager.playerList.Count; i++)// Lifebars till spelarna
            {
                if (playerManager.playerList[0].percentLife < 1.0f)
                {
                    spriteBatch.Draw(TextureManager.lifeBarTex, new Rectangle(180 /*- 50 * (PlayerManager.players - 1), 608*/, 916 - ((PlayerManager.players - 1) * 63), 232, 52), Color.Red);
                }
                spriteBatch.Draw(TextureManager.lifeBarTex, new Rectangle(180, 916 - ((PlayerManager.players - 1) * 63), (int)(232 * playerManager.playerList[0].percentLife), 52), Color.Green);

                if (PlayerManager.players == 2)
                {
                    if (playerManager.playerList[1].percentLife < 1.0f)
                    {
                        spriteBatch.Draw(TextureManager.lifeBarTex, new Rectangle(381, 930, 232, 52), Color.Red);
                    }
                    float healthPos = (float)(playerManager.playerList[1].maxLife - playerManager.playerList[1].life);
                    spriteBatch.Draw(TextureManager.lifeBarTex, new Rectangle((int)(381 + (23.25f * healthPos)), 930, (int)(232 * playerManager.playerList[1].percentLife), 52), Color.Green);
                    Console.WriteLine(healthPos);
                }
            }
            if (PlayerManager.players == 1)
            {
                spriteBatch.Draw(TextureManager.statusBarPlayerOneTex, Vector2.Zero, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);

            }
            if (PlayerManager.players == 2)
            {
                spriteBatch.Draw(TextureManager.statusBarPlayerTwoTex, Vector2.Zero, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);

            }

            switch (gameState)
            {
                case GameState.Pause:

                    menu.Draw(spriteBatch);

                    break;
            }

            spriteBatch.DrawString(TextureManager.timeFont, secondDigitHours.ToString() + firstDigitHours.ToString() +
            ":" + secondDigitMinutes.ToString() + firstDigitMinutes.ToString() +
            ":" + secondDigitSeconds.ToString() + firstDigitSeconds.ToString(), new Vector2(800, 920), Color.Green);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            switch (gameState)
            {
                case GameState.Intro:

                    if (intro == 1)                                                           // vilket intro som ska visas
                        spriteBatch.Draw(TextureManager.introScreen1, Vector2.Zero, Color.White);
                    if (intro == 2)
                        spriteBatch.Draw(TextureManager.introScreen2, Vector2.Zero, Color.White);
                    if (intro == 3)
                        spriteBatch.Draw(TextureManager.introScreen3, Vector2.Zero, Color.White);
                    if (intro == 4)
                        spriteBatch.Draw(TextureManager.introScreen4, Vector2.Zero, Color.Pink);

                    break;


                case GameState.Menu:
                    menu.Draw(spriteBatch);
                    break;

                case GameState.Play:

                    DrawLevels(spriteBatch);
                    enemyManager.Draw(spriteBatch);
                    playerManager.Draw(spriteBatch);

                    break;

                case GameState.Pause:
                    DrawLevels(spriteBatch);
                    enemyManager.Draw(spriteBatch);
                    playerManager.Draw(spriteBatch);

                    break;

                case GameState.Died:
                    spriteBatch.Draw(TextureManager.gameOverScreenTex, Vector2.Zero, Color.White);

                    break;

                case GameState.Won:
                    spriteBatch.Draw(TextureManager.endScreenTex, Vector2.Zero, Color.White);

                    break;
            }
        }

        private void InitializeLevels(ContentManager Content)
        {
            if (levelNr == 1)
                level1 = new Level1(Content, playerManager, enemyManager);
            if (levelNr == 2)
                level2 = new Level2(Content, playerManager, enemyManager);
            if (levelNr == 3)
                level3 = new Level3(Content, playerManager, enemyManager);
            if (levelNr == 4)
                level4 = new Level4(Content, playerManager, enemyManager);

            levelInitialized = true;
        }

        private void UpdateLevels(GameTime gameTime)
        {
            if (levelNr == 1)
                level1.Update(gameTime);
            if (levelNr == 2 && levelInitialized)
                level2.Update(gameTime);
            if (levelNr == 3 && levelInitialized)
                level3.Update(gameTime);
            if (levelNr == 4 && levelInitialized)
                level4.Update(gameTime);
        }

        private void DrawLevels(SpriteBatch spriteBatch)
        {
            if (levelNr == 1)
                level1.Draw(spriteBatch);
            if (levelNr == 2 && levelInitialized)
                level2.Draw(spriteBatch);
            if (levelNr == 3 && levelInitialized)
                level3.Draw(spriteBatch);
            if (levelNr == 4 && levelInitialized)
                level4.Draw(spriteBatch);
        }

        private Level GetLevel()
        {
            if (levelNr == 1)
                return level1;

            if (levelNr == 2)
                return level2;

            if (levelNr == 3)
                return level3;

            if (levelNr == 4)
                return level4;

            return null;
        }

        private void ReloadLevel(ContentManager Content)
        {
            if(hardcore)
            {
                levelNr = 1;
                ResetTime();
            }
            InitializeLevels(Content);
            Level levelX = GetLevel();
            levelX.contentLoader.NextLevel(playerManager, enemyManager, levelX.nextLevelPosX);

        }

        private void TotalPlayTime(GameTime gameTime)
        {
            #region DigitTimer
            digitSeconds += gameTime.ElapsedGameTime.TotalSeconds;

            firstDigitSeconds = (int)digitSeconds;
            if (firstDigitSeconds >= 10)
            {
                secondDigitSeconds++;
                firstDigitSeconds = 0;
                digitSeconds = 0;
            }
            if (secondDigitSeconds >= 6)
            {
                firstDigitMinutes++;
                secondDigitSeconds = 0;
            }

            if (firstDigitMinutes >= 10)
            {
                secondDigitMinutes++;
                firstDigitMinutes = 0;
            }
            if (secondDigitMinutes >= 6)
            {
                firstDigitHours++;
                secondDigitMinutes = 0;
            }
            if (firstDigitHours >= 10)
            {
                secondDigitHours++;
                firstDigitHours = 0;
            }
            #endregion
        }

        private void ResetTime()
        {
            firstDigitSeconds = 0;
            secondDigitSeconds = 0;
            firstDigitMinutes = 0;
            secondDigitMinutes = 0;
            firstDigitHours = 0;
            secondDigitHours = 0;

            digitSeconds = 0;
        }

        public void RestartCamera()
        {
            Camera.centre = new Vector2(34, 0);
            Camera.prevCentre = Camera.centre;
            Camera.transform = Matrix.CreateScale(new Vector3(1, 1, 0))
    * Matrix.CreateTranslation(new Vector3(-Camera.centre.X, -Camera.centre.Y, 0));
        }

        private void ClearLists()
        {
            playerManager.playerList.Clear();
            enemyManager.enemyList.Clear();
            enemyManager.bossAttackList.Clear();

        }
    }
}
