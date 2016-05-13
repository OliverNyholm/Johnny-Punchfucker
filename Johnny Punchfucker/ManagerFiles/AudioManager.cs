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
    public static class AudioManager
    {
        public static bool sound = true;
        public static Song intro { get; private set; }

        #region Johnnys voice
        public static SoundEffect Johnny_Aouch { get; private set; }
        public static SoundEffect Johnny_cheater { get; private set; }
        public static SoundEffect Johnny_Eh { get; private set; }
        public static SoundEffect Johnny_FasterThenTheSpeedOfFight { get; private set; }
        public static SoundEffect Johnny_Hiker { get; private set; }
        public static SoundEffect Johnny_Hmmmh { get; private set; }
        public static SoundEffect Johnny_Huh { get; private set; }
        public static SoundEffect Johnny_hurtMe { get; private set; }
        public static SoundEffect Johnny_ICouldDoItAllNight { get; private set; }
        public static SoundEffect Johnny_Johnny { get; private set; }
        public static SoundEffect Johnny_LightsOut { get; private set; }
        public static SoundEffect Johnny_loveIt { get; private set; }
        public static SoundEffect Johnny_Mhhh { get; private set; }
        public static SoundEffect Johnny_NoTimeForButt { get; private set; }
        public static SoundEffect Johnny_Oaaooooa { get; private set; }
        public static SoundEffect Johnny_OhMommy { get; private set; }
        public static SoundEffect Johnny_screamForMe { get; private set; }
        public static SoundEffect Johnny_takeThis { get; private set; }
        public static SoundEffect Johnny_Wheiiii { get; private set; }
        #endregion

        #region Mingy voice
        public static SoundEffect Mingy_BOMBSBOMBSBOMPS { get; private set; }
        public static SoundEffect Mingy_CatchThis { get; private set; }
        public static SoundEffect Mingy_CutItOute { get; private set; }
        public static SoundEffect Mingy_HoIHeh { get; private set; }
        public static SoundEffect Mingy_Huh { get; private set; }
        public static SoundEffect Mingy_IAmAGamechanger { get; private set; }
        public static SoundEffect Mingy_keepTryin { get; private set; }
        public static SoundEffect Mingy_MingyStrike { get; private set; }
        public static SoundEffect Mingy_mjlk { get; private set; }
        public static SoundEffect Mingy_Myngux2Moh { get; private set; }
        public static SoundEffect Mingy_niceTry { get; private set; }
        public static SoundEffect Mingy_nowImMad { get; private set; }
        public static SoundEffect Mingy_ooohMingyMongoDoId { get; private set; }
        public static SoundEffect Mingy_Ouch { get; private set; }
        public static SoundEffect Mingy_respectTheMyngi { get; private set; }
        public static SoundEffect Mingy_ScrewThis { get; private set; }
        public static SoundEffect Mingy_SoonerOrLater { get; private set; }
        public static SoundEffect Mingy_TheresGonnaBeAnExplosion { get; private set; }
        public static SoundEffect Mingy_YouLoooooseHeh { get; private set; }
        #endregion

        #region Tommys voice4
        public static SoundEffect Tommy_AH { get; private set; }
        public static SoundEffect Tommy_Ahuuu { get; private set; }
        public static SoundEffect Tommy_Ao { get; private set; }
        public static SoundEffect Tommy_Aoa { get; private set; }
        public static SoundEffect Tommy_Aooii { get; private set; }
        public static SoundEffect Tommy_Auh { get; private set; }
        public static SoundEffect Tommy_DirtyHands { get; private set; }
        public static SoundEffect Tommy_DoesEverythingHaveToBeBoutYou { get; private set; }
        public static SoundEffect Tommy_Fabulous { get; private set; }
        public static SoundEffect Tommy_ImGonnaBeAnAngel { get; private set; }
        public static SoundEffect Tommy_LeaveMeAlone { get; private set; }
        public static SoundEffect Tommy_LickMyPeePee { get; private set; }
        public static SoundEffect Tommy_Nonononono { get; private set; }
        public static SoundEffect Tommy_NotAgain { get; private set; }
        public static SoundEffect Tommy_OhPlease { get; private set; }
        public static SoundEffect Tommy_Ouch { get; private set; }
        public static SoundEffect Tommy_Ouh { get; private set; }
        public static SoundEffect Tommy_PleaseDont { get; private set; }
        public static SoundEffect Tommy_TakeMeWithYou { get; private set; }
        public static SoundEffect Tommy_Toodiloo { get; private set; }
        public static SoundEffect Tommy_WienerStrike { get; private set; }
        public static SoundEffect Tommy_YMCA { get; private set; }

        #endregion

        #region EffectSound
        public static SoundEffect Explosion { get; private set; }
        public static SoundEffect Punch1 { get; private set; }
        public static SoundEffect Punch2 { get; private set; }
        public static SoundEffect Punch3 { get; private set; }
        public static SoundEffect Level { get; private set; }
        public static SoundEffect MenuMusic { get; private set; }
        public static SoundEffect MenuMove { get; private set; }
        public static SoundEffect MenuBack { get; private set; }
        public static SoundEffect Win { get; private set; }
        public static SoundEffect Eat { get; private set; }
        public static SoundEffect Bullet { get; private set; }
        public static SoundEffect Jump { get; private set; }




        #endregion


        public static void LoadContent(ContentManager Content)
        {
            intro = Content.Load<Song>(@"Audio\Intro\INTRO");

            #region Johnnys voice
            Johnny_Aouch = Content.Load<SoundEffect>(@"Audio\Johnny\Aouch");
            Johnny_cheater = Content.Load<SoundEffect>(@"Audio\Johnny\cheater");
            Johnny_Eh = Content.Load<SoundEffect>(@"Audio\Johnny\Eh");
            Johnny_FasterThenTheSpeedOfFight = Content.Load<SoundEffect>(@"Audio\Johnny\FasterThenTheSpeedOfFight");
            Johnny_Hiker = Content.Load<SoundEffect>(@"Audio\Johnny\Hiker");
            Johnny_Hmmmh = Content.Load<SoundEffect>(@"Audio\Johnny\Hmmmh");
            Johnny_Huh = Content.Load<SoundEffect>(@"Audio\Johnny\Huh");
            Johnny_hurtMe = Content.Load<SoundEffect>(@"Audio\Johnny\hurtMe");
            Johnny_ICouldDoItAllNight = Content.Load<SoundEffect>(@"Audio\Johnny\ICouldDoItAllNight");
            Johnny_Johnny = Content.Load<SoundEffect>(@"Audio\Johnny\Johnny");
            Johnny_LightsOut = Content.Load<SoundEffect>(@"Audio\Johnny\LightsOut");
            Johnny_loveIt = Content.Load<SoundEffect>(@"Audio\Johnny\loveIt");
            Johnny_Mhhh = Content.Load<SoundEffect>(@"Audio\Johnny\Mhhh");
            Johnny_NoTimeForButt = Content.Load<SoundEffect>(@"Audio\Johnny\NoTimeForButt");
            Johnny_Oaaooooa = Content.Load<SoundEffect>(@"Audio\Johnny\Oaaooooa");
            Johnny_OhMommy = Content.Load<SoundEffect>(@"Audio\Johnny\OhMommy");
            Johnny_screamForMe = Content.Load<SoundEffect>(@"Audio\Johnny\screamForMe");
            Johnny_takeThis = Content.Load<SoundEffect>(@"Audio\Johnny\takeThis");
            Johnny_Wheiiii = Content.Load<SoundEffect>(@"Audio\Johnny\Wheiiii");
            #endregion

            #region Mingys voice
            Mingy_BOMBSBOMBSBOMPS = Content.Load<SoundEffect>(@"Audio\Mingy\BOMBSBOMBSBOMPS");
            Mingy_CatchThis = Content.Load<SoundEffect>(@"Audio\Mingy\CatchThis");
            Mingy_CutItOute = Content.Load<SoundEffect>(@"Audio\Mingy\CutItOute");
            Mingy_HoIHeh = Content.Load<SoundEffect>(@"Audio\Mingy\HoIHeh");
            Mingy_Huh = Content.Load<SoundEffect>(@"Audio\Mingy\Huh");
            Mingy_IAmAGamechanger = Content.Load<SoundEffect>(@"Audio\Mingy\IAmAGamechanger");
            Mingy_keepTryin = Content.Load<SoundEffect>(@"Audio\Mingy\keepTryin");
            Mingy_MingyStrike = Content.Load<SoundEffect>(@"Audio\Mingy\MingyStrike");
            Mingy_mjlk = Content.Load<SoundEffect>(@"Audio\Mingy\mjlk");
            Mingy_Myngux2Moh = Content.Load<SoundEffect>(@"Audio\Mingy\Myngux2Moh");
            Mingy_niceTry = Content.Load<SoundEffect>(@"Audio\Mingy\niceTry");
            Mingy_nowImMad = Content.Load<SoundEffect>(@"Audio\Mingy\nowImMad");
            Mingy_ooohMingyMongoDoId = Content.Load<SoundEffect>(@"Audio\Mingy\ooohMingyMongoDoId");
            Mingy_Ouch = Content.Load<SoundEffect>(@"Audio\Mingy\Ouch");
            Mingy_respectTheMyngi = Content.Load<SoundEffect>(@"Audio\Mingy\respectTheMyngi");
            Mingy_ScrewThis = Content.Load<SoundEffect>(@"Audio\Mingy\ScrewThis");
            Mingy_SoonerOrLater = Content.Load<SoundEffect>(@"Audio\Mingy\SoonerOrLater");
            Mingy_TheresGonnaBeAnExplosion = Content.Load<SoundEffect>(@"Audio\Mingy\TheresGonnaBeAnExplosion");
            Mingy_YouLoooooseHeh = Content.Load<SoundEffect>(@"Audio\Mingy\YouLoooooseHeh");
            #endregion

            #region Tommys voice
            Tommy_AH = Content.Load<SoundEffect>(@"Audio\Tommy\AH");
            Tommy_Ahuuu = Content.Load<SoundEffect>(@"Audio\Tommy\Ahuuu");
            Tommy_Ao = Content.Load<SoundEffect>(@"Audio\Tommy\Ao");
            Tommy_Aoa = Content.Load<SoundEffect>(@"Audio\Tommy\Aoa");
            Tommy_Aooii = Content.Load<SoundEffect>(@"Audio\Tommy\Aooii");
            Tommy_Auh = Content.Load<SoundEffect>(@"Audio\Tommy\Auh");
            Tommy_DirtyHands = Content.Load<SoundEffect>(@"Audio\Tommy\DirtyHands");
            Tommy_DoesEverythingHaveToBeBoutYou = Content.Load<SoundEffect>(@"Audio\Tommy\DoesEverythingHaveToBeBoutYou");
            Tommy_Fabulous = Content.Load<SoundEffect>(@"Audio\Tommy\Fabulous");
            Tommy_ImGonnaBeAnAngel = Content.Load<SoundEffect>(@"Audio\Tommy\ImGonnaBeAnAngel");
            Tommy_LeaveMeAlone = Content.Load<SoundEffect>(@"Audio\Tommy\LeaveMeAlone");
            Tommy_LickMyPeePee = Content.Load<SoundEffect>(@"Audio\Tommy\LickMyPeePee");
            Tommy_Nonononono = Content.Load<SoundEffect>(@"Audio\Tommy\Nonononono");
            Tommy_NotAgain = Content.Load<SoundEffect>(@"Audio\Tommy\NotAgain");
            Tommy_OhPlease = Content.Load<SoundEffect>(@"Audio\Tommy\OhPlease");
            Tommy_Ouch = Content.Load<SoundEffect>(@"Audio\Tommy\Ouch");
            Tommy_Ouh = Content.Load<SoundEffect>(@"Audio\Tommy\Ouh");
            Tommy_PleaseDont = Content.Load<SoundEffect>(@"Audio\Tommy\PleaseDont");
            Tommy_TakeMeWithYou = Content.Load<SoundEffect>(@"Audio\Tommy\TakeMeWithYou");
            Tommy_Toodiloo = Content.Load<SoundEffect>(@"Audio\Tommy\Toodiloo");
            Tommy_WienerStrike = Content.Load<SoundEffect>(@"Audio\Tommy\WienerStrike");
            Tommy_YMCA = Content.Load<SoundEffect>(@"Audio\Tommy\YMCA");
            #endregion

            #region Song
            Explosion = Content.Load<SoundEffect>(@"Audio\SoundEffects\Explosion");
            Punch1 = Content.Load<SoundEffect>(@"Audio\SoundEffects\Punch1");
            Punch2 = Content.Load<SoundEffect>(@"Audio\SoundEffects\Punch2");
            Punch3 = Content.Load<SoundEffect>(@"Audio\SoundEffects\Punch3");
            Eat = Content.Load<SoundEffect>(@"Audio\SoundEffects\Eat");
            Level = Content.Load<SoundEffect>(@"Audio\backgroundmusic\Level1Music1");
            MenuMusic = Content.Load<SoundEffect>(@"Audio\backgroundmusic\MenuMusic");
            MenuMove = Content.Load<SoundEffect>(@"Audio\backgroundmusic\MenuMove1");
            MenuBack = Content.Load<SoundEffect>(@"Audio\backgroundmusic\MenuBack1");
            Win = Content.Load<SoundEffect>(@"Audio\backgroundmusic\Win");
            Bullet = Content.Load<SoundEffect>(@"Audio\SoundEffects\bullet");
            Jump = Content.Load<SoundEffect>(@"Audio\SoundEffects\Jump");

            #endregion



        }
    }
}
