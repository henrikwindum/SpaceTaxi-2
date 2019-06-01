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
        private Vec2F stopMovement = new Vec2F(0.0f, 0.0f);
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
                case "STOP_ACCELERATE_RIGHT":
                case "STOP_ACCELERATE_UP":
                    StopMovement();
                    break;
                }
            }
        }

        public void SetPosition(float x, float y) {
            shape.Position.X = x;
            shape.Position.Y = y;
        }

        public void SetExtent(float width, float height) {
            shape.Extent.X = width;
            shape.Extent.Y = height;
        }

        public void RenderPlayer() {
            //TODO: Next version needs animation. Skipped for clarity.
            Entity.Image = taxiOrientation == Orientation.Left
                ? taxiBoosterOffImageLeft
                : taxiBoosterOffImageRight;
            Entity.RenderEntity();
            ActivateThrusters();
            foreach (var stride in taxiStrides) {
                stride.Render(shape);
            }
        }

        public void Gravity() {
            shape.Direction += gravity;
        }

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

        private void MovementUp() {
            boolMoveUp = true;
        }

        private void MovementLeft() {
            boolMoveLeft = true;
        }

        private void MovementRight() {
            boolMoveRight = true;
        }

        private void StopMovement() {
            boolMoveUp = false;
            boolMoveRight = false;
            boolMoveLeft = false;
        }


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

        public void DropOff() {
            if (BoolPassenger) {
                BoolPassenger = false;
                CurrentCustomer = null;
                StaticTimer.RestartTimer();
            }
        }

        public void FailedDelivery() {
            if (BoolPassenger && CurrentCustomer.TimeLimit < StaticTimer.GetElapsedSeconds()) {
                SpaceBus.GetBus().RegisterEvent(
                    GameEventFactory<object>.CreateGameEventForAllProcessors
                    (GameEventType.GameStateEvent, this,
                        "CHANGE_STATE", "GAME_OVER", ""));
            }
        }

        private void SplitPlatform() {
            if (CurrentCustomer.Platform.Length > 1) {
                CurrentCustomer.Platform = CurrentCustomer.Platform[1].ToString();
            }
        }

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