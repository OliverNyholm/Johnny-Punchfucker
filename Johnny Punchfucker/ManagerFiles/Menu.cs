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
    class Menu
    {
        int menuNumber = 1;
        public bool play, quit, howTo;
        public double spaceTimer;

        public enum MenuState
        {
            MainMenu, NewGame, Difficulty, Options, HowToPlay, Pause, PauseOptions, PauseQuit
        }
        public MenuState menuState;
        public Menu(ContentManager Content)
        {
            TextureManager.LoadContent(Content);

            menuState = MenuState.MainMenu;
        }

        public void Update(GameTime gameTime)
        {

            spaceTimer += gameTime.ElapsedGameTime.Milliseconds;
            #region Key Up
            if (InputHandler.IsButtonDown(PlayerIndex.One, PlayerInput.Up, true) && InputHandler.IsButtonUp(PlayerIndex.One, PlayerInput.Up, false)) // väljer vilken "knapp" man vill till i menyn
            {
                if (menuState != MenuState.HowToPlay)
                    AudioManager.MenuMove.Play();

                menuNumber--;
                if (menuNumber == 0 && menuState == MenuState.Pause) // om man trycker upp vid toppen går man till botten
                {
                    menuNumber = 3;
                }
                if (menuNumber == 0 && (menuState == MenuState.NewGame || menuState == MenuState.MainMenu || menuState == MenuState.Difficulty || menuState == MenuState.PauseQuit))
                {
                    menuNumber = 2;
                }
                if (menuNumber == 0 && menuState == MenuState.Options || menuNumber == 0 && menuState == MenuState.PauseOptions)
                {
                    menuNumber = 2;
                }
            }
            #endregion
            #region Key Down
            if (InputHandler.IsButtonDown(PlayerIndex.One, PlayerInput.Down, true) && InputHandler.IsButtonUp(PlayerIndex.One, PlayerInput.Down, false))
            {
                if (menuState != MenuState.HowToPlay)
                    AudioManager.MenuMove.Play();

                menuNumber++;
                if (menuNumber == 4 && menuState == MenuState.Pause)
                {
                    menuNumber = 1;
                }
                if (menuNumber == 3 && (menuState == MenuState.NewGame || menuState == MenuState.MainMenu || menuState == MenuState.Difficulty || menuState == MenuState.PauseQuit))
                {
                    menuNumber = 1;
                }
                if (menuNumber == 3 && menuState == MenuState.Options || menuNumber == 3 && menuState == MenuState.PauseOptions)
                {
                    menuNumber = 1;
                }

            }
            #endregion

            #region menuStates
            switch (menuState)
            {
                #region Main Menu
                case MenuState.MainMenu:
                    if (menuNumber == 1 && (InputHandler.IsButtonDown(PlayerIndex.One, PlayerInput.Red, true) && InputHandler.IsButtonUp(PlayerIndex.One, PlayerInput.Red, false))) // om man är på 1 som är markerad röd går man dit
                    {
                        menuState = MenuState.NewGame;
                        menuNumber = 1;
                    }
                    if (menuNumber == 2 && (InputHandler.IsButtonDown(PlayerIndex.One, PlayerInput.Red, true) && InputHandler.IsButtonUp(PlayerIndex.One, PlayerInput.Red, false)))
                    {
                        menuState = MenuState.Options;
                        menuNumber = 1;
                    }
                    if (menuNumber == 3 && (InputHandler.IsButtonDown(PlayerIndex.One, PlayerInput.Red, true) && InputHandler.IsButtonUp(PlayerIndex.One, PlayerInput.Red, false)))
                    {
                        quit = true;
                    }
                    break;
                #endregion
                #region NewGame
                case MenuState.NewGame:
                    if (menuNumber == 1 && (InputHandler.IsButtonDown(PlayerIndex.One, PlayerInput.Red, true) && InputHandler.IsButtonUp(PlayerIndex.One, PlayerInput.Red, false)))
                    {
                        PlayerManager.players = 1;
                        menuState = MenuState.Difficulty;
                        menuNumber = 1;
                    }
                    if (menuNumber == 2 && (InputHandler.IsButtonDown(PlayerIndex.One, PlayerInput.Red, true) && InputHandler.IsButtonUp(PlayerIndex.One, PlayerInput.Red, false)))
                    {
                        PlayerManager.players = 2;
                        menuState = MenuState.Difficulty;
                        menuNumber = 1;

                    }
                    if (InputHandler.IsButtonDown(PlayerIndex.One, PlayerInput.Blue, true) && InputHandler.IsButtonUp(PlayerIndex.One, PlayerInput.Blue, false))
                    {
                        AudioManager.MenuBack.Play();

                        menuState = MenuState.MainMenu;
                        menuNumber = 1;
                    }
                    break;
                #endregion
                #region Difficilty
                case MenuState.Difficulty:
                    if (menuNumber == 1 && (InputHandler.IsButtonDown(PlayerIndex.One, PlayerInput.Red, true) && InputHandler.IsButtonUp(PlayerIndex.One, PlayerInput.Red, false)))
                    {
                        GameManager.hardcore = false;
                        play = true;
                    }
                    if (menuNumber == 2 && (InputHandler.IsButtonDown(PlayerIndex.One, PlayerInput.Red, true) && InputHandler.IsButtonUp(PlayerIndex.One, PlayerInput.Red, false)))
                    {
                        GameManager.hardcore = true;
                        play = true;
                    }
                    if (InputHandler.IsButtonDown(PlayerIndex.One, PlayerInput.Blue, true) && InputHandler.IsButtonUp(PlayerIndex.One, PlayerInput.Blue, false))
                    {
                        AudioManager.MenuBack.Play();

                        menuState = MenuState.NewGame;
                        menuNumber = 1;
                    }
                    break;
                #endregion
                #region Options
                case MenuState.Options:
                    if (menuNumber == 1 && ((InputHandler.IsButtonDown(PlayerIndex.One, PlayerInput.Red, true) && InputHandler.IsButtonUp(PlayerIndex.One, PlayerInput.Red, false))) && AudioManager.sound)
                    {
                        AudioManager.sound = false;
                    }
                    else if (menuNumber == 1 && ((InputHandler.IsButtonDown(PlayerIndex.One, PlayerInput.Red, true) && InputHandler.IsButtonUp(PlayerIndex.One, PlayerInput.Red, false))) && !AudioManager.sound)
                    {
                        AudioManager.sound = true;
                    }
                    if (menuNumber == 2 && (InputHandler.IsButtonDown(PlayerIndex.One, PlayerInput.Red, true) && InputHandler.IsButtonUp(PlayerIndex.One, PlayerInput.Red, false)))
                    {
                        menuState = MenuState.HowToPlay;
                    }
                    if (InputHandler.IsButtonDown(PlayerIndex.One, PlayerInput.Blue, true) && InputHandler.IsButtonUp(PlayerIndex.One, PlayerInput.Blue, false))
                    {
                        AudioManager.MenuBack.Play();

                        menuState = MenuState.MainMenu;
                        menuNumber = 1;
                    }

                    break;
                #endregion
                #region How To Play
                case MenuState.HowToPlay:
                    if (InputHandler.IsButtonDown(PlayerIndex.One, PlayerInput.Blue, true) && InputHandler.IsButtonUp(PlayerIndex.One, PlayerInput.Blue, false))
                    {
                        AudioManager.MenuBack.Play();

                        menuState = MenuState.Options;
                        menuNumber = 2;
                    }

                    break;
                #endregion
                #region Pause
                case MenuState.Pause:
                    if (menuNumber == 1 && (InputHandler.IsButtonDown(PlayerIndex.One, PlayerInput.Red, true) && InputHandler.IsButtonUp(PlayerIndex.One, PlayerInput.Red, false)))
                    {
                        play = true;
                    }
                    if (menuNumber == 2 && (InputHandler.IsButtonDown(PlayerIndex.One, PlayerInput.Red, true) && InputHandler.IsButtonUp(PlayerIndex.One, PlayerInput.Red, false)))
                    {
                        menuState = MenuState.PauseOptions;
                        menuNumber = 1;
                    }
                    if (menuNumber == 3 && (InputHandler.IsButtonDown(PlayerIndex.One, PlayerInput.Red, true) && InputHandler.IsButtonUp(PlayerIndex.One, PlayerInput.Red, false)))
                    {
                        menuNumber = 1;
                        menuState = MenuState.PauseQuit;
                    }
                    break;
                #endregion
                #region Pause Options
                case MenuState.PauseOptions:
                    if (menuNumber == 1 && ((InputHandler.IsButtonDown(PlayerIndex.One, PlayerInput.Red, true) && InputHandler.IsButtonUp(PlayerIndex.One, PlayerInput.Red, false))) && AudioManager.sound)
                    {
                        AudioManager.sound = false;
                    }
                    else if (menuNumber == 1 && ((InputHandler.IsButtonDown(PlayerIndex.One, PlayerInput.Red, true) && InputHandler.IsButtonUp(PlayerIndex.One, PlayerInput.Red, false))) && !AudioManager.sound)
                    {
                        AudioManager.sound = true;
                        menuNumber = 1;
                    }
                    if ((InputHandler.IsButtonDown(PlayerIndex.One, PlayerInput.Blue, true) && InputHandler.IsButtonUp(PlayerIndex.One, PlayerInput.Blue, false)) && !howTo)
                    {
                        AudioManager.MenuBack.Play();
                        menuState = MenuState.Pause;
                        menuNumber = 2;
                    }

                    if (menuNumber == 2 && ((InputHandler.IsButtonDown(PlayerIndex.One, PlayerInput.Red, true) && InputHandler.IsButtonUp(PlayerIndex.One, PlayerInput.Red, false))) && !howTo)
                    {
                        howTo = true;
                    }
                    if ((InputHandler.IsButtonDown(PlayerIndex.One, PlayerInput.Blue, true) && InputHandler.IsButtonUp(PlayerIndex.One, PlayerInput.Blue, false)) && howTo)
                    {
                        AudioManager.MenuBack.Play();

                        howTo = false;
                        menuState = MenuState.PauseOptions;
                        menuNumber = 2;
                    }

                    break;
                #endregion
                #region Pause Quit
                case MenuState.PauseQuit:
                    if (menuNumber == 2 && (InputHandler.IsButtonDown(PlayerIndex.One, PlayerInput.Red, true) && InputHandler.IsButtonUp(PlayerIndex.One, PlayerInput.Red, false)))
                    {
                        menuState = MenuState.MainMenu;
                        menuNumber = 0;
                    }
                    else if (menuNumber == 1 && (InputHandler.IsButtonDown(PlayerIndex.One, PlayerInput.Red, true) && InputHandler.IsButtonUp(PlayerIndex.One, PlayerInput.Red, false)))
                    {
                        menuState = MenuState.Pause;
                        menuNumber = 3;
                    }
                    if (InputHandler.IsButtonDown(PlayerIndex.One, PlayerInput.Blue, true) && InputHandler.IsButtonUp(PlayerIndex.One, PlayerInput.Blue, false))
                    {
                        menuState = MenuState.Pause;
                        menuNumber = 3;
                    }
                    break;
                #endregion
            #endregion

            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!(menuState == MenuState.Pause || menuState == MenuState.PauseOptions || menuState == MenuState.PauseQuit))
                spriteBatch.Draw(TextureManager.menuBackground, Vector2.Zero, Color.White);
            else
                spriteBatch.Draw(TextureManager.pauseMenu, Vector2.Zero, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1);


            #region menuStates
            switch (menuState)
            {
                #region Main Menu
                case MenuState.MainMenu:
                    if (menuNumber == 1)
                    {
                        spriteBatch.Draw(TextureManager.menuNewGame, new Vector2(450, 300), Color.Red);
                    }
                    else
                        spriteBatch.Draw(TextureManager.menuNewGame, new Vector2(450, 300), Color.Yellow);

                    if (menuNumber == 2)
                    {
                        spriteBatch.Draw(TextureManager.menuOptions, new Vector2(450, 487), Color.Red);
                    }
                    else
                        spriteBatch.Draw(TextureManager.menuOptions, new Vector2(450, 487), Color.Yellow);
                    break;
                #endregion
                #region New Game
                case MenuState.NewGame:
                    if (menuNumber == 1)
                    {
                        spriteBatch.Draw(TextureManager.menuOnePlayer, new Vector2(450, 300), Color.Red);
                    }
                    else
                        spriteBatch.Draw(TextureManager.menuOnePlayer, new Vector2(450, 300), Color.Yellow);
                    if (menuNumber == 2)
                    {
                        spriteBatch.Draw(TextureManager.menuTwoPlayer, new Vector2(450, 487), Color.Red);
                    }
                    else
                        spriteBatch.Draw(TextureManager.menuTwoPlayer, new Vector2(450, 487), Color.Yellow);
                    break;
                #endregion
                #region Difficulty
                case MenuState.Difficulty:
                    if (menuNumber == 1)
                    {
                        spriteBatch.Draw(TextureManager.noob, new Vector2(450, 300), Color.Red);
                    }
                    else
                        spriteBatch.Draw(TextureManager.noob, new Vector2(450, 300), Color.Yellow);
                    if (menuNumber == 2)
                    {
                        spriteBatch.Draw(TextureManager.hardcore, new Vector2(450, 550), Color.Red);
                    }
                    else
                        spriteBatch.Draw(TextureManager.hardcore, new Vector2(450, 550), Color.Yellow);
                    break;
                #endregion
                #region Options
                case MenuState.Options:
                    if (menuNumber == 1)
                    {
                        spriteBatch.Draw(TextureManager.menuSound, new Vector2(378, 300), Color.Red);
                    }
                    else
                        spriteBatch.Draw(TextureManager.menuSound, new Vector2(378, 300), Color.Yellow);

                    if (AudioManager.sound)
                    {
                        spriteBatch.Draw(TextureManager.menuSoundOn, new Vector2(1530, 345), Color.Yellow);
                    }
                    else
                        spriteBatch.Draw(TextureManager.menuSoundOff, new Vector2(1530, 340), Color.Yellow);

                    if (menuNumber == 2)
                    {
                        spriteBatch.Draw(TextureManager.menuHow, new Vector2(465, 525), Color.Red);
                    }
                    else
                        spriteBatch.Draw(TextureManager.menuHow, new Vector2(465, 525), Color.Yellow);
                    break;
                #endregion
                #region How To Play
                case MenuState.HowToPlay:
                    spriteBatch.Draw(TextureManager.menuMoves, new Vector2(435, 390), Color.White);
                    break;
                #endregion
                #region Pause
                case MenuState.Pause:

                    if (menuNumber == 1)
                    {

                        spriteBatch.Draw(TextureManager.pauseResume, new Vector2(450, 262), Color.Red);
                    }
                    else
                        spriteBatch.Draw(TextureManager.pauseResume, new Vector2(450, 262), Color.Yellow);

                    if (menuNumber == 2)
                    {
                        spriteBatch.Draw(TextureManager.menuOptions, new Vector2(450, 450), Color.Red);
                    }
                    else
                        spriteBatch.Draw(TextureManager.menuOptions, new Vector2(450, 450), Color.Yellow);

                    if (menuNumber == 3)
                    {
                        spriteBatch.Draw(TextureManager.menuQuit, new Vector2(450, 637), Color.Red);
                    }
                    else
                        spriteBatch.Draw(TextureManager.menuQuit, new Vector2(450, 637), Color.Yellow);
                    break;
                #endregion
                #region Pause Options
                case MenuState.PauseOptions:

                    if (menuNumber == 1)
                    {
                        spriteBatch.Draw(TextureManager.menuSound, new Vector2(435, 300), Color.Red);
                    }
                    else
                        spriteBatch.Draw(TextureManager.menuSound, new Vector2(435, 300), Color.Yellow);

                    if (AudioManager.sound)
                    {
                        spriteBatch.Draw(TextureManager.menuSoundOn, new Vector2(1200, 345), Color.Yellow);
                    }
                    else
                        spriteBatch.Draw(TextureManager.menuSoundOff, new Vector2(1200, 340), Color.Yellow);

                    if (menuNumber == 2)
                    {
                        spriteBatch.Draw(TextureManager.menuHow, new Vector2(500, 525), Color.Red);
                    }
                    else
                        spriteBatch.Draw(TextureManager.menuHow, new Vector2(500, 525), Color.Yellow);
                    if (howTo)
                    {
                        spriteBatch.Draw(TextureManager.pauseMenuHow, new Vector2(30, 37), Color.White);
                    }

                    break;
                #endregion
                #region Pause Quit
                case MenuState.PauseQuit:
                    spriteBatch.Draw(TextureManager.pauseQuest, new Vector2(517, 375), Color.Yellow);
                    if (menuNumber == 2)
                    {
                        spriteBatch.Draw(TextureManager.pauseYes, new Vector2(637, 607), Color.Red);
                    }
                    else
                        spriteBatch.Draw(TextureManager.pauseYes, new Vector2(637, 607), Color.Yellow);
                    if (menuNumber == 1)
                    {
                        spriteBatch.Draw(TextureManager.pauseNo, new Vector2(1050, 604), Color.Red);
                    }
                    else
                        spriteBatch.Draw(TextureManager.pauseNo, new Vector2(1050, 604), Color.Yellow);
                    break;
                #endregion
            #endregion
            }
        }
    }
}
