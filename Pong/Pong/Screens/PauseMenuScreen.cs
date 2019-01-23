﻿using Pong.Screens.Controls;
using Pong.Screens.Events;
using Pong.Screens.GameStateManagement;

namespace Pong.Screens
{
    /// <summary>
    ///     The pause menu comes up over the top of the game,
    ///     giving the player options to resume or quit.
    /// </summary>
    public class PauseMenuScreen : MenuScreen
    {
        #region Initialization

        /// <summary>
        ///     Constructor.
        /// </summary>
        public PauseMenuScreen()
        {
            MainMenuEntries.Add(new MainMenuEntry("Paused", false));
            // Create our menu entries.
            var resumeGameMenuEntry = new MenuEntry("Resume Game", false);
            var quitGameMenuEntry = new MenuEntry("Quit Game", false);

            // Hook up menu event handlers.
            resumeGameMenuEntry.Selected += OnCancel;
            quitGameMenuEntry.Selected += QuitGameMenuEntrySelected;

            // Add entries to the menu.
            MenuEntries.Add(resumeGameMenuEntry);
            MenuEntries.Add(quitGameMenuEntry);
        }

        #endregion

        #region Handle Input

        /// <summary>
        ///     Event handler for when the Quit Game menu entry is selected.
        /// </summary>
        private void QuitGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            const string message = "Are you sure you want to quit this game?";

            var confirmQuitMessageBox = new MessageBoxScreen(message);

            confirmQuitMessageBox.Accepted += ConfirmQuitMessageBoxAccepted;

            ScreenManager.AddScreen(confirmQuitMessageBox, ControllingPlayer);
        }


        /// <summary>
        ///     Event handler for when the user selects ok on the "are you sure
        ///     you want to quit" message box. This uses the loading screen to
        ///     transition from the game back to the main menu screen.
        /// </summary>
        private void ConfirmQuitMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, null, null, new BackgroundScreen(), new MainMenuScreen());
        }

        #endregion
    }
}