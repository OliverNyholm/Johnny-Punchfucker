using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Graphics;

namespace Johnny_Punchfucker
{
    class Player : Entity
    {
        //public int lifePoints;
        public Vector2 playerLeftPos, playerRightPos;
        public Rectangle playerLeftBox, playerRightBox;
        float shadowScale;
        int yLimitUp = 505, yLimitDown = 869, hurtSound, hitSound;
        public PlayerIndex playerIndex;
        public bool ableToMoveRight = true, hurtTalk, hitTalk;
        bool kick;
        TimeSpan hurtTalkTimer, hitTalkTimer;
        public float jumpDelay = 0.9f;
        public TimeSpan jumpTimer;
        bool canJump = false;


        public Player(Texture2D tex, Vector2 pos, PlayerIndex playerIndex)
            : base(tex, pos)
        {
            this.playerIndex = playerIndex;
            posJump = pos;
            animationBox = new Rectangle(0, 0, /*75*/112, /*116*/174);
            width /= 9;
            height /= 10;
            shadowScale = 1;
            this.life = 10;
            maxLife = life;
            this.speed = new Vector2(0, 0);
            offset = new Vector2(width / 2, height / 2);
            jumpTimer = TimeSpan.FromSeconds(jumpDelay);
        }

        public override void Update(GameTime gameTime)
        {
            Death(gameTime);
            FloatLayerCalculator();
            //Console.Write(life);

            percentLife = life / maxLife;

            pos += speed;
            posJump.X = pos.X;
            shadowScale = 1f - ((posJump.Y - pos.Y) * -0.01f);
            speed.X = 0;

            if (!canJump)
            {
                if (jumpTimer.TotalSeconds > 0)
                    jumpTimer = jumpTimer.Subtract(gameTime.ElapsedGameTime);
                else
                {
                    canJump = true;
                    jumpTimer = TimeSpan.FromSeconds(jumpDelay);
                }
            }

            #region Sound Timers & Bools
            if (hurtTalk)
            {
                if (hurtTalkTimer.TotalSeconds > 0)
                    hurtTalkTimer = hurtTalkTimer.Subtract(gameTime.ElapsedGameTime);
                else
                {
                    hurtTalk = false;
                }
            }

            if (hitTalk)
            {
                if (hitTalkTimer.TotalSeconds > 0)
                    hitTalkTimer = hitTalkTimer.Subtract(gameTime.ElapsedGameTime);
                else
                {
                    hitTalk = false;
                }
            }
            #endregion

            if (!onGround || (dead && pos.Y < 1500))
                speed.Y += 0.14f;
            else
                speed.Y = 0;

            if (!dead)
            {
                boundingBox = new Rectangle((int)pos.X - width / 2 - 6, (int)pos.Y - height / 2, width - 24, height);
                playerLeftPos = new Vector2(feetBox.X - 67, feetBox.Y); //positionen som fienden ska gå till vänster om spelaren
                playerRightPos = new Vector2(feetBox.X + width - 45, feetBox.Y);

                if (onGround) //Om vi är på marken så är Y = pos.Y
                {
                    feetBox = new Rectangle((int)pos.X - (int)66, (int)pos.Y + (169 - 5) - (int)offset.Y, width - 25, height - (height - 4));
                    if (spriteEffect == SpriteEffects.FlipHorizontally)
                    {
                        playerRightBox = new Rectangle((int)pos.X - 10, (int)pos.Y + 70, 37, 37); //rektangeln till höger om spelaren. Om fienden krockar i börjar han slåss
                        playerLeftBox = new Rectangle((int)pos.X - 52, (int)pos.Y + 70, 37, 37);
                    }
                    else
                    {
                        playerRightBox = new Rectangle((int)pos.X - 15, (int)pos.Y + 70, 37, 37); //rektangeln till höger om spelaren. Om fienden krockar i börjar han slåss
                        playerLeftBox = new Rectangle((int)pos.X - 52, (int)pos.Y + 70, 37, 37);
                    }
                }
                else // Om vi är i luften är Y = jumpPos.Y
                {
                    boundingBox = new Rectangle((int)pos.X - width / 2 - 6, (int)pos.Y - height / 2, width - 24, height - 34);
                    feetBox = new Rectangle((int)pos.X - (int)66, (int)posJump.Y + (169 - 5) - (int)offset.Y, width - 12, height - (height - 4));
                    if (spriteEffect == SpriteEffects.FlipHorizontally)
                    {
                        playerRightBox = new Rectangle((int)pos.X + 8, (int)posJump.Y + 70, 37, 37); //rektangeln till höger om spelaren. Om fienden krockar i börjar han slåss
                        playerLeftBox = new Rectangle((int)pos.X - 52, (int)posJump.Y + 70, 37, 37);
                    }
                    else
                    {
                        playerRightBox = new Rectangle((int)pos.X - 42, (int)posJump.Y + 70, 37, 37); //rektangeln till höger om spelaren. Om fienden krockar i börjar han slåss
                        playerLeftBox = new Rectangle((int)pos.X - 66, (int)posJump.Y + 70, 37, 37);
                    }
                }

                Moving(gameTime);
                Fight(gameTime);
                Block(gameTime);


                if ((fightFrame == 0 && !moving) || walkFrame == 0 && !fight) //Tar bort den gamla animationen som höll på när man byter till en annan animation
                {
                    animationBox.X = 0;
                }

            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            #region Shadows
            if (spriteEffect == SpriteEffects.None && !dead)
            {
                spriteBatch.Draw(TextureManager.playerShadow, new Vector2(posJump.X - 22, posJump.Y + (height / 2) - 9), null, new Color(0, 0, 0, 120), 0f, new Vector2(94.5f / 2, 31.5f / 2), shadowScale, SpriteEffects.None, 0.1f);
            }
            else if (spriteEffect == SpriteEffects.FlipHorizontally && !dead)
                spriteBatch.Draw(TextureManager.playerShadow, new Vector2(posJump.X - 7, posJump.Y + (height / 2) - 9), null, new Color(0, 0, 0, 120), 0f, new Vector2(94.5f / 2, 31.5f / 2), shadowScale, SpriteEffects.None, 0.1f);
            else if (dead)
            {
                posJump.Y = pos.Y;
                spriteBatch.Draw(TextureManager.playerShadow, new Vector2(posJump.X - 7, posJump.Y + (height / 2) - 9), null, new Color(0, 0, 0, 120), 0f, new Vector2(94.5f / 2, 31.5f / 2), shadowScale, SpriteEffects.None, 0.1f);
            }
            #endregion

            spriteBatch.Draw(tex, pos, animationBox, Color.White, 0f, new Vector2(width / 2 + 10, height / 2), 1f, spriteEffect, floatLayerNr);

            //spriteBatch.Draw(tex, feetBox, null, Color.Red, 0, Vector2.Zero, SpriteEffects.None, 1);
            //spriteBatch.Draw(tex, playerRightBox, null, Color.Red, 0, Vector2.Zero, SpriteEffects.None, 1);
            //spriteBatch.Draw(tex, playerLeftBox, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 1);
            spriteBatch.Draw(tex, punchBox, null, Color.Blue, 0, Vector2.Zero, SpriteEffects.None, 1);
            //spriteBatch.Draw(tex, boundingBox, null, Color.Red, 0, Vector2.Zero, SpriteEffects.None, 1);
            //spriteBatch.Draw(tex, blockBox, null, Color.Aquamarine, 0, Vector2.Zero, SpriteEffects.None, 1);
        }

        public void Death(GameTime gameTime)
        {
            if (life <= 0)
            {
                dead = true;
                animationBox.Y = 1530;
                animationBox.X = 0;
                animationBox.Width = 187;
                animationBox.Height = 187;
                boundingBox = new Rectangle((int)pos.X - width / 2, (int)pos.Y - height / 2, 0, 0);
                //feetBox = new Rectangle((int)pos.X - (int)49, (int)posJump.Y + (113 - 4) - (int)offset.Y, 0, 0);
                playerRightBox = new Rectangle((int)pos.X + 5, (int)pos.Y + 35, 0, 0);
                playerLeftBox = new Rectangle((int)pos.X - 30, (int)pos.Y + 35, 0, 0);
            }
            else
                dead = false;

        }

        public void Moving(GameTime gameTime)
        {
            if (!fight && Game1.ready)
            {
                #region Walk Right

                if (InputHandler.IsButtonDown(playerIndex, PlayerInput.Right) && !block && pos.X - 1900 < (Camera.prevCentre.X) && ableToMoveRight)
                {

                    speed.X = 4.5f;

                    moving = true;
                    spriteEffect = SpriteEffects.None;
                    if (InputHandler.IsButtonDown(playerIndex, PlayerInput.Up) || InputHandler.IsButtonDown(playerIndex, PlayerInput.Down))
                        speed.X = 3f;
                }
                #endregion
                #region Walk Left
                if (InputHandler.IsButtonDown(playerIndex, PlayerInput.Left) && pos.X >= Camera.prevCentre.X + 45 && !block)
                {
                    speed.X = -4.5f;
                    moving = true;
                    spriteEffect = SpriteEffects.FlipHorizontally;
                    if (InputHandler.IsButtonDown(playerIndex, PlayerInput.Up) || InputHandler.IsButtonDown(playerIndex, PlayerInput.Down))
                        speed.X = -3f;
                }
                #endregion
                #region Walk Up
                if (InputHandler.IsButtonDown(playerIndex, PlayerInput.Up) && feetBox.Y >= yLimitUp && onGround && !block)
                {
                    speed.Y = -4.5f;
                    moving = true;
                    if (InputHandler.IsButtonDown(playerIndex, PlayerInput.Left) || InputHandler.IsButtonDown(playerIndex, PlayerInput.Right))
                        speed.Y = -3f;
                }
                #endregion
                #region Walk Down
                if (InputHandler.IsButtonDown(playerIndex, PlayerInput.Down) && feetBox.Y <= yLimitDown && onGround && !block)
                {
                    speed.Y = 4.5f;
                    moving = true;
                    if (InputHandler.IsButtonDown(playerIndex, PlayerInput.Left) || InputHandler.IsButtonDown(playerIndex, PlayerInput.Right))
                        speed.Y = 3f;
                }
                #endregion
                #region Moving Bool
                if (!(InputHandler.IsButtonDown(playerIndex, PlayerInput.Left)) && !(InputHandler.IsButtonDown(playerIndex, PlayerInput.Right)) &&
                    !(InputHandler.IsButtonDown(playerIndex, PlayerInput.Up)) && !(InputHandler.IsButtonDown(playerIndex, PlayerInput.Down)))
                {
                    moving = false;
                }
                if (moving && onGround)
                {
                    posJump.Y = pos.Y;
                    animationBox.Width = 112;
                    animationBox.Y = 0;
                    Animation(120, 4, 112, gameTime);
                }
                else if (!moving && onGround && !fight && !block)
                {
                    animationBox.Width = 112;
                    animationBox.X = 0;
                    animationBox.Y = 0;
                    //Animation(120, 1, 75, gameTime);
                }

                #endregion
                #region Onground Bool and Jump
                if (!onGround)
                {
                    animationBox.Width = 112;
                    animationBox.Y = 174;
                    animationBox.X = 0;
                    //Animation(120, 1, 75, gameTime);
                    if (InputHandler.IsButtonDown(playerIndex, PlayerInput.Up) && feetBox.Y >= yLimitUp)
                    {
                        pos.Y += -1.5f;
                        posJump.Y += -1.5f;
                    }
                    if (InputHandler.IsButtonDown(playerIndex, PlayerInput.Down) && feetBox.Y <= yLimitDown && posJump.Y <= yLimitDown - 50)
                    {
                        pos.Y += 1.5f;
                        posJump.Y += 1.5f;
                    }
                    if (pos.Y >= posJump.Y)
                    {
                        pos.Y = posJump.Y;
                        onGround = true;
                        speed.Y = 0;
                    }
                }

                if (InputHandler.IsButtonDown(playerIndex, PlayerInput.Green) && onGround && !block && canJump) // här hoppar man
                {
                    posJump.Y = pos.Y; //när man hoppar svaras punkten man hoppade från i y-led. Man landar på den punkten i y-led sen
                    speed.Y = -3.2f;
                    onGround = false;
                    canJump = false;
                    if (AudioManager.sound)
                        AudioManager.Jump.Play();
                }
                #endregion
            }

        }

        public void Fight(GameTime gameTime)
        {

            if (fight)
            {
                fightTime += gameTime.ElapsedGameTime.TotalMilliseconds;
            }
            else
            {
                fightTime = 0;
                fightingCooldown += gameTime.ElapsedGameTime.TotalMilliseconds;
            }
            #region StandardHit
            if (fightingCooldown >= 300 && InputHandler.IsButtonDown(playerIndex, PlayerInput.Blue) && !fight && onGround && !block)
            {
                frameTime = 120;
                walkFrame = 0;
                moving = false;
                fight = true;
                punch = true;
                fightingCooldown = 0;
            }
            if (punch)
            {
                animationBox.Width = 121;
                animationBox.Y = 771;
                FightAnimation(90, 3, 121, gameTime);
                if (spriteEffect == SpriteEffects.FlipHorizontally)
                {
                    punchBox = new Rectangle((int)pos.X - 15 - (fightFrame * 28), (int)pos.Y - 32, 75, 30);
                }
                else
                    punchBox = new Rectangle((int)pos.X - 80 + (fightFrame * 28), (int)pos.Y - 32, 75, 30);
            }
            if (punch && fightFrame >= 3)
            {
                fight = false;
                punch = false;
                hasHit = false;
                fightFrame = 0;
                fightFrameTime = 80;
                punchBox = new Rectangle((int)pos.X - 40, (int)pos.Y - 28, 0, 0);
            }
            #endregion

            #region Kick
            if (fightingCooldown >= 300 && InputHandler.IsButtonDown(playerIndex, PlayerInput.Red) && !fight && onGround && !block)
            {
                frameTime = 120;
                walkFrame = 0;
                moving = false;
                fight = true;
                kick = true;
                fightingCooldown = 0;
            }
            if (kick)
            {
                if (fightFrame == 0)
                    animationBox.Width = 100;
                else if (fightFrame == 1)
                    animationBox.Width = 144;

                animationBox.Y = 1350;
                FightAnimation(130, 2, 100 + 44 * fightFrame, gameTime);
                if (spriteEffect == SpriteEffects.FlipHorizontally && fightFrame == 1)
                {
                    punchBox = new Rectangle((int)pos.X - 80, (int)pos.Y - 25, 30, 35);
                }
                else if (spriteEffect == SpriteEffects.None && fightFrame == 1)
                    punchBox = new Rectangle((int)pos.X + 40, (int)pos.Y - 20, 30, 30);
            }
            if (kick && fightFrame >= 2)
            {
                animationBox.Width = 112;
                animationBox.Y = 0;
                fight = false;
                kick = false;
                hasHit = false;
                fightFrame = 0;
                fightFrameTime = 80;
                punchBox = new Rectangle((int)pos.X - 40, (int)pos.Y - 28, 0, 0);
            }
            #endregion
        }

        public void Block(GameTime gameTime)
        {
            if (fightingCooldown >= 150 && InputHandler.IsButtonDown(playerIndex, PlayerInput.Yellow) && !fight && !block && onGround)
            {
                block = true;
                moving = false;
                animationBox.Width = 112;
                animationBox.X = 0;
                animationBox.Y = 967;
                fightingCooldown = 0;
            }

            if (block)
            {

                blockTimer += gameTime.ElapsedGameTime.TotalMilliseconds;
                if (spriteEffect == SpriteEffects.FlipHorizontally)
                {
                    blockBox = new Rectangle((int)pos.X - 75, (int)pos.Y - 70, 42, height); //35 på width förr
                }
                else
                    blockBox = new Rectangle((int)pos.X, (int)pos.Y - 70, 42, height);

            }
            if (block && blockTimer >= 500)
            {
                block = false;
                blockTimer = 0;
                blockBox = new Rectangle((int)pos.X, (int)pos.Y - 40, 0, 0);
            }
        }

        public void FloatLayerCalculator()
        {

            if (spriteEffect == SpriteEffects.None)
            {
                floatLayerNr = 0 + posJump.Y * 0.0010f; //numret blir mellan 0.335 och 0.583, vilket placerar en i rätt ordning(ritas först 0, ritas sist 1(?))
            }
            else
                floatLayerNr = 0 + pos.Y * 0.0010f;
        }

        public void PlayerOneHurt()
        {
            hurtSound = Game1.random.Next(1, 6);

            if (!hurtTalk && AudioManager.sound)
            {
                hurtTalk = true;
                switch (hurtSound)
                {
                    case 1:
                        AudioManager.Johnny_Aouch.Play();
                        //AudioManager.Johnny_Aouch.Play();
                        hurtTalkTimer = TimeSpan.FromMilliseconds(AudioManager.Johnny_Aouch.Duration.TotalMilliseconds);
                        break;
                    case 2:
                        AudioManager.Johnny_cheater.Play();
                        hurtTalkTimer = TimeSpan.FromMilliseconds(AudioManager.Johnny_cheater.Duration.TotalMilliseconds);
                        break;
                    case 3:
                        AudioManager.Johnny_Eh.Play();
                        hurtTalkTimer = TimeSpan.FromMilliseconds(AudioManager.Johnny_Eh.Duration.TotalMilliseconds);
                        break;
                    case 4:
                        AudioManager.Johnny_Hmmmh.Play();
                        hurtTalkTimer = TimeSpan.FromMilliseconds(AudioManager.Johnny_Hmmmh.Duration.TotalMilliseconds);
                        break;
                    case 5:
                        AudioManager.Johnny_Wheiiii.Play();
                        hurtTalkTimer = TimeSpan.FromMilliseconds(AudioManager.Johnny_Wheiiii.Duration.TotalMilliseconds);
                        break;
                }
            }
        }

        public void PlayerTwoHurt()
        {
            hurtSound = Game1.random.Next(1, 7);

            if (!hurtTalk && AudioManager.sound)
            {
                hurtTalk = true;
                switch (hurtSound)
                {
                    case 1:
                        AudioManager.Tommy_AH.Play();
                        hurtTalkTimer = TimeSpan.FromMilliseconds(AudioManager.Tommy_AH.Duration.TotalMilliseconds);
                        break;
                    case 2:
                        AudioManager.Tommy_Aooii.Play();
                        hurtTalkTimer = TimeSpan.FromMilliseconds(AudioManager.Tommy_Aooii.Duration.TotalMilliseconds);
                        break;
                    case 3:
                        AudioManager.Tommy_Ao.Play();
                        hurtTalkTimer = TimeSpan.FromMilliseconds(AudioManager.Tommy_Ao.Duration.TotalMilliseconds);
                        break;
                    case 4:
                        AudioManager.Tommy_Ouch.Play();
                        hurtTalkTimer = TimeSpan.FromMilliseconds(AudioManager.Tommy_Ouch.Duration.TotalMilliseconds);
                        break;
                    case 5:
                        AudioManager.Tommy_Ouh.Play();
                        hurtTalkTimer = TimeSpan.FromMilliseconds(AudioManager.Tommy_Ouh.Duration.TotalMilliseconds);
                        break;
                    case 6:
                        AudioManager.Tommy_PleaseDont.Play();
                        hurtTalkTimer = TimeSpan.FromMilliseconds(AudioManager.Tommy_PleaseDont.Duration.TotalMilliseconds);
                        break;
                }
            }
        }

        public void Hit()
        {
            hitSound = Game1.random.Next(1, 4);

            if (AudioManager.sound)
            {
                switch (hitSound)
                {
                    case 1:
                        AudioManager.Punch1.Play();
                        break;
                    case 2:
                        AudioManager.Punch2.Play();
                        break;
                    case 3:
                        AudioManager.Punch3.Play();
                        break;
                }
            }
        }

        public void PlayerOneKillHit()
        {
            hitSound = Game1.random.Next(1, 4);

            if (!hitTalk && AudioManager.sound)
            {
                hitTalk = true;
                switch (hitSound)
                {
                    case 1:
                        AudioManager.Johnny_LightsOut.Play();
                        hitTalkTimer = TimeSpan.FromMilliseconds(AudioManager.Johnny_LightsOut.Duration.TotalMilliseconds);
                        break;
                    case 2:
                        AudioManager.Johnny_screamForMe.Play();
                        hitTalkTimer = TimeSpan.FromMilliseconds(AudioManager.Johnny_screamForMe.Duration.TotalMilliseconds);
                        break;
                    case 3:
                        AudioManager.Johnny_takeThis.Play();
                        hitTalkTimer = TimeSpan.FromMilliseconds(AudioManager.Johnny_takeThis.Duration.TotalMilliseconds);
                        break;
                }
            }
        }

        public void PlayerTwoKillHit()
        {
            hitSound = Game1.random.Next(1, 6);

            if (!hitTalk && AudioManager.sound)
            {
                hitTalk = true;
                switch (hitSound)
                {
                    case 1:
                        AudioManager.Tommy_DirtyHands.Play();
                        hitTalkTimer = TimeSpan.FromMilliseconds(AudioManager.Tommy_DirtyHands.Duration.TotalMilliseconds);
                        break;
                    case 2:
                        AudioManager.Tommy_WienerStrike.Play();
                        hitTalkTimer = TimeSpan.FromMilliseconds(AudioManager.Tommy_WienerStrike.Duration.TotalMilliseconds);
                        break;
                    case 3:
                        AudioManager.Tommy_Nonononono.Play();
                        hitTalkTimer = TimeSpan.FromMilliseconds(AudioManager.Tommy_Nonononono.Duration.TotalMilliseconds);
                        break;
                    case 4:
                        AudioManager.Tommy_LeaveMeAlone.Play();
                        hitTalkTimer = TimeSpan.FromMilliseconds(AudioManager.Tommy_LeaveMeAlone.Duration.TotalMilliseconds);
                        break;
                    case 5:
                        AudioManager.Tommy_OhPlease.Play();
                        hitTalkTimer = TimeSpan.FromMilliseconds(AudioManager.Tommy_OhPlease.Duration.TotalMilliseconds);
                        break;
                }
            }
        }

        public Vector2 GetPos
        {
            get
            {
                return pos;
            }
        }

        public Rectangle GetRec
        {
            get
            {
                return boundingBox;
            }
        }
    }
}
