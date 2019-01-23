﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Pong.Screens.Events;
using Pong.Screens.GameStateManagement;
using Pong.StateManager;
using System;

namespace Pong.Screens
{
    internal class WinnerScreen : GameScreen
    {
        private readonly string _message;
        private Texture2D _gradientTexture;


        public event EventHandler<PlayerIndexEventArgs> Accepted;

        #region Initialization

        /// <summary>
        ///     Constructor automatically includes the standard "A=ok"
        ///     usage text prompt.
        /// </summary>
        public WinnerScreen(string message)
            : this(message, true)
        {
        }


        /// <summary>
        ///     Constructor lets the caller specify whether to include the standard
        ///     "A=ok, B=cancel" usage text prompt.
        /// </summary>
        public WinnerScreen(string message, bool includeUsageText)
        {
            const string usageText = "\nEnter to continue.";

            if (includeUsageText)
            {
                _message = message + usageText;
            }
            else
            {
                _message = message;
            }

            IsPopup = true;

            TransitionOnTime = TimeSpan.FromSeconds(0.2);
            TransitionOffTime = TimeSpan.FromSeconds(0.2);
        }


        /// <summary>
        ///     Loads graphics content for this screen. This uses the shared ContentManager
        ///     provided by the Game class, so the content will remain loaded forever.
        ///     Whenever a subsequent MessageBoxScreen tries to load this same content,
        ///     it will just get back another reference to the already loaded data.
        /// </summary>
        public override void LoadContent()
        {
            ContentManager content = ScreenManager.Game.Content;
            // ToDo: Get a gradient to fix this.
            _gradientTexture = content.Load<Texture2D>("Blank");
        }

        #endregion

        #region Handle Input

        /// <summary>
        ///     Responds to user input, accepting or cancelling the message box.
        /// </summary>
        public override void HandleInput(InputState input)
        {
            PlayerIndex playerIndex;

            // We pass in our ControllingPlayer, which may either be null (to
            // accept input from any player) or a specific index. If we pass a null
            // controlling player, the InputState helper returns to us which player
            // actually provided the input. We pass that through to our Accepted and
            // Cancelled events, so they can tell which player triggered them.
            if (input.IsMenuSelect(ControllingPlayer, out playerIndex))
            {
                // Raise the accepted event, then exit the message box.
                if (Accepted != null)
                {
                    Accepted(this, new PlayerIndexEventArgs(playerIndex));
                }

                LoadingScreen.Load(ScreenManager, null, null, new BackgroundScreen(), new MainMenuScreen());
                //ExitScreen();
            }

        }

        #endregion

        #region Draw

        /// <summary>
        ///     Draws the message box.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            SpriteFont font = ScreenManager.Font;

            // Darken down any other screens that were drawn beneath the popup.
            ScreenManager.FadeBackBufferToBlack(TransitionAlpha * 2 / 3);

            // Center the message text in the viewport.
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            var viewportSize = new Vector2(viewport.Width, viewport.Height);
            Vector2 textSize = font.MeasureString(_message);
            Vector2 textPosition = (viewportSize - textSize) / 2;

            // The background includes a border somewhat larger than the text itself.
            const int hPad = 32;
            const int vPad = 16;

            var backgroundRectangle = new Rectangle((int)textPosition.X - hPad,
                                                    (int)textPosition.Y - vPad,
                                                    (int)textSize.X + hPad * 2,
                                                    (int)textSize.Y + vPad * 2);
            // Fade the popup alpha during transitions.
            Color color = Color.White * TransitionAlpha;

            spriteBatch.Begin();

            // Draw the background rectangle.
            spriteBatch.Draw(_gradientTexture, backgroundRectangle, color);

            // Draw the message box text.
            spriteBatch.DrawString(font, _message, textPosition, color);

            spriteBatch.End();
        }

        #endregion
    }
}
