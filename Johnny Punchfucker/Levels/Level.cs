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
    abstract class Level
    {
        public ContentLoader contentLoader;
        protected PlayerManager playerManager;
        protected EnemyManager enemyManager;
        public Rectangle nextLevelBox;

        public Level(ContentManager Content, PlayerManager playerManager, EnemyManager enemyManager)
        {
            this.playerManager = playerManager;
            this.enemyManager = enemyManager;
        }
    }
}
