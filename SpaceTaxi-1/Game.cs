using System;
using System.Collections.Generic;
using DIKUArcade;
using DIKUArcade.EventBus;
using DIKUArcade.Timers;
using SpaceTaxi_1.SpaceStates;

namespace SpaceTaxi_1 {
    public class Game : IGameEventProcessor<object> {
        private GameTimer gameTimer;
        private StateMachine stateMachine;
        private Window win;

        public Game() {
            // window
            win = new Window("Space Taxi Game v0.1", 500, AspectRatio.R1X1);

            // event bus
            SpaceBus.GetBus().InitializeEventBus(new List<GameEventType> {
                GameEventType.InputEvent, // key press / key release
                GameEventType.WindowEvent, // messages to the window, e.g. CloseWindow()
                GameEventType.GameStateEvent, // commands issued to the player object, e.g. move,
                GameEventType.PlayerEvent // destroy, receive health, etc.
            });
            stateMachine = new StateMachine();
            win.RegisterEventBus(SpaceBus.GetBus());

            // game timer
            gameTimer = new GameTimer(60); // 60 UPS, no FPS limit


            // event delegation
            SpaceBus.GetBus().Subscribe(GameEventType.InputEvent, this);
            SpaceBus.GetBus().Subscribe(GameEventType.WindowEvent, this);
            SpaceBus.GetBus().Subscribe(GameEventType.PlayerEvent, this);
            SpaceBus.GetBus().Subscribe(GameEventType.GameStateEvent, this);
        }
        
        /// <summary>
        /// Given a GameEventType and a GameEvent, calls CloseWindow(), KeyPress() or
        /// KeyRelease, depending on the GameEventType and GameEvent.
        /// </summary>
        /// <param name="eventType">The identified event.</param>
        /// <param name="gameEvent">The active event that are send between system parts.</param>
        public void ProcessEvent(GameEventType eventType, GameEvent<object> gameEvent) {
            if (eventType == GameEventType.WindowEvent) {
                switch (gameEvent.Message) {
                case "CLOSE_WINDOW":
                    win.CloseWindow();
                    break;
                }
            } else if (eventType == GameEventType.InputEvent) {
                switch (gameEvent.Parameter1) {
                case "KEY_PRESS":
                    KeyPress(gameEvent.Message);
                    break;
                case "KEY_RELEASE":
                    KeyRelease(gameEvent.Message);
                    break;
                }
            }
        }
        
        /// <summary>
        /// The Game Loop ("runs" the game).
        /// </summary>
        public void GameLoop() {
            while (win.IsRunning()) {
                gameTimer.MeasureTime();

                while (gameTimer.ShouldUpdate()) {
                    win.PollEvents();
                    SpaceBus.GetBus().ProcessEvents();
                    stateMachine.ActiveState.UpdateGameLogic();
                }

                if (gameTimer.ShouldRender()) {
                    win.Clear();
                    stateMachine.ActiveState.RenderState();
                    win.SwapBuffers();
                }

                if (gameTimer.ShouldReset()) {
                    // 1 second has passed - display last captured ups and fps from the timer
                    win.Title = "Space Taxi | UPS: " + gameTimer.CapturedUpdates + ", FPS: " +
                                gameTimer.CapturedFrames;
                }
            }
        }
        
        /// <summary>
        /// Given a string corresponding to a pressed key, either calls CloseWindow(),
        /// SaveScreenShot(), or registers an PlayerEvent.
        /// </summary>
        /// <param name="key">The pressed key as a string.</param>
        public void KeyPress(string key) {
            switch (key) {
            case "KEY_ESCAPE":
                win.CloseWindow();
                break;
            case "KEY_F12":
                Console.WriteLine("Saving screenshot");
                win.SaveScreenShot();
                break;
            case "KEY_UP":
                SpaceBus.GetBus().RegisterEvent(
                    GameEventFactory<object>.CreateGameEventForAllProcessors(
                        GameEventType.PlayerEvent, this, 
                        "BOOSTER_UPWARDS", "", ""));
                break;
            case "KEY_LEFT":
                SpaceBus.GetBus().RegisterEvent(
                    GameEventFactory<object>.CreateGameEventForAllProcessors(
                        GameEventType.PlayerEvent, this, 
                        "BOOSTER_TO_LEFT", "", ""));
                break;
            case "KEY_RIGHT":
                SpaceBus.GetBus().RegisterEvent(
                    GameEventFactory<object>.CreateGameEventForAllProcessors(
                        GameEventType.PlayerEvent, this, 
                        "BOOSTER_TO_RIGHT", "", ""));
                break;
            }
        }
        
        /// <summary>
        /// Given a string corresponding to a released key, registers a PlayerEvent.
        /// </summary>
        /// <param name="key">The released key as a string.</param>
        public void KeyRelease(string key) {
            switch (key) {
            case "KEY_LEFT":
                SpaceBus.GetBus().RegisterEvent(
                    GameEventFactory<object>.CreateGameEventForAllProcessors(
                        GameEventType.PlayerEvent, this, 
                        "STOP_ACCELERATE_LEFT", "", ""));
                break;
            case "KEY_RIGHT":
                SpaceBus.GetBus().RegisterEvent(
                    GameEventFactory<object>.CreateGameEventForAllProcessors(
                        GameEventType.PlayerEvent, this, 
                        "STOP_ACCELERATE_RIGHT", "", ""));
                break;
            case "KEY_UP":
                SpaceBus.GetBus().RegisterEvent(
                    GameEventFactory<object>.CreateGameEventForAllProcessors(
                        GameEventType.PlayerEvent, this, 
                        "STOP_ACCELERATE_UP", "", ""));
                break;
            }
        }
    }
}