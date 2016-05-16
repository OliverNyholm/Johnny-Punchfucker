    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Johnny_Punchfucker
{
    public static class TextureManager
    {
        #region Intro
        public static Texture2D introScreen1 { get; private set; }
        public static Texture2D introScreen2 { get; private set; }
        public static Texture2D introScreen3 { get; private set; }
        public static Texture2D introScreen4 { get; private set; }
        #endregion

        #region Menu
        public static Texture2D loadingScreen { get; private set; }
        public static Texture2D loadingCircle { get; private set; }
        public static Texture2D menuBackground { get; private set; }
        public static Texture2D menuNewGame { get; private set; }
        public static Texture2D menuOptions { get; private set; }
        public static Texture2D menuHow { get; private set; }
        public static Texture2D menuMoves { get; private set; }
        public static Texture2D highscore { get; private set; }
        public static Texture2D highscoreText { get; private set; }
        public static Texture2D highscorebeaten { get; private set; }
        public static Texture2D highscoreunbeaten { get; private set; }




        public static Texture2D menuQuit { get; private set; }
        public static Texture2D menuOnePlayer { get; private set; }
        public static Texture2D menuTwoPlayer { get; private set; }
        public static Texture2D menuSound { get; private set; }
        public static Texture2D menuSoundOn { get; private set; }
        public static Texture2D menuSoundOff { get; private set; }
        public static Texture2D hardcore { get; private set; }
        public static Texture2D noob { get; private set; }

        public static Texture2D pauseNo { get; private set; }
        public static Texture2D pauseYes { get; private set; }
        public static Texture2D pauseResume { get; private set; }
        public static Texture2D pauseMenu { get; private set; }
        public static Texture2D pauseQuest { get; private set; }
        public static Texture2D pauseMenuHow { get; private set; }


        public static Texture2D endScreenTex { get; private set; }
        public static Texture2D endScreenHC { get; private set; }
        public static Texture2D gameOverScreenTex { get; private set; }

        #endregion

        #region StatusBar graphics
        public static Texture2D lifeBarPlayerOneTex { get; private set; }
        public static Texture2D lifeBarTex { get; private set; }

        #endregion

        #region Environment graphics
        public static Texture2D beachbackgroundTex { get; private set; }
        public static Texture2D statusBarPlayerOneTex { get; private set; }
        public static Texture2D statusBarPlayerTwoTex { get; private set; }
        public static Texture2D beachback2groundTex { get; private set; }
        public static Texture2D beachback3groundTex { get; private set; }
        public static Texture2D waterbackgroundTex { get; private set; }
        public static Texture2D bridgebackgroundTex { get; private set; }
        public static Texture2D roadTex { get; private set; }
        public static Texture2D startMenuTex { get; private set; }
        public static Texture2D smallPlantTex { get; private set; }
        public static Texture2D jungleEntranceTex { get; private set; }
        public static Texture2D jungleBackground1Tex { get; private set; }
        public static Texture2D jungleBackground2Tex { get; private set; }
        public static Texture2D jungleRoadTex { get; private set; }
        public static Texture2D jungleRoadThornTex { get; private set; }
        public static Texture2D templeRoadTex { get; private set; }

        #endregion

        #region Player graphics
        public static Texture2D Player1tex { get; private set; }
        public static Texture2D Player2tex { get; private set; }

        public static Texture2D playerShadow { get; private set; }

        #endregion

        #region Enemy graphics
        public static Texture2D standardEnemyTex { get; private set; }
        public static Texture2D mingy { get; private set; }
        public static Texture2D mingyBanner { get; private set; }
        public static Texture2D susan { get; private set; }

        public static Texture2D vitas { get; private set; }
        public static Texture2D vitasBanner { get; private set; }
        public static Texture2D susanBanner { get; private set; }
        public static Texture2D bulletTex { get; private set; }
        public static Texture2D bombTex { get; private set; }
        public static Texture2D shockwaveTex { get; private set; }
        public static Texture2D fireshieldTex { get; private set; }
        public static Texture2D explosionTex { get; private set; }
        public static Texture2D shockwave { get; private set; }
        public static Texture2D sandripple { get; private set; }
        #endregion

        #region Items
        public static Texture2D cake { get; private set; }
        public static Texture2D pinacolada { get; private set; }
        public static Texture2D sabreTex { get; private set; }
        public static Texture2D barrel { get; private set; }
        #endregion

        #region Fonts
        public static SpriteFont timeFont { get; private set; }
        #endregion

        #region ParticleEffects
        public static Texture2D bloodTex { get; private set; }
        #endregion

        public static void LoadContent(ContentManager Content)
        {
            #region Intro
            introScreen1 = Content.Load<Texture2D>(@"Images1080\intro1");
            introScreen2 = Content.Load<Texture2D>(@"Images1080\intro2");
            introScreen3 = Content.Load<Texture2D>(@"Images1080\intro3");
            //introScreen4 = Content.Load<Texture2D>(@"Images1080\introScreen4");
            #endregion

            #region Menu
            loadingScreen = Content.Load<Texture2D>(@"Images1080\loadingScreen");
            loadingCircle = Content.Load<Texture2D>(@"Images1080\loadingCircle");
            menuBackground = Content.Load<Texture2D>(@"Images1080\MenuImages\menuBackground");
            menuNewGame = Content.Load<Texture2D>(@"Images1080\MenuImages\MenuNewGame");
            highscore = Content.Load<Texture2D>(@"Images1080\MenuImages\Highscore");
            highscoreText = Content.Load<Texture2D>(@"Images1080\MenuImages\HighscoreTExt");
            highscorebeaten = Content.Load<Texture2D>(@"Images1080\MenuImages\IntoHighscore");
            highscoreunbeaten = Content.Load<Texture2D>(@"Images1080\MenuImages\NeededForHighscore");
            menuOptions = Content.Load<Texture2D>(@"Images1080\MenuImages\MenuOptions");
            menuQuit = Content.Load<Texture2D>(@"Images1080\MenuImages\MenuQuit");
            menuOnePlayer = Content.Load<Texture2D>(@"Images1080\MenuImages\MenuOnePlayer");
            menuTwoPlayer = Content.Load<Texture2D>(@"Images1080\MenuImages\MenuTwoPlayer");
            menuSound = Content.Load<Texture2D>(@"Images1080\MenuImages\MenuSound");
            menuSoundOn = Content.Load<Texture2D>(@"Images1080\MenuImages\MenuSoundOn");
            menuSoundOff = Content.Load<Texture2D>(@"Images1080\MenuImages\MenuSoundOff");
            hardcore = Content.Load<Texture2D>(@"Images1080\MenuImages\Hardcore");
            noob = Content.Load<Texture2D>(@"Images1080\MenuImages\Noob");
            menuHow = Content.Load<Texture2D>(@"Images1080\MenuImages\MenuHow");
            menuMoves = Content.Load<Texture2D>(@"Images1080\MenuImages\Moves");
            pauseMenu = Content.Load<Texture2D>(@"Images1080\MenuImages\pauseMenu");
            pauseMenuHow = Content.Load<Texture2D>(@"Images1080\MenuImages\pauseMenuHow");

            pauseNo = Content.Load<Texture2D>(@"Images1080\MenuImages\MenuNo");
            pauseYes = Content.Load<Texture2D>(@"Images1080\MenuImages\MenuYes");
            pauseResume = Content.Load<Texture2D>(@"Images1080\MenuImages\MenuResume");
            pauseQuest = Content.Load<Texture2D>(@"Images1080\MenuImages\PauseQuest");
            endScreenTex = Content.Load<Texture2D>(@"Images1080\endScreen");
            endScreenHC = Content.Load<Texture2D>(@"Images1080\endScreenHC");
            gameOverScreenTex = Content.Load<Texture2D>(@"Images1080\gameover");
            #endregion

            #region StatusBar graphics
            lifeBarTex = Content.Load<Texture2D>(@"Images1080\Lifebar");

            #endregion

            #region Environment graphics
            beachbackgroundTex = Content.Load<Texture2D>(@"Images1080\beachbackground");
            beachback2groundTex = Content.Load<Texture2D>(@"Images1080\beachbackground2");
            beachback3groundTex = Content.Load<Texture2D>(@"Images1080\beachbackground3");
            waterbackgroundTex = Content.Load<Texture2D>(@"Images1080\waterbackground");
            bridgebackgroundTex = Content.Load<Texture2D>(@"Images1080\bridgebackground");
            statusBarPlayerOneTex = Content.Load<Texture2D>(@"Images1080\StatusBar1player");
            statusBarPlayerTwoTex = Content.Load<Texture2D>(@"Images1080\StatusBar");
            roadTex = Content.Load<Texture2D>(@"Images1080\road");
          //  startMenuTex = Content.Load<Texture2D>(@"Images1080\startmenu");
            smallPlantTex = Content.Load<Texture2D>(@"Images1080\plant");
          //  jungleEntranceTex = Content.Load<Texture2D>(@"Images1080\jungleEntrance");
            jungleBackground1Tex = Content.Load<Texture2D>(@"Images1080\junglebackground3");
            jungleBackground2Tex = Content.Load<Texture2D>(@"Images1080\junglebackground4");
          //  jungleRoadTex = Content.Load<Texture2D>(@"Images1080\jungleroad");
            jungleRoadThornTex = Content.Load<Texture2D>(@"Images1080\jungleroadThorn");
          //  templeRoadTex = Content.Load<Texture2D>(@"Images\templeroad");

            #endregion

            #region Player graphics
            Player1tex = Content.Load<Texture2D>(@"Images1080\Player1sprite");
            Player2tex = Content.Load<Texture2D>(@"Images1080\AlexSheet remake P2");
            playerShadow = Content.Load<Texture2D>(@"Images1080\shadow");
            #endregion

            #region Enemy graphics
            standardEnemyTex = Content.Load<Texture2D>(@"Images1080\standardenemy");
            mingy = Content.Load<Texture2D>(@"Images1080\mingusheet");
            mingyBanner = Content.Load<Texture2D>(@"Images1080\MingyBanner");
            susan = Content.Load<Texture2D>(@"Images1080\Susan");
            vitas = Content.Load<Texture2D>(@"Images1080\vitas");
            susanBanner = Content.Load<Texture2D>(@"Images1080\SusanBanner");
            vitasBanner = Content.Load<Texture2D>(@"Images1080\vitasbanner");
            bulletTex = Content.Load<Texture2D>(@"Images1080\bulletTex");
            bombTex = Content.Load<Texture2D>(@"Images1080\bombTex");
            shockwaveTex = Content.Load<Texture2D>(@"Images1080\lava");
            fireshieldTex = Content.Load<Texture2D>(@"Images1080\fireshield");
            explosionTex = Content.Load<Texture2D>(@"Images1080\Explosion02");
            shockwave = Content.Load<Texture2D>(@"Images1080\lava");
            sandripple = Content.Load<Texture2D>(@"Images1080\sand2");
            #endregion

            #region Items
            cake = Content.Load<Texture2D>(@"Images1080\cake");
            pinacolada = Content.Load<Texture2D>(@"Images1080\drink");
            barrel = Content.Load<Texture2D>(@"Images1080\barrel");
        //    sabreTex = Content.Load<Texture2D>(@"images\sabre");
            #endregion

            #region Fonts
            timeFont = Content.Load<SpriteFont>(@"Fonts\timeFont");
            #endregion

            #region ParticleEffects
            bloodTex = Content.Load<Texture2D>(@"Images1080\Blood");
            #endregion
        }
    }
}
