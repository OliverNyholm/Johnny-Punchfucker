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
    abstract class GameObjects
    {
        public Texture2D tex;
        public Vector2 pos, offset;
        public Rectangle boundingBox;
        public int width, height;

        protected bool isDead;
        public bool IsDead
        {
            get { return isDead; }
            set { isDead = value; }
        }

        public GameObjects(Texture2D tex, Vector2 pos)
        {
            this.tex = tex;
            this.pos = pos;

            width = tex.Width;
            height = tex.Height;

            offset = new Vector2(width / 2, height / 2);
        }
    }
}
