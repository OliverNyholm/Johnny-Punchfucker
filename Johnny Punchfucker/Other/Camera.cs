using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Johnny_Punchfucker
{
    class Camera
    {
        public static Vector2 prevCentre;
        public static Matrix transform;
        Viewport view;
        public static Vector2 centre;
        public static bool smooth;
        int CameraAndPlayerSamePos = 1893;
        float x;

        public Camera(Viewport newView)
        {
            view = newView;
        }

        public void Update(Vector2 playerPos, Rectangle pRec)
        {


            centre = new Vector2(playerPos.X + (pRec.Width / 2) - 1200, 0);

            if (smooth)
            {
                centre = new Vector2(playerPos.X + (pRec.Width / 2) - (x), 0);
                x -= 7.5f;
                if (x < 1200)
                {
                    x = 1200;
                    smooth = false;
                }
            }
            else
            {
                x = playerPos.X + 48 - prevCentre.X; //32
            }

            if (prevCentre.X < centre.X && centre.X + CameraAndPlayerSamePos < ContentLoader.levelEndPosX)
            {
                transform = Matrix.CreateScale(new Vector3(1, 1, 0))
                * Matrix.CreateTranslation(new Vector3(-centre.X, -centre.Y, 0));

                prevCentre = centre;
            }
        }

        public Matrix GetTransform
        {
            get
            {
                return transform;
            }
        }
        public Vector2 GetCameraPos
        {
            get
            {
                return centre;
            }
        }

    }
}

