using System;
using System.Collections.Generic;
using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Timers;

namespace SpaceTaxi_1.Taxi {
    public class Player : IGameEventProcessor<object> {
        private readonly DynamicShape shape;
        private readonly Image taxiBoosterOffImageLeft;
        private readonly Image taxiBoosterOffImageRight;
        private Boolean boolMoveLeft;
        private Boolean boolMoveRight;
        private Boolean boolMoveUp;

        public Boolean BoolPassenger;
        public Customer CurrentCustomer;

        private Vec2F gravity = new Vec2F(0.0f, -0.00002f);
        private Vec2F moveLeft = new Vec2F(0.00005f, 0.0f);
        private Vec2F moveRight = new Vec2F(-0.00005f, 0.0f);
        private Vec2F moveUp = new Vec2F(0.0f, 0.00005f);
        public int Score;        
        private Orientation taxiOrientation;

        private List<Image> taxiStrides;

        public Player() {
            shape = new DynamicShape(new Vec2F(), new Vec2F());
            taxiBoosterOffImageLeft =
                new Image(Path.Combine("Assets", "Images", "Txi_Thrust_None.png"));
            taxiBoosterOffImageRight =
                new Image(Path.Combine("Assets", "Images", "Txi_Thrust_None_Right.png"));

            taxiStrides = new List<Image>();
            Entity = new Entity(shape, taxiBoosterOffImageLeft);
            SetExtent(0.1f, 0.05f);

            SpaceBus.GetBus().Subscribe(GameEventType.PlayerEvent, this);


            BoolPassenger = false;
            CurrentCustomer = null;
            Score = 0;
        }

        public Entity Entity { get; }
        
        /// <summary>
        /// Processes events related to player movement.
        /// </summary>
        /// <param name="eventType">The current GameEventType</param>
        /// <param name="gameEvent">The active event that are send between system parts.</param>
        public void ProcessEvent(GameEventType eventType, GameEvent<object> gameEvent) {
            if (eventType == GameEventType.PlayerEvent) {
                switch (gameEvent.Message) {
                case "BOOSTER_UPWARDS":
                    MovementUp();
                    break;
                case "BOOSTER_TO_LEFT":
                    taxiOrientation = Orientation.Left;
                    MovementRight();
                    break;
                case "BOOSTER_TO_RIGHT":
                    taxiOrientation = Orientation.Right;
                    MovementLeft();
                    break;
                case "STOP_ACCELERATE_LEFT":
                    StopMovementLeft();
                    break;
                case "STOP_ACCELERATE_RIGHT":
                    StopMovementRight();
                    break;
                case "STOP_ACCELERATE_UP":
                    StopMovementUp();                    
                    break;
                }
            }
        }
        
        /// <summary>
        /// Sets the position of the player.
        /// </summary>
        /// <param name="x">the x-coordinate.</param>
        /// <param name="y">the y-coordinate.</param>
        public void SetPosition(float x, float y) {
            shape.Position.X = x;
            shape.Position.Y = y;
        }

        /// <summary>
        /// Sets the extent of the player.
        /// </summary>
        /// <param name="width">x-range of the extent.</param>
        /// <param name="height">y-range of the extent.</param>
        public void SetExtent(float width, float height) {
            shape.Extent.X = width;
            shape.Extent.Y = height;
        }
        
        /// <summary>
        /// Renders the player.
        /// </summary>
        public void RenderPlayer() {            
            Entity.Image = taxiOrientation == Orientation.Left
                ? taxiBoosterOffImageLeft
                : taxiBoosterOffImageRight;
            Entity.RenderEntity();
            ActivateThrusters();
            foreach (var stride in taxiStrides) {
                stride.Render(shape);
            }
        }
        
        /// <summary>
        /// Applies gravity to the player.
        /// </summary>
        public void Gravity() {
            shape.Direction += gravity;
        }
        
        /// <summary>
        /// Sets taxiStrides to the correct ImageStride given the corresponding orientation and
        /// current pressed keys.
        /// </summary>
        private void ActivateThrusters() {
            if (boolMoveUp) {
                if (taxiOrientation == Orientation.Left) {
                    taxiStrides = ImageStride.CreateStrides(2,
                        Path.Combine("Assets", "Images", "Txi_Thrust_Bottom_Right.png"));
                } else {
                    taxiStrides = ImageStride.CreateStrides(2,
                        Path.Combine("Assets", "Images", "Txi_Thrust_Bottom.png"));
                }
            }

            if (boolMoveLeft) {
                if (boolMoveUp) {
                    taxiStrides = ImageStride.CreateStrides(2,
                        Path.Combine("Assets", "Images", "Txi_Thrust_Bottom_Back_Right.png"));
                } else {
                    taxiStrides = ImageStride.CreateStrides(2,
                        Path.Combine("Assets", "Images", "Txi_Thrust_Back_Right.png"));
                }
            }

            if (boolMoveRight) {
                if (boolMoveUp) {
                    taxiStrides = ImageStride.CreateStrides(2,
                        Path.Combine("Assets", "Images", "Txi_Thrust_Bottom_Back.png"));
                } else {
                    taxiStrides = ImageStride.CreateStrides(2,
                        Path.Combine("Assets", "Images", "Txi_Thrust_Back.png"));
                }
            }

            if (boolMoveUp == false && boolMoveRight == false && boolMoveLeft == false) {
                taxiStrides.Clear();
            }
        }
        
        /// <summary>
        /// Sets boolMoveUp to true. This is used in checking which keys are currently pressed.
        /// </summary>
        private void MovementUp() {
            boolMoveUp = true;
        }
        
        /// <summary>
        /// Sets boolMoveLeft to true. This is used in checking which keys are currently pressed.
        /// </summary>
        private void MovementLeft() {
            boolMoveLeft = true;
        }
        
        /// <summary>
        /// Sets boolMoveRight to true. This is used in checking which keys are currently pressed.
        /// </summary>
        private void MovementRight() {
            boolMoveRight = true;
        }
        
        /// <summary>
        /// Sets boolMoveLeft to false. This is used in checking which keys are currently pressed.
        /// </summary>
        private void StopMovementRight() {
            boolMoveLeft = false;
        }

        /// <summary>
        /// Sets boolMoveRight to false. This is used in checking which keys are currently pressed.
        /// </summary>
        private void StopMovementLeft() {
            boolMoveRight = false;
        }
        
        /// <summary>
        /// Sets boolMoveUp to false. This is used in checking which keys are currently pressed.
        /// </summary>
        private void StopMovementUp() {
            boolMoveUp = false;
        }
        
        /// <summary>
        /// Adds "force" to current direction of the player.
        /// </summary>
        public void Movement() {
            if (boolMoveUp) {
                shape.Direction += moveUp;
            }

            if (boolMoveLeft) {
                shape.Direction += moveLeft;
            }

            if (boolMoveRight) {
                shape.Direction += moveRight;
            }
        }
        
        /// <summary>
        /// Sets BoolPassenger to false, currentCustomer to null and restarts the timer.
        /// </summary>
        public void DropOff() {
            if (BoolPassenger) {
                BoolPassenger = false;
                CurrentCustomer = null;
                StaticTimer.RestartTimer();
            }
        }
        
        /// <summary>
        /// Upon failing a delivery, a GameStateEvent is registered such that the active
        /// GameStateType is set to GameOver.
        /// </summary>
        public void FailedDelivery() {
            if (BoolPassenger && CurrentCustomer.TimeLimit < StaticTimer.GetElapsedSeconds()) {
                SpaceBus.GetBus().RegisterEvent(
                    GameEventFactory<object>.CreateGameEventForAllProcessors
                    (GameEventType.GameStateEvent, this,
                        "CHANGE_STATE", "GAME_OVER", ""));
            }
        }
        
        /// <summary>
        /// Splits the string containing information about the drop-off platform of the
        /// currently held passenger.
        /// </summary>
        private void SplitPlatform() {
            if (CurrentCustomer.Platform.Length > 1) {
                CurrentCustomer.Platform = CurrentCustomer.Platform[1].ToString();
            }
        }
        
        /// <summary>
        /// Sets BoolPassenger to true, CurrrentCustomer to the given Customer,
        /// calls SplitPlatform(), and restarts the timer. 
        /// </summary>
        /// <param name="customer">The customer that is picked up.</param>
        public void PickUp(Customer customer) {
            if (!BoolPassenger) {
                BoolPassenger = true;
                CurrentCustomer = customer;
                SplitPlatform();
                StaticTimer.RestartTimer();
            }
        }
    }
}