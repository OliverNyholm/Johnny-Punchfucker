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
    public class Game1 : Game
    {
        Camera camera;
        float loadingRotation = 1;
        double loadingTime;
        public static Random random;
        public static bool ready;
        GameManager gameManager;
        Viewport defaultView;

#if (!ARCADE)
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
#else
		public override string GameDisplayName { get { return "Johnny Punchfucker"; } }
#endif

        public Game1()
        {
#if (!ARCADE)
            graphics = new GraphicsDeviceManager(this);
#endif
            
        }
        protected override void Initialize()
        {
            Content.RootDirectory = "Content";
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
#if (!ARCADE)
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
#endif
            defaultView = new Viewport(0, 0, 1920, 1080);

            gameManager = new GameManager();
            gameManager.LoadContent(Content, GraphicsDevice, spriteBatch);
            camera = new Camera(defaultView);
            Game1.random = new Random();
            

        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
#if (!ARCADE)
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
#endif
            if (!ready && gameManager.gameState == GameManager.GameState.Play)
            {
                loadingRotation *= 1.008f; //gör att cirkeln roterar vid loadingScreen
                loadingTime += gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (loadingTime >= 2f && !ready)
            {
                ready = true; // för att spelet ska hinna ladda in före kameran går igång. Kameran annars ledsen :(
                loadingRotation = 1;
                loadingTime = 0;
                gameManager.digitSeconds = 0;
            }

            gameManager.Update(gameTime, GraphicsDevice, Content);

            base.Update(gameTime);
            #region Camera Update
            if (gameManager.gameState == GameManager.GameState.Play && ready || gameManager.gameState == GameManager.GameState.Pause)
            {
                if (PlayerManager.players == 1)
                {
                    camera.Update(gameManager.playerManager.playerList[0].GetPos, gameManager.playerManager.playerList[0].GetRec);
                }
                else if (PlayerManager.players == 2)
                {
                    if (!gameManager.playerManager.playerList[0].dead)
                    {
                        camera.Update(gameManager.playerManager.playerList[0].GetPos, gameManager.playerManager.playerList[0].GetRec);
                    }
                    else if (gameManager.playerManager.playerList[0].dead)
                        camera.Update(gameManager.playerManager.playerList[1].GetPos, gameManager.playerManager.playerList[1].GetRec);
                }
            }
            #endregion

            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            if (gameManager.gameState == GameManager.GameState.Play || gameManager.gameState == GameManager.GameState.Pause)
            {
                GraphicsDevice.Clear(Color.LightPink);
                spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, null, null, null, null, camera.GetTransform);
                gameManager.Draw(spriteBatch);
                spriteBatch.End();

                spriteBatch.Begin();
                gameManager.DrawStats(spriteBatch);
                if (!ready)
                {
                    spriteBatch.Draw(TextureManager.loadingScreen, Vector2.Zero, Color.White);
                    spriteBatch.Draw(TextureManager.loadingCircle, new Vector2(960, 620), null, Color.White, loadingRotation, new Vector2(75, 75), 1, SpriteEffects.None, 1);
                }
                spriteBatch.End();
            }
            else // för att få kameran ur funktion när man är i menyn
            {
                GraphicsDevice.Clear(Color.LightPink);
                spriteBatch.Begin();
                gameManager.Draw(spriteBatch);
                spriteBatch.End();
            }

            base.Draw(gameTime);
        }
    }
}
